using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BulletBehaviour : MonoBehaviour, IHittable, IFactoryObject
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField, Range(0f, 10f)] private float _speed = 7f;

    [SerializeField] private bool _canBeDirected = true;
    [SerializeField, Range(0, 360)] private int _rotationSpeed = 90;

    
    private SphereCollider _col;
    private LevelBounds _wall;
    private Rigidbody _rb;


    // --- Properties -------------------------------------------------------------------------------------------------
    public PlayerController Player { get; set; }
    

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<SphereCollider>();
        //Destroy(this.gameObject, _lifeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(LifetimeRoutine());
    }

    private void Update()
    {
        if (SettingsManager.CanBulletsBeDirected)
        {
            ApplyPlayerRotation();
        }

        Move();

        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }

    // --------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        IHittable hitable = collision.gameObject.GetComponent<IHittable>();
        if (hitable != null)
        {
            hitable.OnHit(this, collision);
            Explode();
            return;
        }

        if (!SettingsManager.BouncyBullets)
        {
            Explode();
            return;
        }

        // Reflect
        Vector3 newDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
        newDir.y = 0f;
        transform.forward = newDir.normalized;
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    void IHittable.OnHit(BulletBehaviour bullet, Collision collision)
    {
        // Calling expolde here will cause Explode to be called twice
        //Explode();
    }

    void IFactoryObject.ReturnToFactory()
    {
        Explode();
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(SettingsManager.BulletLifeTime);
        Explode();
    }

    private void ApplyPlayerRotation()
    {
        transform.Rotate(Vector3.up, Player.RotationInput * _rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void Explode()
    {
        if (Player != null)
        {
            Player.ClearBullet();
            Player = null;
        }
        MonoFactory.ReturnFactoryObject(this);
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************