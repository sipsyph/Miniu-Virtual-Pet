using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSBodyAnimation : MonoBehaviour
{
    public static Animator bodyAnimator;
    public static string[] triggerNames;

    public Animator pubBodyAnimator;

    void Start()
    {
        bodyAnimator = pubBodyAnimator;
        triggerNames = new string[]{"normalTrigger","walkingTrigger","carryingTrigger","sleepingTrigger",
        "sleeping2Trigger","sleeping3Trigger"};
    }

    public static void PlayNormalAnimation()
    {
        ResetTriggerExcept("normalTrigger");
        bodyAnimator.SetTrigger("normalTrigger");
    }

    public static void PlayWalkingAnimation()
    {
        ResetTriggerExcept("walkingTrigger");
        bodyAnimator.SetTrigger("walkingTrigger");
    }
    public static void PlayCarryingAnimation()
    {
        ResetTriggerExcept("carryingTrigger");
        bodyAnimator.SetTrigger("carryingTrigger");
    }

    public static void PlaySleepingAnimation()
    {
        ResetTriggerExcept("sleepingTrigger");
        bodyAnimator.SetTrigger("sleepingTrigger");
    }

    public static void PlaySleeping2Animation()
    {
        ResetTriggerExcept("sleeping2Trigger");
        bodyAnimator.SetTrigger("sleeping2Trigger");
    }

    public static void PlaySleeping3Animation()
    {
        ResetTriggerExcept("sleeping3Trigger");
        bodyAnimator.SetTrigger("sleeping3Trigger");
    }

    public static void ResetTriggerExcept(string triggerName)
    {

        for(int i=0; i<triggerNames.Length;i++)
        {
            if(!triggerName.Equals(triggerNames[i]))
            {
                bodyAnimator.ResetTrigger(triggerNames[i]);
            }
        }

    }

}
