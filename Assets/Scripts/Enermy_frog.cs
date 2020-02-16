using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_frog : Enermy
{
    public Transform leftPoint, rightPoint;
    public Rigidbody2D rb;
    public float speed, jumpForce;
    public Transform groundCheck;
    public LayerMask ground;
    private bool faceLeft = true;
    private float left, right;
    public bool isGround;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        left = leftPoint.position.x;
        right = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        switchAnimation();
    }

    void Move()
    {
        if (faceLeft)
        {
            if (isGround)
            {
                rb.velocity = new Vector2(-speed, jumpForce);
                anim.SetBool("toJump", true);
                anim.SetBool("toFall", false);
            }
            if (transform.position.x <= left)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;              
            }
        }
        else
        {
            if (isGround)
            {
                rb.velocity = new Vector2(speed, jumpForce);
                anim.SetBool("toJump", true);
                anim.SetBool("toFall", false);
            }
            if (transform.position.x >= right)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;
            }
        }
    }

    void switchAnimation()
    {
        if (isGround && anim.GetBool("toFall"))
        {
            anim.SetBool("toJump", false);
            anim.SetBool("toFall", false);
        }
        if (!isGround && rb.velocity.y < 0)
        {
            anim.SetBool("toJump", false);
            anim.SetBool("toFall", true);
        }
        if (anim.GetBool("toJump"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("toJump", false);
                anim.SetBool("toFall", true);
            }
        }

    }
}
