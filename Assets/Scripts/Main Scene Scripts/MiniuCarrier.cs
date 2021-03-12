using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniuCarrier : MonoBehaviour {

    public static bool agentIsHoldingSomething;

	// Use this for initialization
	void Start () {
        agentIsHoldingSomething = false;

    }
	
	// Update is called once per frame
	void Update () {
        CheckIfAgentIsHoldingSomething();

    }

    void CheckIfAgentIsHoldingSomething()
    {
        if(this.transform.parent.childCount>0)
        {
            agentIsHoldingSomething = true;
        }else{
            agentIsHoldingSomething = false;
        }
    }

    public static void DropAllCarriedObj()
    {
        //transform.DetachChildren();
    }


}
