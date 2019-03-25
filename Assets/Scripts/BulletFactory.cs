using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance { get; private set; }
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private BulletBehaviour _bulletPrefab;
    [SerializeField, Range(0, 100)] private int _bulletStartAmount = 2;
    [SerializeField] private bool _allowGrowth = true;

    private Queue<BulletBehaviour> _bullets;
    private int _bulletCounter = 0;

    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        CreateBullets();
    }

    private void CreateBullets()
    {
        _bullets = new Queue<BulletBehaviour>();

        for (int i = 0; i < _bulletStartAmount; i++)
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        _bulletCounter++;
        BulletBehaviour bullet = Instantiate(_bulletPrefab, this.transform, true);
        bullet.name = $"Bullet_{_bulletCounter.ToString("D3")}";
        bullet.transform.position = Vector3.one * 10;
        bullet.gameObject.SetActive(false);
        _bullets.Enqueue(bullet);
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------


    public static BulletBehaviour GetBullet()
    {
        return Instance._GetBullet();
    }

    public static void ReturnBullet(BulletBehaviour bullet)
    {
        Instance._ReturnBullet(bullet);        
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private BulletBehaviour _GetBullet()
    {
        if (_bullets.Count == 0)
        {
            if (!_allowGrowth)
                return null;

            CreateBullet();
        }

        BulletBehaviour b = _bullets.Dequeue();
        b.gameObject.SetActive(true);
        return b;
    }

    private void _ReturnBullet(BulletBehaviour bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.position = Vector3.one * 10;
        _bullets.Enqueue(bullet);
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************