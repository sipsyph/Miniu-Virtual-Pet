using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSEyesAnimation : MonoBehaviour
{

    public static Animator eyesAnimator;
    public static string[] triggerNames;

    public Animator pubEyesAnimator;

    void Start()
    {
        eyesAnimator = pubEyesAnimator;
        triggerNames = new string[]{"normalEyesTrigger","tiredEyesTrigger","sleepingEyesTrigger","scaryEyesTrigger",
        "beingPetEyesTrigger","magicEyesTrigger"};
    }

    public static void PlayNormalEyesAnimation()
    {
        ResetTriggerExcept("normalEyesTrigger");
        eyesAnimator.SetTrigger("normalEyesTrigger");
    }

    public static void PlayTiredEyesAnimation()
    {
        ResetTriggerExcept("tiredEyesTrigger");
        eyesAnimator.SetTrigger("tiredEyesTrigger");
    }

    public static void PlaySleepingEyesAnimation()
    {
        ResetTriggerExcept("sleepingEyesTrigger");
        eyesAnimator.SetTrigger("sleepingEyesTrigger");
    }

    public static void ResetTriggerExcept(string triggerName)
    {

        for(int i=0; i<triggerNames.Length;i++)
        {
            if(!triggerName.Equals(triggerNames[i]))
            {
                eyesAnimator.ResetTrigger(triggerNames[i]);
            }
        }

    }
}
