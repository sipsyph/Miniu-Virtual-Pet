using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuniGuniBubble : MonoBehaviour {

    Quaternion rotation;
    public GameObject guniGuniBubble;
    public static GameObject guniGuniBubbleStatic;
    public TextMesh textInBubble1, textInBubble2;
    public static string textToBeDisplayed1, textToBeDisplayed2;
    public static bool shouldDisable;

	// Use this for initialization
	void Start () {
        guniGuniBubbleStatic = guniGuniBubble;
        shouldDisable = false;
	}
	
	// Update is called once per frame
	void Update () {
        textInBubble1.text = textToBeDisplayed1;
        textInBubble2.text = textToBeDisplayed2;
		if(guniGuniBubble.active && !shouldDisable)
        {
            shouldDisable = true;
            Invoke("DisableGuniGuniObject", 6);
        }
	}
    
    public static void ShowGuniGuni()
    {
        shouldDisable = false;
        guniGuniBubbleStatic.SetActive(true);
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
        shouldDisable = false;
        guniGuniBubble.SetActive(false);
    }
}
