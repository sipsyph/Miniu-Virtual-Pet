using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Transform playObjectsParentNonStatic, throwParentNonStatic, currentHeldObjNonStatic, 
    throwAwayObjNonStatic, paintCanvasObjNonStatic, paintingAreaObjNonStatic, previouslyHeldObjNonStatic;
    public static Transform playObjectsParent, throwParent, currentHeldObj, throwAwayObj, paintCanvasObj,
    paintingAreaObj, previouslyHeldObj;

    public Transform[] playObjectsNonStatic, comfortObjectsNonStatic, entertainmentObjectsNonStatic
    , satiationObjectsNonStatic;
    public static Transform[] playObjects, comfortObjects, entertainmentObjects, satiationObjects;

    private List<Transform> rememberedObjects = new List<Transform>();

    public static bool readyToThrow = false, holdingItem = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Start()
    {
        playObjects = playObjectsNonStatic;
        playObjectsParent = playObjectsParentNonStatic;
        throwParent = throwParentNonStatic;
        currentHeldObj = throwAwayObjNonStatic;
        throwAwayObj = throwAwayObjNonStatic;
        paintCanvasObj = paintCanvasObjNonStatic;
        paintingAreaObj = paintingAreaObjNonStatic;
        previouslyHeldObj = throwAwayObjNonStatic;
    }

    public static Transform objectWithName(string objName)
    {
        foreach(Transform obj in playObjects)
        {
            if(objName == obj.name)
            {
                return obj;
            }
        }
        return null;
    }

    public void AddThisToRememberedObjects(Transform playObj)
    {
        foreach (Transform obj in rememberedObjects)
        {
            Debug.Log(obj.name+" has been looped through");
            if(obj == playObj)
            {
                Debug.Log(playObj+" is already remembered");
                return;
            }
        }

        rememberedObjects.Add(playObj);
        Debug.Log("Added to objects list: "+playObj.name);
        Debug.Log("Remember objects list: "+rememberedObjects);
        return;
    }

    public string DetermineWhatCategoryThisPlayObjIs(string playObjName)
    {
        foreach (var obj in playObjects)
        {
            if(playObjName == obj.name)
            {
                if(obj.GetComponent<PlayObject>().comfortVal > 0)
                {
                    return "Comfort";
                }
                if(obj.GetComponent<PlayObject>().entertainmentVal > 0)
                {
                    return "Entertainment";
                }
                if(obj.GetComponent<PlayObject>().satiationVal > 0)
                {
                    return "Satiation";
                }
            }
        }

        return "";
    }

    void PickUpObj(Transform targetObj)
    {
        
        ItemController.currentHeldObj = targetObj;
        ItemController.previouslyHeldObj = ItemController.currentHeldObj;
        SupervisorAndUI.lastTouchedObj = SupervisorAndUI.currentTouchedObj;
        //SupervisorAndUI.currentHeldObj = ItemController.currentHeldObj;
        Debug.Log("throw parent: "+ItemController.throwParent+"   targetObj: "+targetObj);
        targetObj.position = ItemController.throwParent.position;
        targetObj.SetParent(ItemController.throwParent);
        //playObjectInteractionOngoing = true;

        SupervisorAndUI.playerInteractionOngoing = true;

        SupervisorAndUI.currentTouchedObj = targetObj;
        this.GetComponent<Rigidbody>().isKinematic = true;
        Utilities.IterateRepeatedInteractionCounter();
        Invoke("MakeObjectReadyToThrow", 1/10);
    }

    void MakeObjectReadyToThrow()
    {
        readyToThrow = true;
    }
}
