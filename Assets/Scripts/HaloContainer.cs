using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class HaloContainer : MonoBehaviour
{
    // public int x;
    // public int y; // actually z, but easier to picture
    // private Vector3 pos;
    // private float xPos;
    // private float yPos;
    // private float zPos;
    // // private float gridScale = 300;
    // private float gridSize = 40;
    // private float gridDivisions = 10; // grid change divisions

    // public SerializedObject halo;

    // void Start() {
    //     Behaviour halo =(Behaviour)GetComponent ("Halo");
    // }
    // [SerializeField] private bool _isEnabled = true;
    // [SerializeField] private bool _isDisabled = false;
    [HideInInspector] public int state;
    public Material[] materials;
    private bool wasEnabled;
    void Start() {
        state = 0;
        setMaterial();
    }
    public int toggle() {
        // SerializedObject halo = new SerializedObject(GetComponent("Halo"));
        // halo.FindProperty("m_Enabled").boolValue = _isEnabled;
        state += 1;
        if (state > 2) {
            state = 0;
        }
        setMaterial();
        return state;
    }

    private void setMaterial() {
        GetComponent<MeshRenderer>().material = materials[state];
    }


}
