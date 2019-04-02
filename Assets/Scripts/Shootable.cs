using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neox.Helpers;

public abstract class Shootable : MonoBehaviour, IFactoryObject, IHittable
{

    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------

    [SerializeField, Range(0f, 10f)] protected float _speed = 7f;
    [SerializeField, Range(0, 360)] protected int _rotationSpeed = 90;
    protected float _currentSpeed;
    protected Coroutine _lifetimeRoutine;

    // --- Properties -------------------------------------------------------------------------------------------------
    public PlayerController Player { get; set; }

    public abstract FactoryTypes ObjectType { get; }
    public abstract Collider Col { get; }
    public abstract Rigidbody Rb { get; }
    public abstract AudioSource FireSound { get; }

    public virtual bool ExplodeShootable => true;
    public virtual bool ReflectShootable => false;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    protected virtual void OnEnable()
    {
        _currentSpeed = _speed;
        FireSound.Play();

        _lifetimeRoutine = StartCoroutine(LifetimeRoutine());
    }

    protected virtual void Update()
    {
        if (GameController.Instance.IsPaused)
        {
            return;
        }

        if (SettingsManager.CanBulletsBeDirected)
        {
            ApplyPlayerRotation();
        }
        Move();

    }

    // --------------------------------------------------------------------------------------------
    protected virtual void OnCollisionEnter(Collision collision)
    {
        IHittable hitable = collision.gameObject.GetComponent<IHittable>();
        if (hitable == null)
            return;

        hitable.OnHit(this, collision);

        if (SettingsManager.BouncyBullets && hitable.ReflectShootable)
        {
            Reflect(collision);
        }
        else if (hitable.ExplodeShootable)
        {
            Explode();
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    abstract public void OnHit(Shootable bullet, Collision collision);

    virtual public void ReturnToFactory()
    {
        if (Player != null)
        {
            Player.ClearBullet();
            Player = null;
        }
        MonoFactory.ReturnFactoryObject(this);
    }

    public void Reflect(Collision collision)
    {
        Vector3 newDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
        newDir.y = 0f;
        transform.forward = newDir.normalized;
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    protected virtual void Move()
    {
        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }

    protected virtual void ApplyPlayerRotation()
    {
        transform.Rotate(Vector3.up, Player.RotationInput * _rotationSpeed * Time.deltaTime);
    }
    abstract protected void Explode();

    // --------------------------------------------------------------------------------------------
    protected IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(SettingsManager.BulletLifeTime);
        Debug.Log("Lifetimeroutine Explode!");
        Explode();
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************