using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class MiniuBrain : MonoBehaviour
{
    ItemController itemController = new ItemController();

    List<string[]> rememberedPlayObjs = new List<string[]>(); 

    void Start()
    {
        //PlayerPrefs.DeleteAll(); //Dangerous code, only for debugging purposes
        //InvokeRepeating("RetrieveAllRememberedPlayObjsAndDefinition", 1.0f, 2.0f);
    }

    void Update()
    {
        
    }

    public void UpdateAgentConditionMeters(int energyMeter, int satiationMeter, int funMeter)
    {

    }

    private string RetrieveLastKnownPositionOf(string playObjName)
    {
        return "";
    }

    public string retrieveWantedObjWithTheseConditionMeters(int energyMeter, int satiationMeter, int funMeter)
    {
        string[][] rememberedObjs = RetrieveAllRememberedPlayObjsAndTheirFondVal();
        string currentPlayObjWithHighestFondVal = "";
        float currentFondVal = 0.0f;

        for (var i = 0; i < rememberedObjs.Length; i++) 
        {
            Debug.Log("Remembered objs: "+rememberedObjs[i][0]);
            if(energyMeter <= 300)
            {
                if(itemController.DetermineWhatCategoryThisPlayObjIs(rememberedObjs[i][0]).Equals("Comfort"))
                {
                    //Get comfort object with highest fondness value
                    float floatValOfFond = float.Parse(rememberedObjs[i][1], CultureInfo.InvariantCulture.NumberFormat);
                    if(floatValOfFond >= currentFondVal)
                    {
                        currentPlayObjWithHighestFondVal = rememberedObjs[i][0];
                        currentFondVal = floatValOfFond;
                    }
                }
            }
        }

        Debug.Log("HIGHEST FOND VAL OBJ: "+currentPlayObjWithHighestFondVal+" at "+currentFondVal);
        return currentPlayObjWithHighestFondVal;
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

    public void AddToMiniuMemoryThisPlayObjWithName(Transform playObject)
    {
        if(!PlayObjRemembered(playObject.name))
        {
            Debug.Log("This object is new to me: "+playObject.name);
            PlayerPrefs.SetString("rememberedObjects", PlayerPrefs.GetString("rememberedObjects","") 
            + "{[" + playObject.name + "][" + "0.0" + "][" + "0.00" + "][" + "0.00" + "][" + "0.00" + "]}");
            ItemController itemController = new ItemController();

            itemController.AddThisToRememberedObjects(playObject);
            //Debug.Log("Remembered Obs after adding new Obj"+PlayerPrefs.GetString("rememberedObjects", ""));
        }
        
    }

    private string[] RetrieveAllRememberedPlayObjsAndDefinition()
    {
        string serializedPlayObjs = PlayerPrefs.GetString("rememberedObjects", "");
        string[] playObjsArr = serializedPlayObjs.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
        rememberedPlayObjs.Clear();
        foreach (var str in playObjsArr)
        {
            
            string[] playObjsDefinitionArr = str.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            rememberedPlayObjs.Add(playObjsDefinitionArr);
            //Debug.Log("Play Object: "+str+" Name: "+playObjsDefinitionArr[0]+" Def: "+playObjsDefinitionArr[1]+" Def: "+playObjsDefinitionArr[2]+" Def: "+playObjsDefinitionArr[3]+" Def: "+playObjsDefinitionArr[4]);
        }
        return playObjsArr;
    }

    private string[][] RetrieveAllRememberedPlayObjsAndTheirFondVal()
    {
        string serializedPlayObjs = PlayerPrefs.GetString("rememberedObjects", "");
        string[] playObjsArr = serializedPlayObjs.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
        string[][] rememberedPlayObjsNamesAndTheirFondVal = new string[ItemController.playObjects.Length-1][];
        int i = 0;
        foreach (var str in playObjsArr)
        {
            rememberedPlayObjsNamesAndTheirFondVal[i] = new string[2];
            string[] playObjsDefinitionArr = str.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            rememberedPlayObjsNamesAndTheirFondVal[i][0] = playObjsDefinitionArr[0];
            rememberedPlayObjsNamesAndTheirFondVal[i][1] = playObjsDefinitionArr[1];
            Debug.Log("At RetrieveAllRememberedPlayObjsAndTheirFondVal: "+rememberedPlayObjsNamesAndTheirFondVal[i][0]);
            Debug.Log("At RetrieveAllRememberedPlayObjsAndTheirFondVal: "+rememberedPlayObjsNamesAndTheirFondVal[i][1]);
            i++;
        }
        return rememberedPlayObjsNamesAndTheirFondVal;
    }


    public void UpdateDefinitionOfPlayObjWithName(string playObjectName, float fondnessVal, Vector3 latestPos)
    {
        RetrieveAllRememberedPlayObjsAndDefinition();
        //PlayerPrefs.SetString("rememberedObjects", ""); //Clear data
        string origPlayObjString = PlayerPrefs.GetString("rememberedObjects", "");
        foreach(var playObj in rememberedPlayObjs)
        {
            if(playObj[0].Equals(playObjectName))
            {
                //Old Value  =  New Value
                //playObj[1] = (float.Parse(playObj[1]) + fondnessVal).ToString();
                //playObj[2] = String.Format("{0:0.##}", latestPos.x);
                //playObj[3] = String.Format("{0:0.##}", latestPos.y);
                //playObj[4] = String.Format("{0:0.##}", latestPos.z);

                Debug.Log("Old Value: "+origPlayObjString);
                
                origPlayObjString = origPlayObjString.Replace(
                    BuildPlayObjStringFrom(
                        playObj[0]
                        , playObj[1]
                        , playObj[2]
                        , playObj[3]
                        , playObj[4]
                    )
                    ,BuildPlayObjStringFrom(
                        playObj[0]
                        , (float.Parse(playObj[1]) + fondnessVal).ToString()
                        , String.Format("{0:0.##}", latestPos.x)
                        , String.Format("{0:0.##}", latestPos.y)
                        , String.Format("{0:0.##}", latestPos.z)
                    )
                );

                Debug.Log("New Value: "+origPlayObjString);
                PlayerPrefs.SetString("rememberedObjects", origPlayObjString);
                break;
            }
        }

        //Debug.Log("Retrieved remembered objs: "+PlayerPrefs.GetString("rememberedObjects", ""));
        //RetrieveAllRememberedPlayObjsAndDefinition(); //For debugging purposes
        return;
    }

    public string BuildPlayObjStringFrom(string name, string fondnessVal, string posX, string posY, string posZ)
    {
        return "{[" 
        + name 
        + "][" + (fondnessVal).ToString() 
        + "][" + String.Format("{0:0.##}", posX) 
        + "][" + String.Format("{0:0.##}", posY) 
        + "][" + String.Format("{0:0.##}", posZ) + "]}";
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
