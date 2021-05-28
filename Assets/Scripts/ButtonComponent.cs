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
    private int hoverNum;

    void Start() {
        hoverNum = 0;
    }

    void OnEnable() {
        hoverNum = 0;
    }

    void OnMouseOver() {
        button.GetComponent<OptionButton>().isHover += 1;
        hoverNum += 1;
    }

    void OnMouseExit() {
        button.GetComponent<OptionButton>().isHover -= hoverNum;
        hoverNum = 0;
    }


    void OnMouseUp() {
        button.GetComponent<OptionButton>().toggle();
    }


}
