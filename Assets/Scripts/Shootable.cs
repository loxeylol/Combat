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
    private float _lifetime = SettingsManager.BulletLifeTime;
    // --- Properties -------------------------------------------------------------------------------------------------
    public PlayerController Player { get; set; }

    public abstract FactoryTypes ObjectType { get; }

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {

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
        if (hitable != null)
        {
            hitable.OnHit(this, collision);
        }
        

    }
    public void Reflect(Collision collision)
    {
        Vector3 newDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
        newDir.y = 0f;
        transform.forward = newDir.normalized;
    }

    abstract public void Move();
    abstract public void Explode();


    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************