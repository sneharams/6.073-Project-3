using UnityEngine;
using System.Collections;
 
public class CameraController : MonoBehaviour {
 
    public GameObject controller;
    private Vector3 resetRot;
    private float resetZoom;
    public AudioClip[] backgroundMusic;
    public AudioSource audio;


    void Start () {
        resetRot = controller.transform.eulerAngles;
        resetZoom = Camera.main.fieldOfView;
        
        audio.clip = backgroundMusic[0];
        audio.loop = true;
        audio.Play();
    }

    public void changeMusic(int track)
    {
        audio.Pause();
        if (track < backgroundMusic.Length) {
            audio.clip = backgroundMusic[track];
            audio.Play();
        }
        
    }

    void Update ()
    {
 
    //var speed : float = 1.5f;
    // Rotate Right Click
    if (Input.GetMouseButton(1)) {
        if (controller.transform.eulerAngles.x + Input.GetAxis("Mouse Y") * -1f > 0 &&
            controller.transform.eulerAngles.x + Input.GetAxis("Mouse Y") * -1f < 90) {
            Vector3 rotation = new Vector3 (
                controller.transform.eulerAngles.x + Input.GetAxis("Mouse Y") * -1f,
                controller.transform.eulerAngles.y + Input.GetAxis("Mouse X") * 1f,
                controller.transform.eulerAngles.z
            );
            controller.transform.eulerAngles = rotation;
        } else {
            Vector3 rotation = new Vector3 (
                controller.transform.eulerAngles.x,
                controller.transform.eulerAngles.y + Input.GetAxis("Mouse X") * 1f,
                controller.transform.eulerAngles.z
            );
            controller.transform.eulerAngles = rotation;
        }
    }
            
        //Zoom Out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            if (Camera.main.fieldOfView <= 26)
                Camera.main.fieldOfView += 1f;
        
        //Zoom In
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            if (Camera.main.fieldOfView >= 10)
                Camera.main.fieldOfView -= 1f;
        
        // Reset Camera Position
        if (Input.GetKeyUp(KeyCode.C)) {
            Camera.main.fieldOfView = resetZoom;
            controller.transform.eulerAngles = resetRot;
        }
        //EOF              
    }

        //Add listener for when the value of the Dropdown changes, to take action
        
    public Vector3 getCameraAngle() {
        return controller.transform.eulerAngles;
    }
 
}