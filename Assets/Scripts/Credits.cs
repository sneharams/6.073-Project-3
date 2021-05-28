using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Credits : MonoBehaviour
{


    public GameObject coinMeter;

    void Start() {
        // setPosition();
    }

    void OnMouseUp() {
        coinMeter.GetComponent<CoinMeter>().closeScreen();
    }

    // private void setPosition() {
    //     getPosition();
    //     pos = new Vector3(xPos, yPos, zPos);
    //     transform.position = pos;
    // }

    // private void getPosition() {
    //     xPos = (float)((11-x)*(gridSize/gridDivisions)-2);
    //     yPos = (float)transform.position.y;
    //     zPos = (float)((11-y)*(gridSize/gridDivisions)-2);
    // }

}
