using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BulletBehaviour : Shootable
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField, Range(0f, 10f)] private float _speed = 7f;
    [SerializeField, Range(0, 360)] private int _rotationSpeed = 90;

    private SphereCollider _col;
    private Rigidbody _rb;
    private AudioSource _fireSound;

    // --- Properties -------------------------------------------------------------------------------------------------
    public override FactoryTypes ObjectType => FactoryTypes.Bullet;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<SphereCollider>();
        _fireSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        StartCoroutine(LifetimeRoutine());
        _fireSound.Play();
    }

    private void Update()
    {
        if (!GameController.Instance.IsPaused)
        {

            if (SettingsManager.CanBulletsBeDirected)
            {
                ApplyPlayerRotation();
            }

            Move();
        }



    }

    // --------------------------------------------------------------------------------------------

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    public override void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    public override void Explode()
    {
        if (Player != null)
        {
            Player.ClearBullet();
            Player = null;
        }
        MonoFactory.ReturnFactoryObject(this);
    }

    public override void OnHit(Shootable bullet, Collision collision)
    {
        //nothing to do here
    }

    public override void ReturnToFactory()
    {
        Explode();
    }


    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void ApplyPlayerRotation()
    {
        transform.Rotate(Vector3.up, Player.RotationInput * _rotationSpeed * Time.deltaTime);
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************