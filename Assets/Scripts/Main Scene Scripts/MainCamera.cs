using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static bool moveCameraLeft, moveCameraRight, moveCameraUp, moveCameraDown
    , toggleYMovement, hittingLeftBound, hittingRightBound;
    private bool hittingBounds;
    void Start()
    {
        toggleYMovement = false;
        hittingLeftBound = false;
        hittingRightBound = false;
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
                moveCameraUp = false;
                moveCameraDown = false;
                this.transform.Translate(-Vector3.right * 4.0f * Time.deltaTime);
            }
            if(moveCameraRight)
            {
                moveCameraLeft = false;
                moveCameraUp = false;
                moveCameraDown = false;
                this.transform.Translate(Vector3.right * 4.0f * Time.deltaTime);
            }
            if(moveCameraUp)
            {
                moveCameraDown = false;
                moveCameraRight = false;
                moveCameraLeft = false;
                if(toggleYMovement)
                {
                    this.transform.Translate(Vector3.forward * 4.0f * Time.deltaTime);
                }else{
                    this.transform.Translate(Vector3.up * 4.0f * Time.deltaTime);
                }
                
            }
            if(moveCameraDown)
            {
                moveCameraUp = false;
                moveCameraRight = false;
                moveCameraLeft = false;
                if(toggleYMovement)
                {
                    this.transform.Translate(-Vector3.forward * 4.0f * Time.deltaTime);
                }else{
                    this.transform.Translate(-Vector3.up * 4.0f * Time.deltaTime);
                }
            }
        }
    }

    void PushAwayFromBounds()
    {
        if(hittingBounds)
        {
            //Debug.Log("Camera hitting bounds");
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
        //Debug.Log("Colliding with Main CAmera: "+collision.transform.tag);
        if (collision.transform.tag == "Bounds")
        {
            hittingBounds = true;
            if(moveCameraLeft)
            {
                hittingLeftBound = true;
                hittingRightBound = false;
            }
            if(moveCameraRight)
            {
                hittingRightBound = true;
                hittingLeftBound = false;
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Exit Colliding with Main CAmera: "+collision.transform.tag);
        if (collision.transform.tag == "Bounds")
        {
            hittingBounds = false;
            hittingLeftBound = false;
            hittingRightBound = false;

        }
    }
}
