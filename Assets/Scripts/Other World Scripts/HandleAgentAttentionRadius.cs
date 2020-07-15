using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAgentAttentionRadius : MonoBehaviour {

    public GameObject areaInstancePrefab;
    public GameObject currentHitToHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "To Hit Before Instantiate")
        {
            Debug.Log("To Hit B4 Instantiate is HIT!");
            //currentHitToHit = other.transform.gameObject;
            
            Instantiate(areaInstancePrefab, new Vector3(other.transform.parent.position.x, 4.341797f, other.transform.parent.position.z -94f), other.transform.parent.rotation);
            
        }
    }
}
