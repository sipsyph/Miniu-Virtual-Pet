//Handles the detection of playObjects in the scene through the collider where this script is connected to

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAttentionField : MonoBehaviour {

    #region Variable Declarations
    public static bool ballDetected, ball2Detected, ball3Detected;
    public static int ballMemory, ball2Memory, ball3Memory;
    public static string[] rememberedObjects;
    public static string[,] objectsDetected;
    #endregion

    void Start () {
        ballDetected = false;
        ball2Detected = false;
        //rememberedObjects = new string[50];
        ballMemory = PlayerPrefs.GetInt("ballMemory");
        ball2Memory = PlayerPrefs.GetInt("ball2Memory");
        ball3Memory = PlayerPrefs.GetInt("ball3Memory");
        objectsDetected = new string[50,50];
    }

    void OnTriggerEnter(Collider colidedObj)
    {
        /*if (colidedObj.name == "Ball")
        {
            AddToObjectsDetected(colidedObj.name);
            ballMemory++;
            PlayerPrefs.SetInt("ballMemory", ballMemory);
            AddMemoryPointsToThisObject(colidedObj.name, ballMemory);
        }

        else if (colidedObj.name == "Ball2")
        {
            AddToObjectsDetected(colidedObj.name);
            ball2Memory++;
            PlayerPrefs.SetInt("ball2Memory", ball2Memory);
            AddMemoryPointsToThisObject(colidedObj.name, ball2Memory);
        }

        else if (colidedObj.name == "Ball3")
        {
            AddToObjectsDetected(colidedObj.name);
            ball3Memory++;
            PlayerPrefs.SetInt("ball3Memory", ball3Memory);
            AddMemoryPointsToThisObject(colidedObj.name, ball3Memory);
        }*/
    }

    
    //CUSTOM METHODS
    void SaveDetectedNewObject(string detectedObj)
    {
        
        for (int i = 0; i <= rememberedObjects.Length; i++)
        {
            if (rememberedObjects[i] == "")
            {
                rememberedObjects[i] = detectedObj;
            }else if(rememberedObjects[i] == detectedObj)
            {
                Debug.Log(detectedObj+" already exists in memory");
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
