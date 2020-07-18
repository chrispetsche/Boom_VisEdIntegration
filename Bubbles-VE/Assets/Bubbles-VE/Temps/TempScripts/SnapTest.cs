using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTest : MonoBehaviour
{
    Rigidbody body;
    bool hitting = false;
    Vector3 collisionPt;
    Collider myColl;

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        myColl = gameObject.GetComponent<BoxCollider>();
        collisionPt = Vector3.zero;
    }

    private void Update()
    {
        //transform.Translate(0f, 0f, 1f * Time.deltaTime);
        


        body.WakeUp();
        if (body.IsSleeping())
        {
            Debug.Log("Body Sleeping!!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myColl.isTrigger = false;
    }


    private void OnCollisionExit(Collision collision)
    {
        hitting = false;
    }

    void OnCollisionEnter(Collision collision)
    {
       myColl.isTrigger = true;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 collisionPt = contact.point;
        Debug.Log(collisionPt);

        transform.position =  collisionPt;
    }
}
