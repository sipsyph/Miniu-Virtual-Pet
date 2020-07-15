using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HandleOtherWorldAgent : MonoBehaviour {

    public float movementSpeed;
    private NavMeshAgent agent;
    private Rigidbody rb;
    public static GameObject currentTargetHittoHit;
    public GameObject frontSphere, frontLeftSphere, frontRightSphere, leftSphere, rightSphere, rotatorObj;
    Vector3 lockThisY;

    private int decisionCounter;
    public static bool canWalk;
    // Use this for initialization
    void Start () {
        movementSpeed = 10f;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lockThisY.y = transform.position.y;

        decisionCounter = 0;
        canWalk = true;
    }
	
	// Update is called once per frame
	void Update () {

        DecisionMaking();
        

        if(canWalk)
        {
            FixRotation(currentTargetHittoHit.transform, 0.05f);
            FixRotation(rotatorObj, currentTargetHittoHit.transform, 1f);
            transform.Translate(-transform.forward * (Time.deltaTime * movementSpeed));
        }
        
        //new Vector3(currentTargetHittoHit.transform.position.x, lockThisY.y, -currentTargetHittoHit.transform.position.z)
        LockYAxis();
    }

    void DecisionMaking()
    {
        decisionCounter++;
        //Debu
        if(decisionCounter >= 180)
        {
            int choice = Random.Range(0, 2);

            if(choice > 0)
            {
                //StopAndLookAround();
                canWalk = true;
            }

            else if(choice == 0)
            {
                canWalk = true;
            }

            Debug.Log(choice);

            decisionCounter = 0;
        }
    }

    void StopAndLookAround()
    {
        canWalk = false;
        FixRotation(rotatorObj, leftSphere.transform, 8f);
    }

    void LockYAxis()
    {
        lockThisY = transform.position;
        lockThisY.y = -0.3f;

        transform.position = lockThisY;
    }

    void FixRotation(Transform targetTransform, float stepAmt)
    {
        Vector3 targetDir = targetTransform.transform.position - transform.position;

        // The step size is equal to speed times frame time.
        float step = stepAmt * Time.deltaTime; //DEfault 0.05f

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        //Debug.DrawRay(transform.position, newDir, Color.red);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void FixRotation(GameObject objToRotate, Transform targetTransform, float stepAmt)
    {
        Vector3 targetDir = targetTransform.transform.position - transform.position;

        // The step size is equal to speed times frame time.
        float step = stepAmt * Time.deltaTime; //DEfault 0.05f

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        //Debug.DrawRay(transform.position, newDir, Color.red);

        // Move our position a step closer to the target.
        objToRotate.transform.rotation = Quaternion.LookRotation(newDir);
    }
}
