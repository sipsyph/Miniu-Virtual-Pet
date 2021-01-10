using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuniGuniBubble : MonoBehaviour {

    Quaternion rotation;
    public GameObject guniGuniBubble;
    public TextMesh textInBubble1, textInBubble2;
    public static string textToBeDisplayed1, textToBeDisplayed2;
    private bool shouldDisable;

	// Use this for initialization
	void Start () {
        shouldDisable = false;
        textToBeDisplayed1 = "";
        textToBeDisplayed2 = "";
        //InvokeRepeating("DisableGuniGuniObject", 6, 6);
	}
	
	// Update is called once per frame
	void Update () {
		if(guniGuniBubble.active && !shouldDisable)
        {
            shouldDisable = true;
            Invoke("DisableGuniGuniObject", 6);
        }
	}

    void Awake()
    {
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        textInBubble1.text = textToBeDisplayed1;
        textInBubble2.text = textToBeDisplayed2;
        transform.rotation = rotation;
    }

    void DisableGuniGuniObject()
    {
        shouldDisable = false;
        guniGuniBubble.SetActive(false);
    }
}
