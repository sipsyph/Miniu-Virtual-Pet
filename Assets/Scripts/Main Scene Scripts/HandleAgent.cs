//Main script for the agent
//
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;


public class HandleAgent : MonoBehaviour
{
    #region Variable Declarations
    public bool isInMoodToPlayOnItsOwn, isTired, isLookingForAttention, isInMoodToChill, isInMoodToPaint;
    public Transform[] points, playObjects, sleepComfortObjects, bedObjects, pointsToCarryObjectTo, frontPoints; //Transform Arrays
    public static Transform[] staticPlayObjects, staticPoints;

public Transform currentObjectOfAttention, lastObjectOfAttention, objectBeingIgnored, middlePoint, playObjectsParent
    , targetPointToCarryObjectTo, objectToCarry, objectToPushAndBack, targetDestination, 
    frontPointTargetObject, door, targetObjectToGoTo,
    playObjectHandler, currentDestination, guniGuniObj; //Placeholder Transforms and other points

    
    private int destPoint ,roamCounter ,attentionCounter ,currentObjAttentionCounter ,touchCounter ,
        actionCounter,backCounter,pushCounter,playDecisionCounter,howLongCarryCounter
        ,idleCounter ,noticedInteractionCounter ,waitToFinishRotationCtr ,floatCtr, baseAgentReactionDuration; //Counters

    private int chosen=0, chosenFlip=0; //Roulette variables

    public int energyMeterMax = 10000, noticedInteractionCounterMax = 240, actionCounterMax = 480, boredomMeterMax = 10000; //Counter max values
    public static int energyMeter, ignorePlayerCounter = 0, boredomMeter = 500;

    private bool reachedTargetWithCarriedObj, reachedBedObjectTarget, reachedTargetDestination, 
    reachedFrontPointTarget, reachedTargetObject; //"when reached" bools
    private bool floatUp; //Pseudo animation bool to make carried object float up and down
    private NavMeshAgent agent; //controls the NavMesh of the agent
    public UnityEngine.UI.Text  boredomText, energyText, ignoreText, boredomMeterText;

    public static Transform targetObjectToCarry, objectBeingCarried, objCarrierOfAgent; //Placeholder Transform for what the agent wants to carry currently

    public static bool agentAttentionGotten, wantToCarryObject, agentIsNowCarryingSomething, 
        agentNoticedPlayerInteraction, agentIgnoringPlayer, agentTouchedByPlayerInteractionOngoing; //Reaction? toggles

    public bool keepThisFalse, pushBool, backBool, agentIsSleeping; //Action bools

    private bool backAndPushToggle, carryThenThrowToggle, roamToggle, roamCarryToggle; // Play action toggles

    private bool tiredRoamToggle, tiredRoamCarryToggle, stayInPlaceToggle;// Tired action toggles

    private bool lookAtPaintingToggle, continueDrawingToggle;// Paint action toggles

    Ray ray;
    RaycastHit hit;

    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isInMoodToPlayOnItsOwn = true; //default for debugging purposes
        destPoint = 0; roamCounter = 0; attentionCounter =  0; currentObjAttentionCounter = 0; 
        touchCounter = 0; actionCounter = 475; backCounter = 0; pushCounter = 0; playDecisionCounter = 478; 
        howLongCarryCounter = 0; idleCounter = 0; noticedInteractionCounter = 0; waitToFinishRotationCtr = 0; 
        floatCtr = 0; baseAgentReactionDuration = 0;

        staticPlayObjects = playObjects;
        staticPoints = points;

        Debug.Log(staticPlayObjects); 
        pushBool = carryThenThrowToggle = true;
        backBool = wantToCarryObject = agentIsNowCarryingSomething = reachedTargetWithCarriedObj = floatUp = agentAttentionGotten =
        reachedTargetDestination = backAndPushToggle =  roamToggle = roamCarryToggle = keepThisFalse =
        reachedBedObjectTarget =reachedFrontPointTarget = agentNoticedPlayerInteraction =  agentIgnoringPlayer = agentTouchedByPlayerInteractionOngoing = false;

