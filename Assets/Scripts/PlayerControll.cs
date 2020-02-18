using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerControll : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D coll;
    public Collider2D topColl;
    public Animator anim;
    public GameObject gameover;
    
    public float speed, jumpForce;
    public Transform groundCheck;
    public LayerMask ground;
    public Transform topCheck;

    [Header("Dash参数")]
    public float dashTime;//dash时长
    public float dashSpeed;//dash速度
    public float dashCoolDown;//dash CD时长
    private float lastDash = -10f;//上一次dash时间点
    private float dashTimeLeft;//dash剩余时间
    private float dashFace;//dash方向

    private float horizontalMove;
    public bool isGround, isJumping,isHurted,isCrouch,isDashing;
    public bool jumpPressed,crouchPressed;
    //跳跃次数
    public int jumpCount;
    //分数
    public int cherry = 0;
    public Text Score;
    //音源
    public AudioSource jumpAudio,hurtAudio,cherryAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpAudio.Play();
            jumpPressed = true;
        }
        
        if (Input.GetButtonDown("Crouch"))
        {
            crouchPressed = true;
        }

        if (Input.GetButtonUp("Crouch"))
        {
            crouchPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //冷却结束 可以执行dash
                ReadyToDash();
            }
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);

        if (!isHurted)
        {
            Dash();
            if (isDashing)
                return;
            Move();
            Jump();
            Crouch();
        }
        switchAnimation();
    }
    //move
    void Move()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
            dashFace = horizontalMove;
        }
    }
    //jump
    void Jump()//跳跃
    {
        if (isGround && rb.velocity.y == 0)
        {
            jumpCount = 2;//可跳跃数量
            isJumping = false;
        }
        if (jumpPressed && isGround)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void Crouch()
    {
        if (crouchPressed)
        {
            topColl.enabled = false;
            isCrouch = true;
        }
        else if (crouchPressed == false)
        {
            if (!Physics2D.OverlapCircle(topCheck.position, 0.2f, ground))
            {
                topColl.enabled = true;
                isCrouch = false;
            }
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
           //anim.SetBool("toIdle", false);
        }
        else if (isGround)
        {
            //anim.SetBool("toIdle", true);
            anim.SetBool("toFall", false);
            anim.SetBool("toJump", false);
            //anim.SetBool("toHurted", false);
        }
        if (isCrouch)
        {
            anim.SetBool("toCrouch", true);
        }
        else if (!isCrouch)
        {
            anim.SetBool("toCrouch", false);
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
            cherryAudio.Play();
            Destroy(collision.gameObject);
            cherry += 1;
            Score.text = cherry.ToString();
        }
        if (collision.gameObject.tag == "Enermy")
        {
            Enermy enermy = collision.gameObject.GetComponent<Enermy>();
            enermy.Showdeath();
            jumpCount = 2;
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
                hurtAudio.Play();
                isHurted = true;
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                isHurted = true;
                rb.velocity = new Vector2(speed, jumpForce);
            }
        }

        if (collision.gameObject.tag == "DeadLine")
        {
            gameover.SetActive(true);
            Invoke("Reastart", 2f);//延时调用
        }
    }
    //reastart the game
    private void Reastart()
    {
        //调用静态方法
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //dash预备
    void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }

    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if (!isGround && rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(dashSpeed * dashFace, jumpForce);
                }
                rb.velocity = new Vector2(dashSpeed * dashFace, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                if (!isGround)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }
        }
    }

}
