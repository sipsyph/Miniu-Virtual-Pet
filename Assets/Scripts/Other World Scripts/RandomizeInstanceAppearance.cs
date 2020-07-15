using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeInstanceAppearance : MonoBehaviour {

//This script is attached to the highest parent in an Area Instance object


    public GameObject[] instanceElementGroup1, instanceElementGroup2, instanceElementGroup3, instanceElementGroup4; 
    // Group 1 is usually trees, Group 2 is rocks 'n stuff on the pathway, Group 3 is pathways, Group 4 is an extra element

    public GameObject backgroundPlane, thisInstanceToHit;
	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        thisInstanceToHit.SetActive(true);
        thisInstanceToHit.transform.parent.gameObject.SetActive(true);
        RandomizeThisGroup(instanceElementGroup1);
        RandomizeThisGroupScarcely(instanceElementGroup2);
        ChooseOneRandomPathway(instanceElementGroup3);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void RandomizeThisGroup(GameObject[] gameObjGroupArr)
    { 
        for(int i = 0; i< gameObjGroupArr.Length; i++)
        {
            int flip = Random.Range(0, 2);
            if(flip > 0)
            {
                gameObjGroupArr[i].SetActive(false);
            }
        }
    }

    void RandomizeThisGroupScarcely(GameObject[] gameObjGroupArr)
    {
        for (int i = 0; i < gameObjGroupArr.Length; i++)
        {
            int flip = Random.Range(0, 6);
            if (flip > 0)
            {
                gameObjGroupArr[i].SetActive(false);
            }
        }
    }

    void ChooseOneRandomPathway(GameObject[] pathwayGroupArr)
    {
        int choice = Random.Range(0, pathwayGroupArr.Length);

        pathwayGroupArr[choice].SetActive(true);
    }
}