        objCarrierOfAgent = playObjectHandler;
        energyMeter = energyMeterMax;

        if (!agent.pathPending && agent.remainingDistance < 0.5f) { StartCoroutine("GoNextPoint"); }
        targetPointToCarryObjectTo = points[30];
        objectToCarry = playObjects[Random.Range(0, playObjects.Length)];
        objectToPushAndBack = playObjects[Random.Range(0, playObjects.Length)];
        targetDestination = frontPoints[Random.Range(0, frontPoints.Length)];
        frontPointTargetObject = frontPoints[Random.Range(0, frontPoints.Length)];
        //InvokeRepeating("PickRandomNumberOnce", 1, 5);
        StartCoroutine("ActionDecisionForPlay");
        StartCoroutine("ContinuousRandomNumberGeneration");
    }

    void Update()
    {
        ConditionMeters();
        if (agentIgnoringPlayer)
        {
            ignoreText.text = "Agent is ignoring";
            agentNoticedPlayerInteraction = false;
            StopIgnoringPlayerAfterThisTime(480);
        }

        NoticePlayerInteraction();

        if (!agentIsSleeping && !agentNoticedPlayerInteraction)
        {
            
            agent.isStopped = false;

            if(isInMoodToPlayOnItsOwn)
            {
                isTired = false;
                isLookingForAttention = false;
                isInMoodToChill = false;
                StopBeingTired();
                StopPainting();
                Play(); //Controlled by the Action Decision Coroutine through the carryToggle, pushBackToggle, roamToggle, roamCarryToggle
            }
            else if(isInMoodToPaint)
            {
                isTired = false;
                isInMoodToChill = false;
                isInMoodToPlayOnItsOwn = false;
                isLookingForAttention = false;
                StopPlaying();
                StopBeingTired();
                Paint();
            }
            else if(isTired)
            {
                isInMoodToPlayOnItsOwn = false;
                isLookingForAttention = false;
                isInMoodToChill = false;
                StopPlaying();
                Tired();
            }
            else if(isLookingForAttention) //WIP
            {   
                isTired = false;
                isInMoodToPlayOnItsOwn = false;
                isInMoodToChill = false;
            }
            else if(isInMoodToChill) //WIP
            {
                isTired = false;
                isLookingForAttention = false;
                isInMoodToPlayOnItsOwn = false;
            }
        }
        
        else if(agentIsSleeping)
        {
            boredomMeter = boredomMeterMax;
            agentNoticedPlayerInteraction = false;
            StopPlaying();
            StopBeingTired();
            StopCoroutine("ContinuousRandomNumberGeneration");
            StopCoroutine("GoNextPoint");
            if (reachedBedObjectTarget == false)
            {
                GoToBed();
            }
            else if(reachedBedObjectTarget == true)
            {
                agent.isStopped = true;
                MSBodyAnimation.PlaySleepingAnimation();
            }
        }
        //PracticeRememberedActions(); //Reinforced actions by the player

        boredomMeterText.text = "Boredom Meter: " + boredomMeter;

    }

    #region Major Agent Reaction Methods


    private void OnMouseDown()
    {
        Debug.Log("Agent clicked");
        guniGuniObj.gameObject.SetActive(true);
        agentTouchedByPlayerInteractionOngoing = true;
        HandleSupervisor.lastTouchedObj = HandleSupervisor.currentTouchedObj;
        HandleSupervisor.currentTouchedObj = this.transform;
        HandleSupervisor.playerInteractionOngoing = true;

        Utilities.IterateRepeatedInteractionCounter();
    }

    void NoticePlayerInteraction()
    {
        if(!agentIgnoringPlayer && !agentIsSleeping)
        {
            Debug.Log("Player last touched obj is "+HandleSupervisor.lastTouchedObj);
            Debug.Log("Player current touched obj is "+HandleSupervisor.currentTouchedObj);
            if(HandleSupervisor.playerInteractionOngoing)
            {
                agentNoticedPlayerInteraction = true;
            }
        }

        if (agentNoticedPlayerInteraction && !agentIsSleeping)
        {
            noticedInteractionCounter++;
            boredomMeter--;
            if(boredomMeter<=0)
            {
                boredomMeter = 0;
            }
            StopCoroutine("ContinuousRandomNumberGeneration");
            StopPlaying();

            if(HandleSupervisor.currentTouchedObj == this.transform)
            {
                //GoToPlayer();
                RotateTowards(door);
                Debug.Log("Agent reacting to click on itself");
            }
            
            else if (HandleSupervisor.currentTouchedObj != null && HandleSupervisor.currentTouchedObj.tag == "Play Object")
            {
                GoToDestination(HandleSupervisor.currentTouchedObj.transform); //insert decision making process here
                Debug.Log("Agent reacting to play object "+ HandleSupervisor.currentTouchedObj.name);
            }

            else if(HandleSupervisor.currentTouchedObj.name == "Actable Plane Group")
            {
                GoToDestination(Utilities.GetLastPlaneClickedPosition());
                Debug.Log("Agent reacting to click on plane");
            }
            
            else if(HandleSupervisor.currentTouchedObj.name == "Door")
            {
                GoToPlayer();
                Debug.Log("Agent reacting to Door");
            }

            else
            {
                GoToDestination(HandleSupervisor.currentTouchedObj);
                Debug.Log("Else statement");
            }
            
            Debug.Log(HowLongAgentShouldReact());
            boredomText.text = "Agent Noticed";
        }

        
        if(noticedInteractionCounter > HowLongAgentShouldReact())
        {
            StartCoroutine("ContinuousRandomNumberGeneration");
            Debug.Log("Agent is finished noticing interaction");

            HandleSupervisor.playerInteractionOngoing = true;

            noticedInteractionCounter = 0;
            agentNoticedPlayerInteraction = false;
            HandleSupervisor.playerInteractionOngoing = false;
        }
    }

    int HowLongAgentShouldReact()
    {
        return baseAgentReactionDuration * 120;
    }

    void StopIgnoringPlayerAfterThisTime(int forHowManyFrames)
    {
        ignorePlayerCounter++;
        if (ignorePlayerCounter > forHowManyFrames)
        {
            ignorePlayerCounter = 0;
            agentNoticedPlayerInteraction = false;
            agentIgnoringPlayer = false;
            ignoreText.text = "Agent is not ignoring";
        }
    }
    #endregion
    
    #region Agent Action Methods

    void StopPlaying()
    {
        DropCarriedObjectAfterThisTime(5);
        StopCoroutine("ContinuousRandomNumberGeneration");
        StopCoroutine("ActionDecisionForPlay");
        backAndPushToggle = false; carryThenThrowToggle = false; roamToggle = false; roamCarryToggle = false;

    }

    void StopBeingTired()
    {
        StopCoroutine("ContinuousRandomNumberGeneration");
        StopCoroutine("ActionDecisionForTired");
        tiredRoamToggle=false; stayInPlaceToggle=false;
        agent.isStopped = false;
        agent.speed = 2f;
    }

    void StopPainting()
    {
        StopCoroutine("ContinuousRandomNumberGeneration");
        StopCoroutine("ActionDecisionForPaint");
        lookAtPaintingToggle=false; continueDrawingToggle=false;
        agent.isStopped = false;
    }
    void HandleGoNextPoint()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) 
        { 
            StartCoroutine("GoNextPoint"); 
        }
    }

    void Paint()
    {
        targetObjectToGoTo = ItemController.paintingAreaObj;
        if (reachedTargetObject) //Go to the painting area first, before actually painting
        {
            RotateTowards(ItemController.paintCanvasObj);
            StartCoroutine("ActionDecisionForPaint");
            if(lookAtPaintingToggle)
            {
                Debug.Log("Agent is looking at painting");
                StartCoroutine("ActionDecisionForPaint");
                //MSBodyAnimation.PlayLookingAnimation(); //maybe multiple random looking animations?
            }

            if(continueDrawingToggle)
            {
                Debug.Log("Agent is continuing to draw");
                StartCoroutine("ActionDecisionForPaint");
                //MSBodyAnimation.PlayPaintingAnimation(); 
            }               
        }
        else
        {
            Debug.Log("Agent is going to Paint Canvas location");
            GoToDestination(targetObjectToGoTo);
        }
    }

    void Tired()
    {
        StartCoroutine("ActionDecisionForTired");
        if(tiredRoamToggle)
        {
            agent.isStopped = false;
            Debug.Log("Agent is tired roaming");
            HandleGoNextPoint();
            StartCoroutine("ActionDecisionForTired");
            MSBodyAnimation.PlayNormalAnimation();
        }

        if(stayInPlaceToggle)
        {
            agent.isStopped = true;
            StartCoroutine("ActionDecisionForTired");   
            MSBodyAnimation.PlayWalkingAnimation();
        }
    }

    void Play()
    {
        StartCoroutine("ActionDecisionForPlay");
        if (carryThenThrowToggle)
        {
            Debug.Log("Agent is carrying then throwing");
            CarryThisObject(objectToCarry);
            CarryObjectTo(targetPointToCarryObjectTo);
            

            if (reachedTargetWithCarriedObj)
            {
                DropCarriedObjectAfterThisTime(5);
                GoIdleForThisTime(30);
                roamToggle = true;
                backAndPushToggle = false;
                carryThenThrowToggle = false;
                roamCarryToggle = false;
            }
        }

        if (backAndPushToggle)
        {
            Debug.Log("Agent is back and pushing ");
            DropCarriedObjectAfterThisTime(5);
            BackThenPushThisObject(objectToPushAndBack);
            StartCoroutine("ActionDecisionForPlay");
            MSBodyAnimation.PlayWalkingAnimation();
        }

        if (roamToggle)
        {
            Debug.Log("Agent is roaming");
            DropCarriedObjectAfterThisTime(5);
            HandleGoNextPoint();
            StartCoroutine("ActionDecisionForPlay");
            MSBodyAnimation.PlayWalkingAnimation();
        }

        if(roamCarryToggle)
        {
            if(!agentIsNowCarryingSomething)
            {
                MSBodyAnimation.PlayWalkingAnimation();
            }
            Debug.Log("Agent is roaming while carrying");
            CarryThisObject(objectToCarry);
            HandleGoNextPoint();
            StartCoroutine("ActionDecisionForPlay");
        }
    }

    void GoToPlayer()
    {
        StopCoroutine("ContinuousRandomNumberGeneration");
        Debug.Log(currentDestination);
        GoToDestination(frontPoints[0]); //Current Front Point Destination
        //RotateTowards(door);
        if (reachedFrontPointTarget)    
        {
            GoIdleForThisTime(900);
        }
    }

    void GoToDestination(Transform obj)
    {
        StopCoroutine("GoNextPoint");
        StopCoroutine("ActionDecisionForPlay");
        agent.destination = obj.position;
    }

    void GoToDestination(Vector3 obj)
    {
        StopCoroutine("GoNextPoint");
        StopCoroutine("ActionDecisionForPlay");
        agent.destination = obj;
    }

    void RotateTowards(Transform obj)
    {
        agent.isStopped = true;
        int rotateSpeed = 5;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
                ((obj.position - transform.position).normalized), Time.deltaTime * rotateSpeed);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void BackThenPushThisObject(Transform obj)
    {
        StopCoroutine("GoNextPoint");
        if(backBool)
        {
            destPoint = Random.Range(0, points.Length);
            agent.destination = points[destPoint].position;
            agent.speed = 2f;
            backCounter++;
            Debug.Log("Backing");

            if (backCounter > 20)
            {
                backCounter = 0;
                backBool = false;
                pushBool = true;
            }
        }

        else if (pushBool)
        {
            agent.destination = obj.position;
            agent.speed = 4f;
            pushCounter++;
            Debug.Log("Pushing");
            if (pushCounter > 120)
            {
                pushCounter = 0;
                pushBool = false;
                backBool = true;
            }
        }

    }

    void CarryThisObject(Transform obj)
    {
        StopCoroutine("GoNextPoint");
        targetObjectToCarry = obj;
        wantToCarryObject = true;
        if(agentIsNowCarryingSomething == false)
        {
            agent.destination = obj.position;
        }
        else{
            
            objectBeingCarried = obj;
        }
    }

    void CarryObjectTo(Transform destinationPoint)
    {
        if (agentIsNowCarryingSomething == true)
        {
            StopCoroutine("GoNextPoint");
            StopCoroutine("ActionDecisionForPlay");
            agent.destination = destinationPoint.position;
        }
    }

    void DropCarriedObjectAfterThisTime(int forHowManyFrames)
    {
        if (agentIsNowCarryingSomething == true)
        {
            howLongCarryCounter++;

            if (howLongCarryCounter > forHowManyFrames)
            {
                MSBodyAnimation.PlayWalkingAnimation();
                wantToCarryObject = false;
                objectBeingCarried.transform.SetParent(playObjectsParent.transform);
                objectBeingCarried.GetComponent<Rigidbody>().isKinematic = false;
                objectBeingCarried.GetComponent<Rigidbody>().detectCollisions = true;
                Debug.Log("AGENT IS DROPPING OBJECT "+ objectBeingCarried);

                objectBeingCarried = null;
                targetObjectToCarry = null;
                agentIsNowCarryingSomething = false;
                howLongCarryCounter = 0;
                HandleGoNextPoint();
            }
        }
    }

    void GoIdleForThisTime(int forHowManyFrames)
    {
        StopCoroutine("GoNextPoint");
        StopCoroutine("ActionDecisionForPlay");
        agent.isStopped = true;
        noticedInteractionCounter = 0;
        idleCounter++;
        Debug.Log("Agent should be idle");
        if(idleCounter > forHowManyFrames)
        {
            agent.isStopped = false;
            StartCoroutine("GoNextPoint");
            StartCoroutine("ActionDecisionForPlay");
            idleCounter = 0;
            Debug.Log("Agent shouldnt be idle");
            return;
        }
    }

    void StopAgentFromMoving()
    {
        StopCoroutine("GoNextPoint");
        StopCoroutine("ActionDecisionForPlay");
        agent.isStopped = true;
    }

    void StartAgentMoving()
    {
        StartCoroutine("GoNextPoint");
        StartCoroutine("ActionDecisionForPlay");
        agent.isStopped = false;
    }

    void GotoNextPoint()
    {
        if (points.Length == 0) { return; } // Returns if no points have been set up

        destPoint = Random.Range(0, points.Length);
        agent.destination = points[destPoint].position;
    }

    void GoToBed()
    {
        agent.isStopped = false;
        agent.destination = bedObjects[0].position;
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

    IEnumerator ActionDecisionForPlay()
    {
        actionCounter++;
        boredomText.text = "Action Counter" + actionCounter;
        if (actionCounter > actionCounterMax) //Every so frames the agent makes a decision
        {
            int actionDecision = Random.Range(0, 100);

            if (actionDecision <= 25)
            {
                int randomPoint = Random.Range(0, pointsToCarryObjectTo.Length);
                int randomObject = Random.Range(0, playObjects.Length);
                targetPointToCarryObjectTo = pointsToCarryObjectTo[randomPoint];
                objectToCarry = playObjects[randomObject];
                Debug.Log("Point to carry the object to "+targetPointToCarryObjectTo);
                agent.isStopped = false;

                carryThenThrowToggle = true;
                backAndPushToggle = false;
                roamToggle = false;
                roamCarryToggle = false;
            }

            if (actionDecision > 25 && actionDecision <= 50)
            {
                int randomObject = Random.Range(0, playObjects.Length);
                objectToPushAndBack = playObjects[randomObject];

                agent.isStopped = false;

                backAndPushToggle = true;
                carryThenThrowToggle = false;
                roamToggle = false;
                roamCarryToggle = false;
            }

            if (actionDecision > 50 && actionDecision <=75)
            {
                agent.isStopped = false;

                roamToggle = true;
                carryThenThrowToggle = false;
                backAndPushToggle = false;
                roamCarryToggle = false;

            }

            if (actionDecision > 75)
            {
                agent.isStopped = false;
                
                int randomObject = Random.Range(0, playObjects.Length);
                objectToCarry = playObjects[randomObject];

                roamToggle = false;
                carryThenThrowToggle = false;
                backAndPushToggle = false;
                roamCarryToggle = true;

            }

            actionCounter = 0;
        }
            yield return null;
    }

    IEnumerator ActionDecisionForTired()
    {
        actionCounter++;
        boredomText.text = "Action Counter" + actionCounter;
        if (actionCounter > actionCounterMax) //Every so frames the agent makes a decision
        {
            int actionDecision = Random.Range(0, 100);

            if (actionDecision <= 50)
            {
                actionCounter+=500;
                agent.isStopped = true;

                tiredRoamToggle = false;
                stayInPlaceToggle = true;
            }

            if (actionDecision > 50)
            {
                agent.isStopped = false;

                tiredRoamToggle = true;
                stayInPlaceToggle = false;
            }

            actionCounter = 0;
        }
            yield return null;
    }

    IEnumerator ActionDecisionForPaint()
    {
        actionCounter++;
        boredomText.text = "Action Counter" + actionCounter;
        if (actionCounter > actionCounterMax) //Every so frames the agent makes a decision
        {
            int actionDecision = Random.Range(0, 100);

            if (actionDecision <= 50)
            {
                actionCounter+=100;
                agent.isStopped = true;

                continueDrawingToggle = false;
                lookAtPaintingToggle = true;
            }

            if (actionDecision > 50)
            {
                agent.isStopped = true;

                continueDrawingToggle = true;
                lookAtPaintingToggle = false;
            }

            actionCounter = 0;
        }
            yield return null;
    }

    #endregion
    
    #region Other Methods

    void AntiStuck() //Future fix for weird bug where agent gets stuck on something in the
    {
        if (agent.isStopped == false)
        {
            Debug.Log(this.transform);
        }
        //TBC
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


        if(collision.transform.name == targetObjectToGoTo.name)
        { reachedTargetObject = true; }
        else if(collision.transform.name != targetObjectToGoTo.name)
        { reachedTargetObject = false; }


        if (collision.transform.name == bedObjects[0].name)
        { reachedBedObjectTarget = true; }
        else 
        { reachedBedObjectTarget = false; }
    }

    void ConditionMeters()
    {
        if(energyMeter > 0 && agentIsSleeping == false)
        {
            HandleBall.isAgentSleeping = false;
            energyMeter--;
        }

        if (!agentIsSleeping && !agentNoticedPlayerInteraction)
        {
            boredomMeter++;
            if(boredomMeter >= boredomMeterMax)
            {
                boredomMeter = boredomMeterMax;
            }
        }

        if(agentIsSleeping == true)
        {
            DropCarriedObjectAfterThisTime(0);
            HandleBall.isAgentSleeping = true;
            energyMeter++;
        }

        if(energyMeter > energyMeterMax)
        {
            energyMeter = energyMeterMax;
            agentIsSleeping = false;
        }

        if(energyMeter <= 0)
        {
            agentIsSleeping = true;
        }

        energyText.text = ""+energyMeter;
    }

    #endregion
}