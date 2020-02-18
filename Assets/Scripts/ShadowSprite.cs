using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    //获取人物位置
    private Transform player;

    //获取人物图层
    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    //设置颜色
    private Color color;

    [Header("时间控制参数")]
    public float activeTime;//shadow显示时间
    public float activeStart;//shadow开始显示的时间

    [Header("透明度控制")]
    private float alpha;
    public float alphaSet;//初始值
    public float alphaMutiplier;//变化的速度

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMutiplier;

        color = new Color(0.5f,0.5f,1,alpha);

        thisSprite.color = color;

        //超过shadow存在时间后
        if (Time.time >= activeStart + activeTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(gameObject);
        }
    }
}
