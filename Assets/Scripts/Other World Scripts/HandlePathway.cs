using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePathway : MonoBehaviour {

    private int counter;
    private bool thisPathwayIsHit, finishedCounting, shouldStartCounting;
	// Use this for initialization
	void Start () {
        counter = 0;
        thisPathwayIsHit = false;
        shouldStartCounting = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(shouldStartCounting && HandleOtherWorldAgent.canWalk)
        {
            counter++;
            if (counter > 480)
            {
                this.transform.parent.transform.parent = InstanceController.staticGarbageControllerObj.transform;
                this.transform.parent.gameObject.SetActive(false);
                counter = 0;

                shouldStartCounting = false;
            }
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            shouldStartCounting = true;
            Debug.Log("Agent has exited");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            shouldStartCounting = false;
            Debug.Log("Colliding with agent");
        }
    }

}
