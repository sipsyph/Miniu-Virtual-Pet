using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageController : MonoBehaviour {

    public GameObject garbageControllerChildObj, currentGarbageControllerChildObj;

	// Use this for initialization
	void Start () {
        CreateGarbageControllerChildObj();
        InvokeRepeating("DeleteCurrentGarbageControllerChild", 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateGarbageControllerChildObj()
    {
        currentGarbageControllerChildObj = InstanceController.staticGarbageControllerObj = Instantiate(garbageControllerChildObj, this.transform);
    }

    void DeleteCurrentGarbageControllerChild()
    {
        if(currentGarbageControllerChildObj.transform.childCount >= 4)
        {
            Destroy(currentGarbageControllerChildObj);
            CreateGarbageControllerChildObj();
        }
        
    }
}
