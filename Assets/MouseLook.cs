using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //Sensitivity variable of mouse camera look
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //Locks in cursor to game scene
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        //Clamps looking to 90 degrees up or down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Doing rotation for up and down like this to allow clamping
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 

        //Rotates camera left and right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
