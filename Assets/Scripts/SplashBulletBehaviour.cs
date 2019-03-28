using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashBulletBehaviour : Shootable
{
    public override FactoryTypes ObjectType => throw new NotImplementedException();

   
    public override AudioSource FireSound { get => throw new NotImplementedException(); }

    public override int Rotationspeed => throw new NotImplementedException();

    public override float Speed => throw new NotImplementedException();

    public override Collider Col => throw new NotImplementedException();

    public override Rigidbody Rb => throw new NotImplementedException();


    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------

    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
		
    }
    protected override void Update()
    {
        base.Update();
        
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public override void Explode()
    {
        if(Player!= null)
        {
            Player.ClearBullet();
            Player = null;
        }
        MonoFactory.ReturnFactoryObject(this);
    }

    protected override void Move()
    {
        base.Move();
    }

    public override void OnHit(Shootable bullet, Collision collision)
    {
        throw new NotImplementedException();
    }

    public override void ReturnToFactory()
    {
        Explode();
    }

    protected override void ApplyPlayerRotation()
    {
        base.ApplyPlayerRotation();
    }


    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************