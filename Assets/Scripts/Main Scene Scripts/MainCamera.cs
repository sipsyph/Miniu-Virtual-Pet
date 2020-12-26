using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static bool moveCameraLeft, moveCameraRight;
    private bool hittingBounds;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PushAwayFromBounds();
        CameraMovement();
    }

    void CameraMovement()
    {
        if(SupervisorAndUI.cameraMode)
        {
            if(moveCameraLeft)
            {
                moveCameraRight = false;
                this.transform.Translate(-Vector3.right * 4.0f * Time.deltaTime);
            }
            if(moveCameraRight)
            {
                moveCameraLeft = false;
                this.transform.Translate(Vector3.right * 4.0f * Time.deltaTime);
            }
        }
    }

    void PushAwayFromBounds()
    {
        if(hittingBounds)
        {
            if(moveCameraLeft)
            {
                this.transform.Translate(Vector3.right * 7.0f * Time.deltaTime);
                moveCameraLeft = false;
            }
            if(moveCameraRight)
            {
                this.transform.Translate(-Vector3.right * 7.0f * Time.deltaTime);
                moveCameraRight = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Colliding with Main CAmera: "+collision.transform.name);
        if (collision.transform.tag == "Bounds")
        {
            hittingBounds = true;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Exit Colliding with Main CAmera: "+collision.transform.name);
        if (collision.transform.tag == "Bounds")
        {
            hittingBounds = false;

        }
    }
}
