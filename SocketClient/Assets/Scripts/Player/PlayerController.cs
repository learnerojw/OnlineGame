using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isJump;

    public float moveSpeed;
    public float jumpSpeed;

    public Transform groundPTr;
    public Transform gunTr;
    
    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        gunTr = transform.GetChild(1);
        gunTr.AddComponent<GunController>();
        groundPTr = transform.GetChild(2);
        moveSpeed = 10;
        jumpSpeed = 20;
        
    }

    private void Update()
    {
        RayCheck();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float moveH = Input.GetAxis("Horizontal");
        if(moveH!=0)
        {
            rb.velocity = new Vector2(moveH * moveSpeed, rb.velocity.y);
        }
        
    }

    private void Jump()
    {
        if(isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            isJump = false;
        }
    }

    private void RayCheck()
    {
        Vector2 size = new Vector2(1, 0.25f);
        Collider2D collider2D=Physics2D.OverlapBox(groundPTr.transform.position, size, 0);        
        if (collider2D != null&& Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundPTr.position, new Vector2(1, 0.25f));
    }
}
