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

    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject leftLeg;
    public GameObject rightLeg;

    // PLAYER ROTATION ANGLES
    private float turnStart;
    private float turnEnd;
    private float turnTime;
    private float turnFrames = 20;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public int clickX;
    [HideInInspector] public int clickY;
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
        Debug.Log(output);
    }

    #endregion

    void Start()
    {
        updateMoveSpeed();
        frameNum = -1;
        moveTime = 0;
        turnTime = -1;
        xDir = 1;
        yDir = -1;
        direction = 1f;
        // Save copy of original rotations of each limb
        leftArmRot = leftArm.transform.eulerAngles.x;
        rightArmRot = rightArm.transform.eulerAngles.x;
        leftLegRot = leftLeg.transform.eulerAngles.x;
        rightLegRot = rightLeg.transform.eulerAngles.x;
        isMoving = false;
        xPos = 1;
        yPos = 1;
        clickX = 1;
        clickY = 1;
        previousPos = transform.position;
        previousCamPos = playerCamera.transform.position;
        print(transform.position);
    }

    // Update is called once per frame
    void Update() {
        if(xPos == clickX && yPos == clickY && moveTime <= 0 && frameNum < 0) {
            isMoving = false;
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

    private void movePlayer() {
        if (turnTime >= 0) {
            float range = turnEnd-turnStart;
            if (range > 180) {
                range = -90;
            } else if (range < -180) {
                range = 90;
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

}
