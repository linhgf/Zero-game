using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    //shadow预制体
    public GameObject shadowPrefab;
    //shadow数量
    public int shadowCount;

    private Queue<GameObject> shadowQueue = new Queue<GameObject>();

    private void Awake()
    {
        //单列模式
        instance = this;

        //初始化对象池
        fillPool();
    }

    public void fillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var tempShadow = Instantiate(shadowPrefab);
            tempShadow.transform.SetParent(transform);

            //初始化后，取消启用，返回对象池
            ReturnPool(tempShadow);
        }
    }

    public void ReturnPool(GameObject shadow)
    {
        shadow.SetActive(false);
        shadowQueue.Enqueue(shadow);
    }

    public GameObject GetFromPool()
    {
        if (shadowQueue.Count == 0)
        {
            fillPool();
        }

        var outShadow = shadowQueue.Dequeue();

        outShadow.SetActive(true);
        return outShadow;
    }
}
