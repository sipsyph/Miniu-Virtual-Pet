//Handles the detection of playObjects in the scene through the collider where this script is connected to

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionField : MonoBehaviour {

    #region Variable Declarations
    private float noticedFramesCtr, noticedFramesCtrMax;

    MiniuBrain miniuBrain;

    #endregion

    void Start () {
        miniuBrain = new MiniuBrain();
        noticedFramesCtrMax = 10f;
        noticedFramesCtr = 0.0f;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(!MiniuAgent.agentIsSleeping)
        {
            if(collision.transform.tag == "Play Object")
            {
                //Debug.Log("Object in perception: "+collision.transform.name);
                miniuBrain.AddToMiniuMemoryThisPlayObjWithName(collision.transform);
            }
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if(!MiniuAgent.agentIsSleeping)
        {
            if(collision.transform.tag == "Play Object")
            {
                noticedFramesCtr = noticedFramesCtr + Time.deltaTime;
                if(noticedFramesCtr >= noticedFramesCtrMax)
                {
                    


                    Debug.Log("INCREMENTING FOND VALUE FOR OBJ "+collision.transform.name);
                    miniuBrain.UpdateDefinitionOfPlayObjWithName(collision.transform.name, 0.01f, collision.transform.position);
                    noticedFramesCtr = 0;
                }
            }
        }

    }
}
