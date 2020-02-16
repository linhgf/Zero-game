using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_eagle : Enermy
{
    public float speed;
    public Transform top, bottom;
    private float topY, bottomY;
    public Rigidbody2D rb;
    bool toTop = false;//判断是否到达top处
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        topY = top.position.y;
        bottomY = bottom.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (!toTop)
        {
            rb.velocity = new Vector2(0, speed);
            if (transform.position.y >= topY)
            {
                toTop = true;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, -speed);
            if (transform.position.y <= bottomY)
            {
                toTop = false;
            }
        }
    }
}
