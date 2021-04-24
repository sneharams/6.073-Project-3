using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlaceSoil : MonoBehaviour
{
    public GameObject objects;
    public GameObject grid;
    public GameObject background;
    // [HideInInspector] public Material materialNormal;
    // [HideInInspector] public Material materialHover;
    // [HideInInspector] public Material materialHoverOccupied;
    // [HideInInspector] public GameObject controller;
    // [HideInInspector] public int xPos;
    // [HideInInspector] public int yPos;

    [HideInInspector] public int placeMode;
    [HideInInspector] public bool removeMode;

    void Start() {
        placeMode = 0;
    }

    void OnMouseOver() {
    }

    void OnMouseExit() {
    }


    public void toggle() {
        placeMode = background.GetComponent<HaloContainer>().toggle();
    }


}
