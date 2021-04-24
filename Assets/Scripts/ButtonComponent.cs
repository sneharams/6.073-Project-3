using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ButtonComponent : MonoBehaviour
{
    public GameObject button;

    // void Start() {
    //     placeMode = false;
    // }

    // void Update() {
    //     if (placeMode) {
    //         background.GetComponent<HaloContainer>().enable();
    //     } else {
    //         background.GetComponent<HaloContainer>().disable();
    //     }
    // }


    void OnMouseOver() {
    }

    void OnMouseExit() {
    }


    void OnMouseUp() {
        button.GetComponent<PlaceSoil>().toggle();
    }


}
