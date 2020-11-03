using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonMovement : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab = null;
    [SerializeField] Vector3 rotationBoundries = new Vector3(0, 0 , 0);
    [SerializeField] float rotationSpeed = 6f;
    private GameObject bulletSpawn = null;

    void Start()
    {
        bulletSpawn = transform.GetChild(0).gameObject;
    }

    void Update()
    {   
        /*
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        else 
        {   
            if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {    
                Rotate();  
            }
        }

        
        if (Input.touchCount > 0)
        {   
            var touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                Rotate();
            }
            else if(touch.phase == TouchPhase.Began)
            {
               Fire();      
                
            }
        }*/
        
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        } else
        {
            Rotate();
        }         
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        bulletRigidbody.AddForce(transform.forward * bulletPrefab.transform.GetComponent<Bullet>().velocity, ForceMode.Impulse);
    }

    private void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 direction = (ray.GetPoint(100000.0f)).normalized;

        var rotation = Quaternion.LookRotation(direction, Vector3.up);      
        var qua = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        
        // Prevent unwanted rotations
        qua.x = Mathf.Clamp(qua.x, -90, 0);
        qua.z = 0;
        transform.rotation = qua;
    }
}
