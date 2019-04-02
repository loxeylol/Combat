using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neox.Helpers;


public class LevelObject : MonoBehaviour, IHittable, IFactoryObject
{

    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    private AudioSource _wallHitSound;

    // --- Properties -------------------------------------------------------------------------------------------------
    public virtual bool ExplodeShootable => true;
    public virtual bool ReflectShootable => true;

    public FactoryTypes ObjectType => FactoryTypes.Cube;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _wallHitSound = GetComponent<AudioSource>();
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    public void OnHit(Shootable bullet, Collision collision)
    {
        _wallHitSound.Play();
        //if (!SettingsManager.BouncyBullets)
        //{
        //    bullet.Explode();
        //}
        //bullet.Reflect(collision);
    }

    public void ReturnToFactory()
    {
        MonoFactory.ReturnFactoryObject(this);
    }


    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************