//Main script for the agent
//
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;


public class MiniuAgent : MonoBehaviour
{
    #region Variable Declarations
    public Transform[] points, playObjects, frontPoints; //Transform Arrays
    public static Transform[] staticPlayObjects, staticPoints;

    public Transform currentObjectOfAttention, lastObjectOfAttention, middlePoint, playObjectsParent
    , targetPointToCarryObjectTo, objectToCarry, targetDestination, 
    frontPointTargetObject, door, targetObjectToGoTo,
    playObjectHandler, currentDestination, guniGuniObj; //Placeholder Transforms and other points

    
    private int destPoint ,attentionCounter ,currentObjAttentionCounter ,touchCounter
        ,howLongCarryCounter ,idleCounter ,noticedInteractionCounter , baseAgentReactionDuration; //Counters

    private int chosen=0, chosenFlip=0; //Roulette variables

    public int noticedInteractionCounterMax = 240, actionCounterMax = 480; //Counter max values
    public static int ignorePlayerCounter = 0;

    public static int energyMeter, satiationMeter, funMeter;
    private int energyMeterMax = 1000, satiationMeterMax = 1000, funMeterMax = 1000, condittionMeterMax = 1000;

    private bool reachedTargetWithCarriedObj, reachedBedObjectTarget, reachedTargetDestination, 
    reachedFrontPointTarget, reachedTargetObject; //"when reached" bools
    private bool floatUp; //Pseudo animation bool to make carried object float up and down
    private NavMeshAgent agent; //controls the NavMesh of the agent
    public UnityEngine.UI.Text  energyText, ignoreText, funMeterText, debugText;

    public static Transform targetObjectToCarry, objectBeingCarried, objCarrierOfAgent; //Placeholder Transform for what the agent wants to carry currently

    public static Transform obJBeingUsedByAgent;

    public static bool agentAttentionGotten, wantToCarryObject, agentIsNowCarryingSomething, 
        agentNoticedPlayerInteraction, agentIgnoringPlayer, agentTouchedByPlayerInteractionOngoing; //Reaction? toggles

    public static bool agentIsSleeping, agentIsTired; //Action bools

    private bool hasChosenSleepPos = false, hasFoundCurrentLookingForObj = false;

    private bool lookAtPaintingToggle, continueDrawingToggle;// Paint action toggles

    public static Transform currentWantObj;
    Ray ray;
    RaycastHit hit;

    #endregion

    void Start()
    {
        agentIsSleeping = false; agentIsTired = false;
        agentNoticedPlayerInteraction = false;
        agent = GetComponent<NavMeshAgent>();

        staticPlayObjects = playObjects;
        staticPoints = points;
        objCarrierOfAgent = playObjectHandler;
        energyMeter = energyMeterMax;

        targetPointToCarryObjectTo = points[30];
        objectToCarry = playObjects[Random.Range(0, playObjects.Length)];
        targetDestination = frontPoints[Random.Range(0, frontPoints.Length)];
        frontPointTargetObject = frontPoints[Random.Range(0, frontPoints.Length)];
    }

    void Update()
    {
        ConditionMeters();
        ReactionToConditionMeters();
        
        //GuniGuniBubble.textToBeDisplayed = "hello";
        debugText.text = "Miniu Using Obj: " + ItemController.currentHeldObj.name;
    }

/* #region  Condition Meters stuff */


    void ConditionMeters()
    {
        HandleEnergyConditionMeter();

        //Agent is awake and hasn't been interacting with player for a while
        if (!agentIsSleeping && !agentNoticedPlayerInteraction)
        {
            funMeter++;
            if(funMeter >= funMeterMax)
            {
                funMeter = funMeterMax;
            }
        }
        energyText.text = "Energy: "+energyMeter;
    }

    void HandleEnergyConditionMeter()
    {
        //Agent is awake, so energy should go down overtime
        if(energyMeter > 0 && !agentIsSleeping)
        {
            if(energyMeter == energyMeterMax * .8)
            {
                MSEyesAnimation.PlayNormalEyesAnimation();
            }
            agentIsTired = false;
            energyMeter--;
        }
        
        //Energy should go up if agent is sleeping
        if(agentIsSleeping == true)
        {
            energyMeter++;
        }

        //Agent should wake up if energy has reached max
        if(energyMeter > energyMeterMax)
        {
            MSEyesAnimation.PlayTiredEyesAnimation();
            energyMeter = energyMeterMax;
            agentIsSleeping = false;
        }

        //Agent should now sleep if energy has reached min
        if(energyMeter <= 0)
        {
            agentIsSleeping = true;
        }
        //Tired eyes at 30% of energy
        if(energyMeter == energyMeterMax * .3)
        {
            if(!agentIsSleeping)
            {
                agentIsTired = true;
                MSEyesAnimation.PlayTiredEyesAnimation();
                
            }
        }
        //Start chosen sleep animation at 2% of energy
        if(energyMeter == energyMeterMax * .02 )
        {
            int chosenSleepingAnim = Random.Range(0,100);
            MSEyesAnimation.PlaySleepingEyesAnimation();
            if(chosenSleepingAnim >= 0 && chosenSleepingAnim <= 33)
            {
                MSBodyAnimation.PlaySleepingAnimation();
            }
            else if(chosenSleepingAnim > 33 && chosenSleepingAnim <= 66)
            {
                MSBodyAnimation.PlaySleeping2Animation();
            }
            else
            {
                MSBodyAnimation.PlaySleeping3Animation();
            }
        }
    }

