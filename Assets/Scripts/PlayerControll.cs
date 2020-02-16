﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D coll;
    public Animator anim;

    
    public float speed, jumpForce;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump,isHurted;
    public bool jumpPressed;

    public int jumpCount;
    public int cherry = 0;
    public Text Score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);

        if (!isHurted)
        {
            Move();
            Jump();
        }
        switchAnimation();
    }
    //move
    void Move()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }
    //jump
    void Jump()//跳跃
    {
        if (isGround && rb.velocity.y == 0)
        {
            jumpCount = 2;//可跳跃数量
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    //select animation
    void switchAnimation()
    {
        anim.SetFloat("toRun", Mathf.Abs(rb.velocity.x));
        if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("toJump", true);
            anim.SetBool("toFall", false);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("toFall", true);
            anim.SetBool("toJump", false);
            anim.SetBool("toIdle", false);
        }
        else if (isGround)
        {
            anim.SetBool("toIdle", true);
            anim.SetBool("toFall", false);
            anim.SetBool("toHurted", false);
        }
        if (isHurted)
        {
            anim.SetBool("toHurted", true);
            if (Mathf.Abs(rb.velocity.x) < speed * 0.3)
            {
                anim.SetBool("toHurted", false);
                isHurted = false;
            }
        }
    }

    //trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            cherry += 1;
            Score.text = cherry.ToString();
        }
        if (collision.gameObject.tag == "Enermy")
        {
            Enermy enermy = collision.gameObject.GetComponent<Enermy>();
            enermy.Showdeath();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    //collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enermy")
        {
            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                isHurted = true;
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                isHurted = true;
                rb.velocity = new Vector2(speed, jumpForce);
            }
        }
       
    }
}
