using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerTransform : MonoBehaviour
{
    public float moveSpeed;

    // Animation Constants
    private static int moveFrames = 120; // player translation
    private static int turnFrames = 60;  // player rotation
    private static int keyFrames = 6;    // number of segments for animation
    private static float moveHeightDelta = -2f; // player height displacement for steps
    private static float keyFrameDelta = moveHeightDelta * keyFrames; // adjust for keyFrames
    private static float moveDelta = 40f;  // horizontal player displacement
    private static float moveKFDelta = moveFrames/keyFrames; // horizontal player displacement for a keyFrame
    
    // Translation Vectors
    private static Vector3 forward = new Vector3( 0f, 0f, -moveDelta);
    private static Vector3 back    = new Vector3( 0f, 0f,  moveDelta);
    private static Vector3 left    = new Vector3( moveDelta, 0f,  0f);
    private static Vector3 right   = new Vector3(-moveDelta, 0f,  0f);
    private Vector3 moveVector; // holds current translation vector

    // Animation 
    private float moveTime;
    private float turnTime;

    // Turn Animation Angles
    private float turnStart;
    private float turnEnd;
    
    private float delta; // keyFrame directional switch



    // Start is called before the first frame update
    void Start()
    {
        moveVector = forward;
        moveTime = 0;
        turnTime = -1;
        turnStart = 180;
        turnEnd = 180;
        delta = keyFrameDelta;
        // print(moveFKDelta);
    }

    // Update is called once per frame
    void Update() {
        Boolean move = false;
        if (Input.GetKey(KeyCode.W)) {
            if (moveTime <= 0) {
                turnStart = turnEnd;
                turnEnd = 180;
                moveVector = forward;
                move = true;
            }
        }
        else if (Input.GetKey(KeyCode.S)) {
            if (moveTime <= 0) {
                turnStart = turnEnd;
                turnEnd = 0;
                moveVector = back;
                move = true;
            }
        }
        else if (Input.GetKey(KeyCode.D)) {
            if (moveTime <= 0) {
                turnStart = turnEnd;
                turnEnd = -90;
                moveVector = right;
                move = true;
            }
        }
        else if (Input.GetKey(KeyCode.A)) {
            if (moveTime <= 0) {
                turnStart = turnEnd;
                turnEnd = 90;
                moveVector = left;
                move = true;
            }
        }

        if (move) {
            moveTime = moveFrames;
            turnTime = -1;
            if (turnStart != turnEnd) {
                turnTime = turnFrames;
            }
        }

        // update to be less dumb
        if (moveTime > 0) {
            float yOff = delta;
            print(moveTime);
            if (moveTime <= moveKFDelta) {
                print("6");
                yOff = -delta;
            } else if (moveTime <= moveKFDelta*2) {
                print("5");
                yOff = delta;
            } else if (moveTime <= moveKFDelta*3) {
                print("4");
                yOff = -delta;
            } else if (moveTime <= moveKFDelta*4) {
                print("3");
                yOff = delta;
            } else if (moveTime <= moveKFDelta*5) {
                print("2");
                yOff = -delta;
            } else {
                print("1");
            }
            movePlayer(yOff);
        }
    }

    private void movePlayer(float yOff) {
        if (turnTime >= 0) {
            float range = turnEnd-turnStart;
            if (range > 180) {
                range = -90;
            } else if (range < -180) {
                range = 90;
            }
            float turnAngle = turnStart + range/turnFrames * (turnFrames-turnTime);
            transform.rotation = Quaternion.Euler(0, turnAngle , 0);
            turnTime -= moveSpeed;
        } else {
            if (moveTime < moveSpeed){
                Vector3 moveVectorKF = new Vector3(moveVector.x, yOff, moveVector.z);
                Vector3 moveDelta = (moveVectorKF * moveTime) / (moveFrames * 10);
                transform.position = transform.position + moveDelta;
                moveTime = 0;
            } else {
                Vector3 moveVectorKF = new Vector3(moveVector.x, yOff, moveVector.z);
                Vector3 moveDelta = (moveVectorKF * moveSpeed) / (moveFrames * 10);
                transform.position = transform.position + moveDelta;
                moveTime -= moveSpeed;
            }
            // if (moveTime <= 0) {
            //     Debug.Log("player end");
            // }
        }
    }

    private void print(String text) {
        Debug.Log(text);
    }


}
