using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed, jumpSpeed;
    public GameObject fireball;
    public float fireballDelay, fireballSpeed;

    Rigidbody rb;
    PlayerCollision col;

    bool shootingFireball = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<PlayerCollision>();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && col.grounded)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShootFireball());
        }
    }

    IEnumerator ShootFireball()
    {
        if (shootingFireball) { yield break; }
        shootingFireball = true;
        Fireball f = Instantiate(fireball,transform.position,Quaternion.identity).GetComponent<Fireball>();
        
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, 1 << 8);

        f.speed = fireballSpeed;
        f.direction = (hit.point - f.transform.position).normalized;

        yield return new WaitForSeconds(fireballDelay);

        shootingFireball = false;
    }
}
