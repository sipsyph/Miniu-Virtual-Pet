//Handles the controls for the ball playObjects in the scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBall : MonoBehaviour {

    #region Variable Declarations
    public static bool ballTouched, ball2Touched, ball3Touched, twigTouched, playobjectInteractionOngoing, isAgentSleeping;

    private bool  shouldDropAll = false, shouldDropThis = false;
    public static bool readyToThrow = false;
    public Transform playObjectsParent, throwParent, currentHeldObj, throwAwayObj;
    private int ctr, heldCtr;

    public static bool holding;
    Ray ray;
    RaycastHit hit;

    #endregion

    void Start () {
        playobjectInteractionOngoing = false;
        readyToThrow = false;
        ItemController.currentHeldObj = ItemController.throwAwayObj;
        HandleSupervisor.currentHeldObj = ItemController.throwAwayObj;
        isAgentSleeping = false;
        ctr = 0; heldCtr = 0;
    }
	
	void Update () 
    {
        if (ItemController.throwParent.transform.childCount > 0)
        {
            holding = true;
            //HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
            HandleSupervisor.currentTouchedObj = ItemController.throwParent.GetChild(0);

        }
        else if (ItemController.throwParent.transform.childCount <= 0)
        {
            holding = false;
        }

        if(HandleAgent.objCarrierOfAgent.transform.childCount > 1)
        {
            
            if(this.transform.parent == HandleAgent.objCarrierOfAgent)
            {
                DropObj();
            }
            
        }

        if(this.transform.parent == HandleAgent.objCarrierOfAgent)
        {
            heldCtr++;

            if(heldCtr > 600)
            {
                shouldDropThis = true;
                
            }

            if(shouldDropThis)
            {
                DropObj();
                heldCtr = 0;
                shouldDropThis = false;
            }
        }

        if(this.transform.parent != HandleAgent.objCarrierOfAgent)
        {
            heldCtr = 0;
            shouldDropThis = false;
        }

        if(isAgentSleeping)
        {
            ctr++;
            if (ctr < 60)
            {
                shouldDropAll = true;
            }

            if (shouldDropAll)
            {
                DropObj();
            }
            
            if(ctr>=60)
            {
                shouldDropAll = false;
                ctr = 60;
            }
        }

        if(!isAgentSleeping)
        {
            ctr = 0;
        }
        
    }
    
    

    void OnMouseDown()
    {
        if (holding)
        {
            DropObj();
            readyToThrow = false;
        }

        if (!holding)
        {
            readyToThrow = false;
            PickUpObj();
        }
    }

    public void PickUpGeneratedSoulFood(Transform soulFoodObj)
    {
        if (holding)
        {
            DropObj();
            readyToThrow = false;
        }

        if (!holding)
        {
            readyToThrow = false;
            PickUpObj(soulFoodObj);
        }
    }
    void DropAllObj()
    {
        transform.SetParent(ItemController.playObjectsParent.transform);
        GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<Rigidbody>().detectCollisions = true;
        ItemController.currentHeldObj = ItemController.throwAwayObj;
        HandleSupervisor.currentHeldObj = ItemController.currentHeldObj;
    }

    void DropObj()
    {
        Utilities.DropThisObj(this.transform, ItemController.playObjectsParent);
        HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
        HandleSupervisor.currentTouchedObj = this.transform;
        ItemController.currentHeldObj = ItemController.throwAwayObj;
        HandleSupervisor.currentHeldObj = ItemController.currentHeldObj;
        
    }

    void PickUpObj(Transform targetObj)
    {
        Debug.Log("PickUpObj(soulFoodObj) " + targetObj.name);
        ItemController.currentHeldObj = targetObj;
        HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
        HandleSupervisor.currentHeldObj = ItemController.currentHeldObj;
        Debug.Log("throw parent: "+ItemController.throwParent+"   targetObj: "+targetObj);
        targetObj.position = ItemController.throwParent.position;
        targetObj.SetParent(ItemController.throwParent);
        playobjectInteractionOngoing = true;

        HandleSupervisor.playerInteractionOngoing = true;

        HandleSupervisor.currentTouchedObj = targetObj;
        this.GetComponent<Rigidbody>().isKinematic = true;
        Utilities.IterateRepeatedInteractionCounter();
        Invoke("MakeObjectReadyToThrow", 1/10);
    }
     
    void PickUpObj()
    {
        Debug.Log("PickUpObj() " + transform.name);
        ItemController.currentHeldObj = this.transform;
        HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
        HandleSupervisor.currentHeldObj = ItemController.currentHeldObj;
        this.transform.position = ItemController.throwParent.position;
        this.transform.SetParent(ItemController.throwParent);
        Debug.Log(ItemController.throwParent.transform.childCount);
        playobjectInteractionOngoing = true;

        HandleSupervisor.playerInteractionOngoing = true;

        HandleSupervisor.currentTouchedObj = this.transform;
        this.GetComponent<Rigidbody>().isKinematic = true;
        Utilities.IterateRepeatedInteractionCounter();
        Invoke("MakeObjectReadyToThrow", 1/10);
    }

    void MakeObjectReadyToThrow()
    {
        Debug.Log("Ready TO Throw");
        readyToThrow = true;
    }


    
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.name + " " + HandleAgent.targetObjectToCarry.name);
        if (collision.transform.name == "Miniu Agent" && HandleAgent.wantToCarryObject 
            && HandleAgent.targetObjectToCarry.name == this.transform.name)
        {
            MSBodyAnimation.PlayCarryingAnimation();
            this.transform.position = HandleAgent.objCarrierOfAgent.transform.position;
            this.transform.SetParent(HandleAgent.objCarrierOfAgent.transform);
            this.transform.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.GetComponent<Rigidbody>().detectCollisions = false;
            HandleAgent.objectBeingCarried = this.transform;
            HandleAgent.agentIsNowCarryingSomething = true;
        }
    }
    
    
}
