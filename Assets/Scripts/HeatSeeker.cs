using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HeatSeeker : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] private float moveSpeed = 350f;
    [SerializeField] private float turnSpeed = 2000f; // Degrees per second
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        //target = FindObjectOfType<Enemy>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        HeatSeekerMove();
    }

    private void HeatSeekerMove()
    {
        target = FindObjectOfType<Enemy>().transform;
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * turnSpeed;
        rb.velocity = transform.up * moveSpeed;
        if (target == null)
        {
            target = FindObjectOfType<Enemy>().transform;
        }
        else
        {
            target = GetComponent<Transform>();
        }
    }
}
