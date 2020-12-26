//Handles the detection of playObjects in the scene through the collider where this script is connected to

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionField : MonoBehaviour {

    #region Variable Declarations
    public static bool ballDetected, ball2Detected, ball3Detected;
    public static int ballMemory, ball2Memory, ball3Memory;
    public static string[] rememberedObjects;
    public static string[,] objectsDetected;

    MiniuBrain miniuBrain;

    #endregion

    void Start () {
        miniuBrain = new MiniuBrain();
        rememberedObjects = new string[50];
        objectsDetected = new string[50,50];

    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Play Object")
        {
            //Debug.Log("Object in perception: "+collision.transform.name);
            miniuBrain.AddToMiniuMemoryThisPlayObjWithName(collision.transform.name);
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.transform.tag == "Play Object")
        {
            //miniuBrain.incrementFondnessValueOfPlayObjWithName(collision.transform.name, 0.01f);
        }
    }

    
    void SaveDetectedNewObject(string detectedObj)
    {
        
        for (int i = 0; i <= rememberedObjects.Length; i++)
        {
            if (rememberedObjects[i] == "")
            {
                rememberedObjects[i] = detectedObj;
            }else if(rememberedObjects[i] == detectedObj)
            {
                //Debug.Log(detectedObj+" already exists in memory");
            }

            //Debug.Log(rememberedObjects[i]);
        }
    }

    void AddMemoryPointsToThisObject(string obj, int points)
    {
        for (int i = 0; i <= objectsDetected.Length; i++)
        {
            if (objectsDetected[0, i] == obj)
            {
                objectsDetected[1, i] = points.ToString();

                //Debug.Log(objectsDetected[0,i] + " now has "+ objectsDetected[1, i] + " points");
                break;
            }
        }

    }

    void AddToObjectsDetected(string obj)
    {
        for(int i=0; i<=objectsDetected.Length; i++)
        {
            if(objectsDetected[0,i] == null)
            {
                objectsDetected[0,i] = obj;
                //Debug.Log(obj+" has been added to the detected objects list");
                break;
            }
        }
    
    }
}
