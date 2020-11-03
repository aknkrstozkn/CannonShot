using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    void Update()
    {
        // Destroy after fall
        if(transform.position.y < 0f)
        {
            Object.Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            Push(collision);
            transform.parent.gameObject.GetComponent<TargetManager>().targetCount--;
            // Disable Collider for unwanted collision calculations
            GetComponent<Collider>().enabled = false;            
        }
    }

    void Push(Collision collision)
    {
        // Get Rigidbodies
        var collisionRB = collision.gameObject.GetComponent<Rigidbody>();
        var rb = GetComponent<Rigidbody>();

        // Activate Gravity for realistic fall
        rb.useGravity = true;
        // Push itself to the direction of the bullet
        rb.AddForce(collisionRB.velocity, ForceMode.VelocityChange);                               
    }
}
