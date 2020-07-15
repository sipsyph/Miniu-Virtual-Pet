using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Transform playObjectsParentNonStatic, throwParentNonStatic, currentHeldObjNonStatic, 
    throwAwayObjNonStatic, paintCanvasObjNonStatic, paintingAreaObjNonStatic;
    public static Transform playObjectsParent, throwParent, currentHeldObj, throwAwayObj, paintCanvasObj,
    paintingAreaObj;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Start()
    {
        playObjectsParent = playObjectsParentNonStatic;
        throwParent = throwParentNonStatic;
        currentHeldObj = currentHeldObjNonStatic;
        throwAwayObj = throwAwayObjNonStatic;
        paintCanvasObj = paintCanvasObjNonStatic;
        paintingAreaObj = paintingAreaObjNonStatic;
    }

    void Update()
    {
        
    }
}
