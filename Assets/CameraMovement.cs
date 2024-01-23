using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraMovement : MonoBehaviour
{
    //Define variables for the camera movement
    private bool dragPan = false;
    private Vector3 inputDir;
    private Vector2 lastMousePos;
    private float moveSpeed = 8f;
    private float sens = 10f;

    void Update()
    {
        inputDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) inputDir.z = 1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = 1f;
        if (Input.GetKey(KeyCode.Q)) inputDir.y = -1f;
        if (Input.GetKey(KeyCode.E)) inputDir.y = 1f;

        //Collect input data and apply it to a vector 3 
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x + transform.up * inputDir.y;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        //If the right mouse button is down the active drag pan so the user can look around
        if (Input.GetMouseButtonDown(1)) { 
            dragPan = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1)) { dragPan = false; }

        if (dragPan)//if dragpan is true then update the camera rotation
        {
            Vector2 mouseMovDelta = (Vector2)Input.mousePosition - lastMousePos;

            transform.eulerAngles += new Vector3(mouseMovDelta.y * sens * Time.deltaTime * -1, mouseMovDelta.x * sens * Time.deltaTime, 0);

            lastMousePos = Input.mousePosition;
        }

        //Quit if the user presses escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
