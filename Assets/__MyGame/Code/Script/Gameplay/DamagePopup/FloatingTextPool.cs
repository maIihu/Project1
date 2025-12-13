using System.Collections.Generic;
using UnityEngine;

public class FloatingTextPool : MonoBehaviour
{
    public static FloatingTextPool Instance;

    [Header("Prefab & Parent")]
    public FloatingText floatingTextPrefab;
    public Transform poolParent;

    [Header("Init")]
    public int initialCount = 10;

    private readonly Queue<FloatingText> pool = new Queue<FloatingText>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialCount; i++)
            CreateNew();
    }

    private FloatingText CreateNew()
    {
        var ft = Instantiate(floatingTextPrefab, poolParent);
        ft.gameObject.SetActive(false);
        pool.Enqueue(ft);
        return ft;
    }

    public FloatingText Get()
    {
        if (pool.Count == 0)
            CreateNew();

        var ft = pool.Dequeue();
        ft.gameObject.SetActive(true);
        return ft;
    }

    public void Release(FloatingText ft)
    {
        if (ft == null) return;

        ft.gameObject.SetActive(false);
        pool.Enqueue(ft);
    }

}