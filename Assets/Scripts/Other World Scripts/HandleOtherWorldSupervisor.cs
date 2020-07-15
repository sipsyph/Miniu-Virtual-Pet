using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HandleOtherWorldSupervisor : MonoBehaviour {

    public Button cameraButton, returnToMainSceneButton;
    public Text debugText, energyText;
    public GameObject backCamera, sideCamera;

	// Use this for initialization
	void Start () {
        SetUpEvents();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetUpEvents()
    {
        cameraButton.onClick.AddListener(() =>
        {
            if(backCamera.activeInHierarchy)
            {
                sideCamera.SetActive(true);
                backCamera.SetActive(false);
            }

            else if(sideCamera.activeInHierarchy)
            {
                
                backCamera.SetActive(true);
                sideCamera.SetActive(false);
            }
        });

        returnToMainSceneButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Main Scene");
        });
    }
}
