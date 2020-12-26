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
        
        SupervisorAndUI.lastTouchedObj = SupervisorAndUI.currentTouchedObj;
        SupervisorAndUI.currentTouchedObj = this.transform;
        SupervisorAndUI.playerInteractionOngoing = true;

        Utilities.IterateRepeatedInteractionCounter();
    }
}
