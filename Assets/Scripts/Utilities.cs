using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {
    public static string inventoryStringPlaceholder="";
	// Use this for initialization
	void Start () {
		
	}

    public static void SetLastPlaneClickedPosition(float xPos, float yPos, float zPos)
    {
        PlayerPrefs.SetFloat("x", xPos);
        PlayerPrefs.SetFloat("y", yPos);
        PlayerPrefs.SetFloat("z", zPos);
    }

    public static void UpdateAgentStats()
    {
        //******Update Agent Age******//
        PlayerPrefs.SetInt("AgeSeconds", PlayerPrefs.GetInt("AgeSeconds") + 1);

        if(PlayerPrefs.GetInt("AgeSeconds") > 59)
        {
            PlayerPrefs.SetInt("AgeSeconds", 0);

            PlayerPrefs.SetInt("AgeMinutes", PlayerPrefs.GetInt("AgeMinutes") + 1);

            if (PlayerPrefs.GetInt("AgeMinutes") > 59)
            {
                PlayerPrefs.SetInt("AgeMinutes", 0);

                PlayerPrefs.SetInt("AgeHours", PlayerPrefs.GetInt("AgeHours") + 1);

                if (PlayerPrefs.GetInt("AgeHours") > 23)
                {
                    PlayerPrefs.SetInt("AgeHours", 0);

                    PlayerPrefs.SetInt("AgeDays", PlayerPrefs.GetInt("AgeDays") + 1);
                }
            }

        }
        //*****************************//
    }

    public static Vector3 GetLastPlaneClickedPosition()
    {
        float x, y, z;
        x = PlayerPrefs.GetFloat("x");
        y = PlayerPrefs.GetFloat("y");
        z = PlayerPrefs.GetFloat("z");
        Vector3 posVec = new Vector3(x, y, z);
        return posVec;
    }

    public static void IterateRepeatedInteractionCounter()
    {
        if (SupervisorAndUI.lastTouchedObj == SupervisorAndUI.currentTouchedObj)
        {
            SupervisorAndUI.supervisorActionRepeatedCounter++;
        }
    }

    public static int ToDeltaTime(int frames)
    {
        return Convert.ToInt32(frames * Time.deltaTime);
    }

    public static void AddObjectToInventory(string obj)
    {
        PlayerPrefs.SetString("Inventory", PlayerPrefs.GetString("Inventory")+obj+ "|");
        Debug.Log(PlayerPrefs.GetString("Inventory"));
    }

    public static void RemoveObjectFromInventory(string obj)
    {
        //PlayerPrefs.SetString("Inventory", PlayerPrefs.GetString("Inventory") + obj);
        inventoryStringPlaceholder = PlayerPrefs.GetString("Inventory");
        inventoryStringPlaceholder = inventoryStringPlaceholder.Replace(obj + "|", string.Empty);
        PlayerPrefs.SetString("Inventory", inventoryStringPlaceholder);
        Debug.Log(PlayerPrefs.GetString("Inventory"));
    }

    public static void DropThisObj(Transform transformObj, Transform playObjectsParent)
    {
        transformObj.SetParent(playObjectsParent);
        transformObj.GetComponent<Rigidbody>().isKinematic = false;
        transformObj.GetComponent<Rigidbody>().detectCollisions = true;
    }
}
