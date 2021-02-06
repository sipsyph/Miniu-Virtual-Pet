//Handles the detection of playObjects in the scene through the collider where this script is connected to

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttentionField : MonoBehaviour {

    #region Variable Declarations
    private float noticedFramesCtr, noticedFramesCtrMax;
    public static List<string> objectsInVision = new List<string>();

    MiniuBrain miniuBrain;

    #endregion

    void Start () {
        miniuBrain = new MiniuBrain();
        noticedFramesCtrMax = 10f;
        noticedFramesCtr = 0.0f;
    }

    public static bool ThisObjIsInVision(string objName)
    {
        foreach(string obj in AttentionField.objectsInVision)
        {
            if(obj.Equals(objName))
            {
                Debug.Log("Attention field, This Obj is in vision! "+objName);
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(!MiniuAgent.agentIsSleeping)
        {
            if(collision.transform.tag == "Play Object")
            {
                objectsInVision.Add(collision.transform.name);
                Debug.Log("Adding to vision list: "+collision.transform.name);
                miniuBrain.AddToMiniuMemoryThisPlayObjWithName(collision.transform);
                miniuBrain.UpdateDefinitionOfPlayObjWithName(collision.transform.name, 0.01f, collision.transform.position);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        
        if(collision.transform.tag == "Play Object")
        {
            string objToRemove = collision.transform.name;

            foreach(string playObj in objectsInVision)
            {
                if(playObj.Equals(objToRemove))
                {
                    objectsInVision.Remove(objToRemove);
                    Debug.Log("Removing from vision list: "+objToRemove);
                    break;
                }
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

                    Debug.Log("Updating info for "+collision.transform.name);
                    miniuBrain.UpdateDefinitionOfPlayObjWithName(collision.transform.name, 0.01f, collision.transform.position);
                    noticedFramesCtr = 0;
                }
            }
        }

    }
}
