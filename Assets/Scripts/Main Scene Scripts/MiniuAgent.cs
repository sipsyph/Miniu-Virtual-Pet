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

    public static int energyMeter, satiationMeter, entertainedMeter;
    private int energyMeterMax = 100, satiationMeterMax = 100, entertainedMeterMax = 100;

    private bool reachedTargetWithCarriedObj, reachedBedObjectTarget, reachedTargetDestination, 
    reachedFrontPointTarget, reachedTargetObject; //"when reached" bools
    private bool floatUp; //Pseudo animation bool to make carried object float up and down
    private NavMeshAgent agent; //controls the NavMesh of the agent
    public UnityEngine.UI.Text  energyText, ignoreText, entertainedMeterText;

    public static Transform targetObjectToCarry, objectBeingCarried, objCarrierOfAgent; //Placeholder Transform for what the agent wants to carry currently

    public static bool agentAttentionGotten, wantToCarryObject, agentIsNowCarryingSomething, 
        agentNoticedPlayerInteraction, agentIgnoringPlayer, agentTouchedByPlayerInteractionOngoing; //Reaction? toggles

    public static bool agentIsSleeping; //Action bools

    private bool lookAtPaintingToggle, continueDrawingToggle;// Paint action toggles

    Ray ray;
    RaycastHit hit;

    #endregion

    void Start()
    {
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
        RandomWalkingAround();
        //entertainedMeterText.text = "Boredom Meter: " + entertainedMeter;
    }

    void GoToSleep()
    {
        
    }

    void LookForThis()
    {
        //TODO: Display object icon in guniguni obj
        //TODO: Walk around in last known position of obj, and around the area
    }

    void RandomWalkingAround()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) { StartCoroutine("GoNextPoint"); }
        MSBodyAnimation.PlayWalkingAnimation();
    }

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
        StartCoroutine("GoNextPoint");
        agent.isStopped = false;
    }

    void GoToBed()
    {
        agent.isStopped = false;
        //agent.destination = bedObjects[0].position;
    }

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

    void ConditionMeters()
    {
        if(energyMeter > 0 && agentIsSleeping == false)
        {
            agentIsSleeping = false;
            energyMeter--;
        }

        if (!agentIsSleeping && !agentNoticedPlayerInteraction)
        {
            entertainedMeter++;
            if(entertainedMeter >= entertainedMeterMax)
            {
                entertainedMeter = entertainedMeterMax;
            }
        }

        if(agentIsSleeping == true)
        {
            //agentIsSleeping = true;
            energyMeter++;
        }

        if(energyMeter > energyMeterMax)
        {
            energyMeter = energyMeterMax;
            agentIsSleeping = false;
        }

        if(energyMeter <= 0)
        {
            //agentIsSleeping = true;
        }

        energyText.text = ""+energyMeter;
    }

}