using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCubeToFollow : MonoBehaviour {

    // Use this for initialization
    public Animator cubeAnim;
	void Start () {
        HandlePlane.currentCube = this.transform;
        Invoke("DestroyThisObject", 2);
	}
	
	// Update is called once per frame
	void Update () {
        if(HandlePlane.GoDestroy)
        {
            DestroyThisObject();
            HandlePlane.GoDestroy = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Actable Plane Group")
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX;
            SetThisAsLastTouchedPos();
            cubeAnim.SetTrigger("ExpandSphereTrigger");
            Utilities.SetLastPlaneClickedPosition(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }

        if(collision.transform.name == "Miniu Model" || collision.transform.tag == "Play Object")
        {
            DestroyThisObject();
        }
    }

    void SetThisAsLastTouchedPos()
    {
        Utilities.SetLastPlaneClickedPosition(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        HandleSupervisor.lastTouchedPos = this.transform;
    }

    void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }
}
