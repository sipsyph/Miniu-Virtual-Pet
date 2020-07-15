using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {
    Quaternion rot;
	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        rot = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = rot;
    }
}
