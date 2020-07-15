using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlane : MonoBehaviour {

    public static bool planeBeingTouchedInteractionOngoing;
    Ray ray;
    RaycastHit hit;
    public GameObject prefab;
    public static bool GoDestroy;
    public static Transform lastCube, currentCube;
    private int resetPlaneCounter = 0, resetPlaneCounterMax = 600;
    // Use this for initialization
    void Start () {
        planeBeingTouchedInteractionOngoing = false;
        GoDestroy = false;
    }
	
	// Update is called once per frame
	void Update () {
        //CreatePlayerTouchOnPlaneIndicator();
        //commented for now cuz its weird that agent notices plane hit instead of play object being dropped
    }

    void CreatePlayerTouchOnPlaneIndicator()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "Actable Plane Group")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GoDestroy = true;
                    //GameObject obj = Instantiate(prefab, new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z),
                    //Quaternion.identity) as GameObject;
                    Utilities.SetLastPlaneClickedPosition(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                    Debug.Log("Plane CLicked");
                    Utilities.IterateRepeatedInteractionCounter();
                    HandleSupervisor.playerInteractionOngoing = true;
                    /*if(HandleSupervisor.lastTouchedObj.tag == "Play Object")
                    {
                        Debug.Log("Dumaan sa if");
                        HandleSupervisor.lastTouchedObj = this.transform;
                    }else{
                        HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
                        HandleSupervisor.currentTouchedObj = this.transform;
                    }*/

                    HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
                    HandleSupervisor.currentTouchedObj = this.transform;

                    planeBeingTouchedInteractionOngoing = true;
                }
            }
        }
    }
}
