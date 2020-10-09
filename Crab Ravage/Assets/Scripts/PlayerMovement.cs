using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed, jumpSpeed;

    public static PlayerMovement Instance { get; private set; }

    Rigidbody rb;
    PlayerCollision col;
    private void Awake()
    {
        Instance = this;
    }

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
            Vector3 newPos = transform.position + transform.forward * movementSpeed * Time.deltaTime;
            if(newPos.x >= -9.5 && newPos.x <= 9.5)
            {
                transform.position = newPos;
            }
        }

        if (Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && col.grounded)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    public Vector3 GetLocationUnderPlayer()
    {
        return new Vector3(transform.position.x, -4.08f, transform.position.z);
    }
}
