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
    private AudioSource _fireSound;
    // --- Properties -------------------------------------------------------------------------------------------------
    public override FactoryTypes ObjectType => FactoryTypes.Bullet;

    public override Collider Col => GetComponent<SphereCollider>();
    public override Rigidbody Rb => GetComponent<Rigidbody>();
    public override AudioSource FireSound => GetComponent<AudioSource>();


    // --- Unity Functions --------------------------------------------------------------------------------------------    

    // --------------------------------------------------------------------------------------------

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public override void OnHit(Shootable bullet, Collision collision)
    {
        //nothing to do here
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    protected override void Explode()
    {
        ReturnToFactory();
    }


    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************