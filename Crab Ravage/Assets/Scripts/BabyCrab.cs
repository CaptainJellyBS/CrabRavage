using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCrab : MonoBehaviour
{

    bool grounded;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
        StartCoroutine(Behavior());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    IEnumerator Behavior()
    {
        while(transform.position.y < 8.3f)
        {
            transform.position += Vector3.up * Time.deltaTime * moveSpeed * 6;
            yield return null;
        }

        transform.position = new Vector3(Random.Range(-8.0f, 8.0f), transform.position.y, 0);

        yield return new WaitForSeconds(1.5f);

        while (!grounded)
        {
            yield return null;
        }

        Vector3 dir;

        if(PlayerMovement.Instance.transform.position.x <= transform.position.x)
        {
            dir = Vector3.left;
        }
        else
        {
            dir = Vector3.right;
        }

        while(transform.position.x < 12 && transform.position.x > -12)
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
