using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Paint canvas clicked");
        
        HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
        HandleSupervisor.currentTouchedObj = this.transform;
        HandleSupervisor.playerInteractionOngoing = true;

        Utilities.IterateRepeatedInteractionCounter();
    }
}
