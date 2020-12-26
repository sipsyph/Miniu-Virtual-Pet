using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniuBrain : MonoBehaviour
{

    List<string[]> rememberedPlayObjs = new List<string[]>(); 

    void Start()
    {
        //PlayerPrefs.DeleteAll(); //Dangerous code, only for debugging purposes
        InvokeRepeating("DebugRememberedObjs", 2.0f, 3.0f);
        //InvokeRepeating("RetrieveAllRememberedPlayObjsAndDefinition", 1.0f, 2.0f);
    }

    void Update()
    {
        
    }

    private bool PlayObjRemembered(string playObjName)
    {
        RetrieveAllRememberedPlayObjsAndDefinition();

        bool remembered = false;
        foreach(var playObj in rememberedPlayObjs)
        {
            if(playObj[0].Equals(playObjName))
            {
                Debug.Log("I have already seen this object: "+playObjName);
                remembered = true;
                break;
            }
        }

        return remembered;
    }

    public void AddToMiniuMemoryThisPlayObjWithName(string playObjectName)
    {
        if(!PlayObjRemembered(playObjectName))
        {
            Debug.Log("This object is new to me: "+playObjectName);
            PlayerPrefs.SetString("rememberedObjects", PlayerPrefs.GetString("rememberedObjects","") 
            + "{[" + playObjectName + "][" + "0.0" + "]}");
            //Debug.Log("Remembered Obs after adding new Obj"+PlayerPrefs.GetString("rememberedObjects", ""));
        }
        
    }

    private string[] RetrieveAllRememberedPlayObjsAndDefinition()
    {
        string serializedPlayObjs = PlayerPrefs.GetString("rememberedObjects", "");
        string[] playObjsArr = serializedPlayObjs.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var str in playObjsArr)
        {
            Debug.Log("Play Object: "+str);
            string[] playObjsDefinitionArr = str.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log("Play Object Def 1: "+playObjsDefinitionArr[0]);
            //Debug.Log("Play Object Def 2: "+playObjsDefinitionArr[1]);
            rememberedPlayObjs.Add(playObjsDefinitionArr);
            
        }
        return playObjsArr;
    }

    private void DebugRememberedObjs()
    {
        Debug.Log("Retrieved remembered objs: "+PlayerPrefs.GetString("rememberedObjects", ""));
    }



    public float RetrieveFondnessValueOfPlayObjWithName(string playObjectName)
    {
        return 0.0f;
    }

    public void IncrementFondnessValueOfPlayObjWithName(string playObjectName, float fondnessVal)
    {
        return;
    }

    // public float retrieveFondnessValueOfWord(string word)
    // {
    //     return PlayerPrefs.GetFloat(word);
    // }

    // public void incrementFondnessValueOfWord(string word, float fondnessVal)
    // {
    //     PlayerPrefs.SetFloat(word, PlayerPrefs.GetFloat(word) + fondnessVal);
    //     return;
    // }
}
