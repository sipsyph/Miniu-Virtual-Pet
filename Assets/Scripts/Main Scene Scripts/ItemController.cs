using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Transform playObjectsParentNonStatic, throwParentNonStatic, currentHeldObjNonStatic, 
    throwAwayObjNonStatic, paintCanvasObjNonStatic, paintingAreaObjNonStatic, previouslyHeldObjNonStatic;
    public static Transform playObjectsParent, throwParent, currentHeldObj, throwAwayObj, paintCanvasObj,
    paintingAreaObj, previouslyHeldObj;

    public static bool readyToThrow = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Start()
    {
        playObjectsParent = playObjectsParentNonStatic;
        throwParent = throwParentNonStatic;
        currentHeldObj = throwAwayObjNonStatic;
        throwAwayObj = throwAwayObjNonStatic;
        paintCanvasObj = paintCanvasObjNonStatic;
        paintingAreaObj = paintingAreaObjNonStatic;
        previouslyHeldObj = throwAwayObjNonStatic;
    }

    void PickUpObj(Transform targetObj)
    {
        
        ItemController.currentHeldObj = targetObj;
        ItemController.previouslyHeldObj = ItemController.currentHeldObj;
        SupervisorAndUI.lastTouchedObj = SupervisorAndUI.currentTouchedObj;
        SupervisorAndUI.currentHeldObj = ItemController.currentHeldObj;
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
