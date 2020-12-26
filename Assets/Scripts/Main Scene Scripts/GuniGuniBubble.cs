using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuniGuniBubble : MonoBehaviour {

    Quaternion rotation;
    public GameObject guniGuniBubble;

	// Use this for initialization
	void Start () {
        InvokeRepeating("DisableGuniGuniObject", 6, 6);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
    }

    void DisableGuniGuniObject()
    {
        guniGuniBubble.SetActive(false);
    }
}
