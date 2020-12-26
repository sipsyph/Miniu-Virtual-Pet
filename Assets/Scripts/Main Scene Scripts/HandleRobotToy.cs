using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HandleRobotToy : MonoBehaviour {

    private NavMeshAgent agent;
    public int destPoint = 0, roamCtr = 0, roamCtrMax = 240;
    public Transform throwParent;
    private bool goRoam = true;
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine("GoNextPoint");
    }
	
	// Update is called once per frame
	void Update () {

        if(this.transform.parent.name == "Ball Handler" || this.transform.parent.name == "Throw Parent")
        {
            agent.enabled = false;
            //Debug.Log("Robot Toy is disabled");
        }
        else
        {
            //Debug.Log("Robot Toy is enabled");
            agent.enabled = true;
        }

        if(agent.enabled)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f) { StartCoroutine("GoNextPoint"); }
            roamCtr++;
            if (roamCtr >= roamCtrMax)
            {
                agent.ResetPath();
                roamCtr = 0;
            }
        }
    }

    IEnumerator GoNextPoint()
    {
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = Random.Range(0, MiniuAgent.staticPoints.Length);
        // Set the agent to go to the currently selected destination.
        agent.destination = MiniuAgent.staticPoints[destPoint].position;
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Miniu Agent" && MiniuAgent.wantToCarryObject
            && MiniuAgent.targetObjectToCarry.name == this.transform.name)
        {
            //Debug.Log("Colliding with agent");
            StopCoroutine("GoNextPoint");
            agent.enabled = false;
        }
    }


}
