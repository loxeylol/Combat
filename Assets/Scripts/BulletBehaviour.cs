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

    private AudioSource _fireSound;
    // --- Properties -------------------------------------------------------------------------------------------------
    public override FactoryTypes ObjectType => FactoryTypes.Bullet;

    public override Collider Col => GetComponent<SphereCollider>();
    public override Rigidbody Rb => GetComponent<Rigidbody>();
    public override AudioSource FireSound => GetComponent<AudioSource>();

    public override int Rotationspeed => _rotationSpeed;

    public override float Speed => _speed;


    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        StartCoroutine(LifetimeRoutine());
        FireSound.Play();
    }

    //private void Update()
    //{
    //    if (GameController.Instance.IsPaused)
    //    {
    //        return;
    //    }

    //        if (SettingsManager.CanBulletsBeDirected)
    //        {
    //            ApplyPlayerRotation();
    //        }

    //        Move();
        
    //}

    // --------------------------------------------------------------------------------------------

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

   

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


    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************