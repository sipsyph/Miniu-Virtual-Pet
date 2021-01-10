//Handles the controls for the ball playObjects in the scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayObject : MonoBehaviour {

    #region Variable Declarations
    public static bool playObjectInteractionOngoing;

    private bool  shouldDropAll = false, shouldDropThis = false;
    public int satiationVal, tasteVal, comfortVal, warmVal, coldVal, entertainmentVal;
    private int ctr, heldCtr;

    public static bool holding;
    private Color oldColor;
    Ray ray;
    RaycastHit hit;

    #endregion

    void Start () 
    {
        oldColor = transform.gameObject.GetComponent<Renderer>().material.color;
        playObjectInteractionOngoing = false;
        ItemController.currentHeldObj = ItemController.throwAwayObj;
        ctr = 0; heldCtr = 0;
    }
	
	void Update () 
    {
        if (ItemController.throwParent.transform.childCount > 0)
        {
            ItemController.holdingItem = true;
            //SupervisorAndUI.lastTouchedObj = SupervisorAndUI.currentTouchedObj;
            SupervisorAndUI.currentTouchedObj = ItemController.throwParent.GetChild(0);

        }
        else if (ItemController.throwParent.transform.childCount <= 0)
        {
            ItemController.holdingItem = false;
        }

        if(MiniuAgent.objCarrierOfAgent.transform.childCount > 1)
        {
            if(this.transform.parent == MiniuAgent.objCarrierOfAgent)
            {
                DropObj();
            }
        }

        if(this.transform.parent == MiniuAgent.objCarrierOfAgent)
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

        if(this.transform.parent != MiniuAgent.objCarrierOfAgent)
        {
            heldCtr = 0;
            shouldDropThis = false;
        }

        if(MiniuAgent.agentIsSleeping)
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

        if(!MiniuAgent.agentIsSleeping)
        {
            ctr = 0;
        }
        
    }
    
    

    void OnMouseDown()
    {
        if (ItemController.holdingItem)
        {
            Debug.Log("Clikcked "+transform.name+" while holding "+ItemController.currentHeldObj);
            //DropObj();
            //readyToThrow = false;
        }

        if (!ItemController.holdingItem)
        {
            ItemController.readyToThrow = false;
            PickUpObj();
        }
    }

    void DropAllObj()
    {
        transform.SetParent(ItemController.playObjectsParent.transform);
        GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<Rigidbody>().detectCollisions = true;
        ItemController.currentHeldObj = ItemController.throwAwayObj;
        //SupervisorAndUI.currentHeldObj = ItemController.currentHeldObj;
        ItemController.previouslyHeldObj = ItemController.throwAwayObj;
    }

    void DropObj()
    {
        Utilities.DropThisObj(this.transform, ItemController.playObjectsParent);
        SupervisorAndUI.lastTouchedObj = SupervisorAndUI.currentTouchedObj;
        SupervisorAndUI.currentTouchedObj = this.transform;
        ItemController.currentHeldObj = ItemController.throwAwayObj;
        //SupervisorAndUI.currentHeldObj = ItemController.currentHeldObj;
        ItemController.previouslyHeldObj = ItemController.throwAwayObj;
    }

    
     
    void PickUpObj()
    {
        //Debug.Log("PickUpObj() " + transform.name);
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.3f);
        transform.gameObject.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        ItemController.currentHeldObj = this.transform;
        ItemController.previouslyHeldObj = ItemController.currentHeldObj;
        SupervisorAndUI.lastTouchedObj = SupervisorAndUI.currentTouchedObj;
        //SupervisorAndUI.currentHeldObj = ItemController.currentHeldObj;
        this.transform.position = ItemController.throwParent.position;
        this.transform.SetParent(ItemController.throwParent);
        //Debug.Log(ItemController.throwParent.transform.childCount);
        playObjectInteractionOngoing = true;
        SupervisorAndUI.playerInteractionOngoing = true;

        SupervisorAndUI.currentTouchedObj = this.transform;
        this.GetComponent<Rigidbody>().isKinematic = true;
        Utilities.IterateRepeatedInteractionCounter();
        Invoke("MakeObjectReadyToThrow", .2f  * Time.deltaTime);
        //MakeObjectReadyToThrow();
    }
    

    void MakeObjectReadyToThrow()
    {
        ItemController.readyToThrow = true;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.name + " " + MiniuAgent.targetObjectToCarry.name);
        if (collision.transform.name == "Miniu Agent" && MiniuAgent.wantToCarryObject 
            && MiniuAgent.targetObjectToCarry.name == this.transform.name)
        {
            MSBodyAnimation.PlayCarryingAnimation();
            this.transform.position = MiniuAgent.objCarrierOfAgent.transform.position;
            this.transform.SetParent(MiniuAgent.objCarrierOfAgent.transform);
            this.transform.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.GetComponent<Rigidbody>().detectCollisions = false;
            MiniuAgent.objectBeingCarried = this.transform;
            MiniuAgent.agentIsNowCarryingSomething = true;
        }

        if(collision.transform.name == "Actable Plane Group" || collision.transform.tag == "Play Object")
        {
            transform.gameObject.GetComponent<Renderer>().material.SetColor("_Color", oldColor);
            transform.gameObject.layer = LayerMask.NameToLayer("Default");
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
    
    
}
