using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float velocity = 50f;
    // Start is called before the first frame update
    void Start()
    {
        // Resize colliders size according to speed
        // To be able collide at high speed.
        ChangeCollider();
    }

    void ChangeCollider()
    {
        GetComponent<CapsuleCollider>().height = velocity / 100f;
        GetComponent<CapsuleCollider>().center = new Vector3(0, 0, (GetComponent<CapsuleCollider>().height / 100f) * 35);
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy
        if(transform.position.y < 0f)
        {
            Object.Destroy(this.gameObject);
        }
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Target"))
        {
            Push(collision);
        }
    }

    void Push(Collision collision)
    {
        var collisionRB = collision.gameObject.GetComponent<Rigidbody>();
        var rb = GetComponent<Rigidbody>();     
        Vector3 direction = collision.transform.position - transform.position;

        collisionRB.useGravity = true;
        collisionRB.AddForce(rb.velocity, ForceMode.VelocityChange);                               
    }
    */
}