    void ReactionToConditionMeters()
    {
        if(agentIsTired)
        {
            //LookForThis(objName);
        }

        if(agentIsSleeping)
        {
            GoToSleep();
        }else{
            BeAwake();
            RandomWalkingAround();
        }


    }
    
/* #endregion */
    
    void GoToSleep()
    {
        //LookForThis("Sleep Obj");
        StopAgentFromMoving();
    }

    void BeAwake()
    {
        StartAgentMoving();
        
    }

    void LookForThis(string objName)
    {
        if(!hasFoundCurrentLookingForObj)
        {
            //TODO: Display object icon in guniguni obj
            GuniGuniBubble.textToBeDisplayed1 = "look for: "+objName;
            
            //TODO: For a certain amt of time walk around in last known position of obj, 
            //      and around the area, and perform the most effective request action
            //      until the object is seen
            //   
            //if()
        }
   

    }

    void MoveTowards()
    {

    }

    void PerformThisRequestAction(string requestActionName)
    {
        //Request Actions: Whine, Puppy eyes, Jump
        //TODO: Make enum for Request Actions
    }

    void UseThis(string objName)
    {
        GuniGuniBubble.textToBeDisplayed2 = "using: "+objName;
        //TODO: Use animations for each play obj
    }



    void RandomWalkingAround()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) { StartCoroutine("GoNextPoint"); }
        MSBodyAnimation.PlayWalkingAnimation();
    }



    int HowLongAgentShouldReact()
    {
        return baseAgentReactionDuration * 120;
    }
    

    void RotateTowards(Transform obj)
    {
        agent.isStopped = true;
        int rotateSpeed = 5;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
                ((obj.position - transform.position).normalized), Time.deltaTime * rotateSpeed);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    

    void CarryThisObject(Transform obj)
    {
        
    }

    void CarryObjectTo(Transform destinationPoint)
    {
        
    }

    void StopAgentFromMoving()
    {
        StopCoroutine("GoNextPoint");
        agent.isStopped = true;
    }

    void StartAgentMoving()
    {
        //StartCoroutine("GoNextPoint");
        agent.isStopped = false;
    }

    void GoToBed()
    {
        //agent.isStopped = false;
        //agent.destination = bedObjects[0].position;
    }
/* #region  RNG stuff */

    IEnumerator ContinuousRandomNumberGeneration()
    {
        currentDestination = frontPoints[Random.Range(0, frontPoints.Length)];

        int val = Random.Range(0, 10);

        if (val <= 4)
        {
            chosenFlip = 0;
        }
        else if (val > 4)
        {
            chosenFlip = 1;
        }

        baseAgentReactionDuration = Random.Range(5,10);
        yield return null;
    }

    IEnumerator GoNextPoint()
    {
        destPoint = Random.Range(0, points.Length);
        agent.destination = points[destPoint].position;
        yield return null;
    }


    void FlipCoin()
    {
        int val = Random.Range(0,10);
        if (val <= 4)
        {
            chosenFlip = 0;
        }
        else if (val > 4)
        {
            chosenFlip = 1;
        }
        else
        {
            chosenFlip = 1;
        }
    }

/* #endregion */
    

    private void OnMouseDown()
    {
        Debug.Log("Agent clicked");
        guniGuniObj.gameObject.SetActive(true);
        agentTouchedByPlayerInteractionOngoing = true;
        SupervisorAndUI.lastTouchedObj = SupervisorAndUI.currentTouchedObj;
        SupervisorAndUI.currentTouchedObj = this.transform;
        SupervisorAndUI.playerInteractionOngoing = true;

        Utilities.IterateRepeatedInteractionCounter();
    }

    private void OnTriggerEnter(Collider collision)
    {

        if(collision.transform.name == frontPointTargetObject.name)
        { reachedFrontPointTarget = true; }
        else
        { reachedFrontPointTarget = false; }


        if (collision.transform.name == targetPointToCarryObjectTo.name)
        { reachedTargetWithCarriedObj = true; }
        else if(collision.transform.name != targetPointToCarryObjectTo.name)
        { reachedTargetWithCarriedObj = false; }


        if(collision.transform.name == targetDestination.name)
        { reachedTargetDestination = true; }
        else if(collision.transform.name != targetDestination.name)
        { reachedTargetDestination = false; }


        // if(collision.transform.name == targetObjectToGoTo.name)
        // { reachedTargetObject = true; }
        // else if(collision.transform.name != targetObjectToGoTo.name)
        // { reachedTargetObject = false; }


        // if (collision.transform.name == bedObjects[0].name)
        // { reachedBedObjectTarget = true; }
        // else 
        // { reachedBedObjectTarget = false; }
    }

    

}