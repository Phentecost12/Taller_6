using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet_Manager : MonoBehaviour
{
    [Header("Marbles")]
    [SerializeField] private Marble marblePrefab;
    [SerializeField] private int marblePool_DefaultCapacity;
    [SerializeField] private int marblePool_MaxCpacity;
    [SerializeField] private bool marblePool_Check;

    [Header("Bowling Balls")]
    [SerializeField] private BowlingBall bowlPrefab;
    [SerializeField] private int bowlPool_DefaultCapacity;
    [SerializeField] private int bowlPool_MaxCpacity;
    [SerializeField] private bool bowlPool_Check;

    private ObjectPool<Marble> marblePool;
    private ObjectPool<BowlingBall> bowlPool;
    public static Bullet_Manager Instance { get; private set; } = null;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        marblePool = new ObjectPool<Marble>(() =>
        {
            return Instantiate(marblePrefab);
        }, marble =>
        {
            marble.gameObject.SetActive(true);
        }, marble =>
        {
            marble.gameObject.SetActive(false);
        }, marble =>
        {
            Destroy(marble.gameObject);
        }, marblePool_Check, marblePool_DefaultCapacity, marblePool_MaxCpacity);

        bowlPool = new ObjectPool<BowlingBall>(() =>
        {
            return Instantiate(bowlPrefab);
        }, bowling =>
        {
            bowling.gameObject.SetActive(true);
        }, bowling =>
        {
            bowling.gameObject.SetActive(false);
        }, bowling =>
        {
            Destroy(bowling.gameObject);
        }, bowlPool_Check, bowlPool_DefaultCapacity, bowlPool_MaxCpacity);

        StartPool();
    }

    private void StartPool()
    {
        for(int i = 0; i < marblePool_DefaultCapacity; i++)
        {
            var bullet = marblePool.Get();
            marblePool.Release(bullet);
        }
        for (int i = 0; i < bowlPool_DefaultCapacity; i++)
        {
            var bullet = bowlPool.Get();
            bowlPool.Release(bullet);
        }
    }
}
