using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoulFoodGenerator : MonoBehaviour
{
    
    private AudioSource audioSource;

    public AudioClip inputMusic;
    public Button makeIntoFoodBtn;

    public GameObject generatedFoodCarrier;
    public static Transform generatedSoulFood;
    private System.Random rnd;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rnd = new System.Random();
        SetUpEvents();
    }

    void ConvertIntoFood(AudioSource inputMusic)
    {
        
    }

    void SetUpEvents()
    {
        makeIntoFoodBtn.onClick.AddListener(() =>
        {
            HandleBall handleBall = new HandleBall();

            int randomNumber = rnd.Next(1,4);
            Debug.Log("RANDOMLY GENERATED NUMBER 1-3 ==> "+randomNumber);
            if(randomNumber == 1)
            {
                //generatedSoulFood.GetComponent<Renderer>().material.color = Color.red;
            }
            else if(randomNumber == 2)
            {
                //generatedSoulFood.GetComponent<Renderer>().material.color = Color.red;
            }
            else if(randomNumber == 3)
            {
                //generatedSoulFood.GetComponent<Renderer>().material.color = Color.red;
            }

            
        });
    }
}
