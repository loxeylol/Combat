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


    // --- Properties -------------------------------------------------------------------------------------------------
    public PlayerController Player { get; set; }


    public abstract FactoryTypes ObjectType { get; }
    public abstract int Rotationspeed { get; }
    public abstract float Speed { get; }
    public abstract Collider Col { get; }
    public abstract Rigidbody Rb { get; }
    public abstract AudioSource FireSound { get; }

    public virtual bool ExplodeShootable => true;
    public virtual bool ReflectShootable => false;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {

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
        Debug.Log("Shootableupdate");
        Move();

    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    abstract public void OnHit(Shootable bullet, Collision collision);

    abstract public void ReturnToFactory();

    public IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(SettingsManager.BulletLifeTime);

        Explode();
    }
    public void OnCollisionEnter(Collision collision)
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

    public void Reflect(Collision collision)
    {
        Vector3 newDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
        newDir.y = 0f;
        transform.forward = newDir.normalized;
    }

    protected virtual void Move()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    protected virtual void ApplyPlayerRotation()
    {
        transform.Rotate(Vector3.up, Player.RotationInput * Rotationspeed * Time.deltaTime);
    }
    abstract public void Explode();


    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************