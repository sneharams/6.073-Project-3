using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerController : MonoBehaviour
{
    #region Class Variables

    [System.Serializable] public enum PlayerSpeed {
        Slow,
        Normal,
        Fast
    };

    // PUBLIC VARIABLES
    public PlayerSpeed playerSpeed;
    private float moveSpeed;
    public GameObject playerCamera;
    [Header("Items")]
    public GameObject waterPail;
    public GameObject waterStream;
    [Header("Limbs")]
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject leftLeg;
    public GameObject rightLeg;

    [Header("Misc")]
    public GameObject gridContainer;
    private GridContainer grid;

    // PLAYER ROTATION ANGLES
    private float turnStart;
    private float turnEnd;
    private float turnTime;
    private float turnFrames = 20;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public int clickX;
    [HideInInspector] public int clickY;
    [HideInInspector] public Tuple<int,int> goalSquare;
    private List<Tuple<int,int>> movePath;
    public int xPos;
    public int yPos;

    // PLAYER TRANSLATION CONSTANTS
    private static float bounceDelta = .02f; // player height displacement for steps
    private float bounceDirection; // keyFrame directional switch
    // private float bounceDelta = 2f;
    // private static float bounceDeltaKF = bounceDelta * keyFrames; // adjust for keyFrames
    private float moveDelta;  // horizontal player displacement
    private Vector3 forward;
    private Vector3 back;
    private Vector3 left;
    private Vector3 right;
    private Vector3 up;
    private Vector3 moveVector; // holds current translation vector

    private Vector3 previousPos;
    private Vector3 previousCamPos;
    private float moveTime; // Frames remaining for leg rotation in 1 half-rotation frame
    private static int moveFrames = 20;    // Length of half-rotation of lin frames
    private static int numFrames = 6;
    private int frameNum;       // Current half-rotation frame (6 frames)

    private float xDir;
    private float yDir;
    private float direction;
    private float leftArmRot;
    private float rightArmRot;
    private float leftLegRot;
    private float rightLegRot;

    private float pourTime;
    private float pourFrames = 60f;
    private int pourFrame;

    private int moveType;



    
    #endregion

    #region Console Print Methods
    private void print(params object[] list) 
    {
        string output = "";
        foreach (object item in list) {
            if (item is string) {
                output += item + ": ";
            } else {
                output += item + ", ";
            }
        }
        output = output.Substring(0, output.Length-2);
        // Debug.Log(output);
    }

    #endregion

    void Start()
    {
        updateMoveSpeed();
        moveType = 0;
        frameNum = -1;
        moveTime = 0;
        turnTime = -1;
        xDir = 1;
        yDir = -1;
        direction = 1f;
        turnEnd = 0;
        // Save copy of original rotations of each limb
        leftArmRot = leftArm.transform.eulerAngles.x;
        rightArmRot = rightArm.transform.eulerAngles.x;
        leftLegRot = leftLeg.transform.eulerAngles.x;
        rightLegRot = rightLeg.transform.eulerAngles.x;
        // waterPailRot = waterPail.transform.eulerAngles.z;
        isMoving = false;
        clickX = xPos;
        clickY = yPos;
        goalSquare = Tuple.Create(xPos, xPos);
        movePath = new List<Tuple<int, int>>();
        previousPos = transform.position;
        previousCamPos = playerCamera.transform.position;
        pourTime = 0;
        pourFrame = -1;
        grid = gridContainer.GetComponent<GridContainer>();
    }


    public void setMoveType(int type) {
        moveType = type;
    }
    // Update is called once per frame
    #region State Updates

    void Update() {
        updateMove();
        if (!isMoving) {
            // updatePour();
        }
    }

    private void updatePour() {
        // POUR WATER ANIMATION
        if (pourTime <= 0 && pourFrame < 0 && Input.GetKeyUp(KeyCode.W)) {
            pourFrame = 0;
            pourTime = pourFrames;
        }
        if (pourFrame >= 0) {
            if (pourTime <= 0) {
                pourFrame += 1;
            } else {
                pourWater();
            }
            if (pourFrame > 2) {
                pourFrame = -1;
            }
        }
    }

    private void updateMove() {
        if (moveType != 3)
            updateMoveClick();
        else
            updateMoveKey();
    }

    private void updateMoveClick() {
        // isMoving = false;
        if (xPos == clickX && yPos == clickY && moveTime <= 0 && frameNum < 0) {
            if (movePath.Count > 0) {
                clickX = movePath[0].Item1;
                clickY = movePath[0].Item2;
                movePath.RemoveAt(0);
                // isMoving = true;
            }
        } else {
            // isMoving = true;
        }
        if (frameNum < 0) {
            // if move animation finished/not running
            if (moveTime <= 0) {
                updateMoveSpeed();
                turnStart = turnEnd;
                if (clickX != xPos) {
                    direction *= -1;
                    frameNum = 0;
                    if (clickX < xPos) {
                        turnEnd = 90;
                        moveVector = left;
                        xPos -= 1;
                    } else {
                        turnEnd = -90;
                        moveVector = right;
                        xPos += 1;
                    }
                } else if (clickY != yPos) {
                    direction *= -1;
                    frameNum = 0;
                    if (clickY < yPos) {
                        turnEnd = 0;
                        moveVector = back;
                        yPos -= 1;
                    } else {
                        turnEnd = 180;
                        moveVector = forward;
                        yPos += 1;
                    }
                }
            }
            if (turnStart != turnEnd) {
                turnTime = turnFrames;
            }
        } 
        // update frame variables
        if (frameNum >= 0 && moveTime <= 0 && turnTime < 0) {
            previousPos = transform.position;
            previousCamPos = playerCamera.transform.position;
            leftArmRot = leftArm.transform.eulerAngles.x;
            rightArmRot = rightArm.transform.eulerAngles.x;
            leftLegRot = leftLeg.transform.eulerAngles.x;
            rightLegRot = rightLeg.transform.eulerAngles.x;
            xDir = 1;
            yDir = 1;
            if (frameNum == 0) {
                xDir = -1;
                yDir = -1;
            } else if (frameNum == 1) {
            } else if (frameNum == 2) {
                yDir = -1;
            } else if (frameNum == 3) {
                xDir = -1;
            } else if (frameNum == 4) {
                xDir = -1;
                yDir = -1;
            } else if (frameNum == 5) {
                xDir = 1;
            } 
            moveTime = moveFrames;
            frameNum += 1;
            if (frameNum > numFrames) {
                frameNum = -1;
                moveTime = 0;
            }
        }
        
        if (moveTime > 0 || turnTime >= 0) {
            isMoving = true;
            movePlayer();
        } else if (movePath.Count > 0) {
            isMoving = true;
        } else {
            isMoving = false;
        }
    }

    private void updateMoveKey() {
        isMoving = false;
        if (xPos == clickX && yPos == clickY && moveTime <= 0 && frameNum < 0) {
            // if (movePath.Count > 0) {
            //     clickX = movePath[0].Item1;
            //     clickY = movePath[0].Item2;
            //     movePath.RemoveAt(0);
            //     isMoving = true;
            // }
            if (Input.GetKey(KeyCode.W)) {
                int yNew = yPos + 1;
                if (grid.isEmpty(xPos, yNew)) {
                    clickY = yNew;
                }
            } else if (Input.GetKey(KeyCode.S)) {
                int newY = yPos - 1;
                if (grid.isEmpty(xPos, newY)) {
                    clickY = newY;
                }
            } else if (Input.GetKey(KeyCode.A)) {
                int xNew = xPos - 1;
                if (grid.isEmpty(xNew, yPos)) {
                    clickX = xNew;
                }
            } else if (Input.GetKey(KeyCode.D)) {
                int xNew = xPos + 1;
                if (grid.isEmpty(xNew, yPos)) {
                    clickX = xNew;
                }
            }
        } else {
            isMoving = true;
        }
        if (frameNum < 0) {
            // if move animation finished/not running
            if (moveTime <= 0) {
                updateMoveSpeed();
                turnStart = turnEnd;
                if (clickX != xPos) {
                    direction *= -1;
                    frameNum = 0;
                    if (clickX < xPos) {
                        turnEnd = 90;
                        moveVector = left;
                        xPos -= 1;
                    } else {
                        turnEnd = -90;
                        moveVector = right;
                        xPos += 1;
                    }
                } else if (clickY != yPos) {
                    direction *= -1;
                    frameNum = 0;
                    if (clickY < yPos) {
                        turnEnd = 0;
                        moveVector = back;
                        yPos -= 1;
                    } else {
                        turnEnd = 180;
                        moveVector = forward;
                        yPos += 1;
                    }
                }
            }
            if (turnStart != turnEnd) {
                turnTime = turnFrames;
            }
        } 
        // update frame variables
        if (frameNum >= 0 && moveTime <= 0 && turnTime < 0) {
            previousPos = transform.position;
            previousCamPos = playerCamera.transform.position;
            leftArmRot = leftArm.transform.eulerAngles.x;
            rightArmRot = rightArm.transform.eulerAngles.x;
            leftLegRot = leftLeg.transform.eulerAngles.x;
            rightLegRot = rightLeg.transform.eulerAngles.x;
            xDir = 1;
            yDir = 1;
            if (frameNum == 0) {
                xDir = -1;
                yDir = -1;
            } else if (frameNum == 1) {
            } else if (frameNum == 2) {
                yDir = -1;
            } else if (frameNum == 3) {
                xDir = -1;
            } else if (frameNum == 4) {
                xDir = -1;
                yDir = -1;
            } else if (frameNum == 5) {
                xDir = 1;
            } 
            moveTime = moveFrames;
            frameNum += 1;
            if (frameNum > numFrames) {
                frameNum = -1;
                moveTime = 0;
            }
        }
        
        if (moveTime > 0 || turnTime >= 0) {
            movePlayer();
        }
    }

    public void handleClick(int x, int y) {
        // print(x, y);
        movePath = getPath(Tuple.Create(x,y));
        movePath.RemoveAt(0);
        if (movePath.Count > 0) {
            goalSquare = Tuple.Create(x,y);
        }
    }

    public void setGoalSquare(int x, int y) {
        goalSquare = Tuple.Create(x,y);
    }

    public bool hasPath(int x, int y) {
        // hardcoded
        if (x == 1 && y == 7) {
            List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x+1,y));
            tempPath.RemoveAt(0);
            if (tempPath.Count > 0) {
                movePath = tempPath;
                goalSquare = Tuple.Create(x,y);
                return true;
            }
            return false;
        } 
        else {
            List<Tuple<int,int>> bestPath = new List<Tuple<int,int>>();
            if (grid.isWithinGrid(x-1, y)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x-1,y));
                tempPath.RemoveAt(0);
                print("Path", tempPath.Count, bestPath.Count);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            } 
            else if (grid.isWithinGrid(x+1, y)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x+1,y));
                tempPath.RemoveAt(0);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            } 
            else if (grid.isWithinGrid(x, y-1)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x,y-1));
                tempPath.RemoveAt(0);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            } 
            else if (grid.isWithinGrid(x, y+1)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x,y+1));
                tempPath.RemoveAt(0);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            } 
            else if (grid.isWithinGrid(x-1, y-1)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x-1,y-1));
                tempPath.RemoveAt(0);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            } 
            else if (grid.isWithinGrid(x+1, y-1)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x+1,y-1));
                tempPath.RemoveAt(0);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            } 
            else if (grid.isWithinGrid(x+1, y+1)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x+1,y+1));
                tempPath.RemoveAt(0);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            } 
            else if (grid.isWithinGrid(x-1, y+1)) {
                List<Tuple<int,int>> tempPath = getPath(Tuple.Create(x-1,y+1));
                tempPath.RemoveAt(0);
                if (tempPath.Count > 0) {
                    if (bestPath.Count == 0 || tempPath.Count < bestPath.Count) {
                        bestPath = tempPath;
                    }
                }
            }
            movePath = bestPath;
            if (bestPath.Count > 0) {
                goalSquare = Tuple.Create(x,y);
                return true;
            } else {
                goalSquare = Tuple.Create(xPos,y);
            }
            return false;
        }
    }

    private List<Tuple<int, int>> getPath(Tuple<int, int> endPos) {
        List<List<Tuple<int, int>>> paths = new List<List<Tuple<int, int>>>();
        paths.Add(new List<Tuple<int, int>>());
        paths[0].Add(Tuple.Create(xPos, yPos));
        // Visited Squares
        HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();
        visited.Add(Tuple.Create(xPos, yPos));                  // add player square
        for (int x = 0; x <= grid.gridX+1; x++) {   // add top bottom border squares
            visited.Add(Tuple.Create(x, 0));
            visited.Add(Tuple.Create(x, grid.gridY+1));
        }
        for (int y = 1; y <= grid.gridY; y++) {   // add left right border squares
            visited.Add(Tuple.Create(0, y));
            visited.Add(Tuple.Create(grid.gridX+1, y));
        }
        
        
        while (paths.Count > 0) {
            List<List<Tuple<int, int>>> currPaths = new List<List<Tuple<int, int>>>(paths);
            // print("num paths", currPaths.Count);
            paths.Clear();
            foreach (List<Tuple<int, int>> currPath in currPaths) {
                // print("num squares", currPath.Count);
                // print(currPath[currPath.Count-1]);
                List<Tuple<int,int>> pathCopy = new List<Tuple<int, int>>(currPath);
                // current square
                Tuple<int, int> currSquare = currPath[currPath.Count-1];
                // next squares
                HashSet<Tuple<int, int>> squares = new HashSet<Tuple<int, int>>(); 
                squares.Add(Tuple.Create(currSquare.Item1+1, currSquare.Item2  ));
                squares.Add(Tuple.Create(currSquare.Item1-1, currSquare.Item2  ));
                squares.Add(Tuple.Create(currSquare.Item1  , currSquare.Item2+1));
                squares.Add(Tuple.Create(currSquare.Item1  , currSquare.Item2-1));
                squares.ExceptWith(visited);
                foreach (Tuple<int, int> square in squares) {
                    visited.Add(square);
                    if (grid.isEmpty(square.Item1, square.Item2)) {
                        List<Tuple<int,int>> path = new List<Tuple<int, int>>(pathCopy);
                        path.Add(square);
                        paths.Add(path);
                        // print("square", square, "end", endPos);
                        if (square.Equals(endPos)) {
                            return path;
                        }
                    }
                }            
            }
        }

        return new List<Tuple<int,int>>();
        
    }

    public void turnPlayer(int x, int y) {
        turnStart = turnEnd;
        float yDiff = xPos - x;
        float xDiff = yPos - y;
        turnEnd = (float)Math.Atan2(yDiff, xDiff) * 180.0f / (float)Math.PI;
        if (turnStart != turnEnd) {
            turnTime = turnFrames;
            frameNum = numFrames+1;
        }
        // print(turnStart, turnEnd);
    }
    
    

    #endregion


    #region Water Pour Animation
    private void pourWater() {
        // if (pourFrame == 0 || pourFrame == 3) {
        //     float angle = 30f * ((1/3)*pourFrame);
        //     Vector3 leftDelta = new Vector3 (
        //         leftArmRot+angle*((pourFrames-pourTime)/pourFrames),
        //         leftArm.transform.eulerAngles.y,
        //         leftArm.transform.eulerAngles.z
        //     );
        //     Vector3 rightDelta = new Vector3 (
        //         rightArmRot+angle*((pourFrames-pourTime)/pourFrames),
        //         rightArm.transform.eulerAngles.y,
        //         rightArm.transform.eulerAngles.z
        //     );
        //     leftArm.transform.eulerAngles = leftDelta;
        //     rightArm.transform.eulerAngles = rightDelta;
        // } else {
        //     if (pourFrame == 1) {
        //         float angle = -8f;
        //     } else {
        //         float angle = 8f;
        //     }
        //     Vector3 pailDelta = new Vector3 (
        //         waterPail.transform.eulerAngles.x,
        //         waterPail.transform.eulerAngles.y,
        //         waterPailRot+angle*((pourFrames-pourTime)/pourFrames)
        //     );
        //     waterPail.transform.eulerAngles = pailDelta;
        // }
        // pourTime -= Time.deltaTime * moveSpeed;
    }


    #endregion


    #region Move Animation
    private void movePlayer() {
        if (turnTime >= 0) {
            float range = turnEnd-turnStart;
            if (range > 180) {
                range = range-360;
            } else if (range < -180) {
                range = range+360;
            }
            float turnAngle = turnStart + range/turnFrames * (turnFrames-turnTime);
            transform.rotation = Quaternion.Euler(0, turnAngle, 0);
            turnTime -= moveSpeed;//Time.deltaTime*100*moveSpeed; //moveSpeed;
        } else {
            updatePosition();
            updateLegs();
            updateArms();
            moveTime -= Time.deltaTime*100*moveSpeed; // moveSpeed;
            if (moveTime < 0) {
                moveTime = 0;
                updatePosition();
                updateLegs();
                updateArms();
            }
        }
    }
    
    private void updatePosition() {
        // update XZ position
        float frameBounce = bounceDelta*yDir;
        Vector3 moveDelta = new Vector3(moveVector.x, frameBounce, moveVector.z);
        moveDelta = previousPos + moveDelta * (moveFrames-moveTime)/moveFrames;
        transform.position = moveDelta;
        updateCameraPosition();
    }

    private void updateCameraPosition() {
        Vector3 moveDelta = new Vector3(moveVector.x, 0, moveVector.z);
        moveDelta = previousCamPos + moveDelta * (moveFrames-moveTime)/moveFrames;
        playerCamera.transform.position = moveDelta;
    }

    private void updateArms() {
        float angle = 25f*direction;
        Vector3 leftDelta = new Vector3 (
            leftArmRot+angle*xDir*((moveFrames-moveTime)/moveFrames),
            leftArm.transform.eulerAngles.y,
            leftArm.transform.eulerAngles.z
        );
        Vector3 rightDelta = new Vector3 (
            rightArmRot+angle*xDir*((moveFrames-moveTime)/moveFrames),
            rightArm.transform.eulerAngles.y,
            rightArm.transform.eulerAngles.z
        );
        leftArm.transform.eulerAngles = leftDelta;
        rightArm.transform.eulerAngles = rightDelta;
    }

    private void updateLegs() {
        float angle = 20f*direction;
        Vector3 leftDelta = new Vector3 (
            leftLegRot-angle*xDir*((moveFrames-moveTime)/moveFrames),
            leftLeg.transform.eulerAngles.y,
            leftLeg.transform.eulerAngles.z
        );
        Vector3 rightDelta = new Vector3 (
            rightLegRot-angle*xDir*((moveFrames-moveTime)/moveFrames),
            rightLeg.transform.eulerAngles.y,
            rightLeg.transform.eulerAngles.z
        );
        leftLeg.transform.eulerAngles = leftDelta;
        rightLeg.transform.eulerAngles = rightDelta;
    }

    private void updateMoveSpeed() {
        // update speed
        if (playerSpeed == PlayerSpeed.Slow) {
            moveSpeed = 1f;
        } else if (playerSpeed == PlayerSpeed.Normal) {
            moveSpeed = 2f;
        } else {
            moveSpeed = 4f;
        }
        // update deltas (todo: update to use multipliers for built in Vector3s)
        moveDelta = numFrames/9f;///numFrames/10;  // horizontal player displacement
        forward = new Vector3( 0f, 0f, -moveDelta);
        back    = new Vector3( 0f, 0f,  moveDelta);
        left    = new Vector3( moveDelta, 0f,  0f);
        right   = new Vector3(-moveDelta, 0f,  0f);
        up      = new Vector3(0f, bounceDelta, 0f);
        moveVector = forward; 
    }
    
    #endregion

}
