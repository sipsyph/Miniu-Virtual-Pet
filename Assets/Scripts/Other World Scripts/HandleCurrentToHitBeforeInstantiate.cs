using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCurrentToHitBeforeInstantiate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HandleOtherWorldAgent.currentTargetHittoHit = this.transform.gameObject;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            this.transform.gameObject.SetActive(false);
        }
    }
}
