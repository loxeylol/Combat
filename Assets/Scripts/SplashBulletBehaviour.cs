using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neox.Helpers;
public class SplashBulletBehaviour : Shootable
{






    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    private float _radius = 0;
    private bool _hitPlayer = false;

    private MeshRenderer _meshRenderer => GetComponent<MeshRenderer>();
    // --- Properties -------------------------------------------------------------------------------------------------
    public override FactoryTypes ObjectType => FactoryTypes.SplashBullet;

    public override AudioSource FireSound => GetComponent<AudioSource>();

    public override Collider Col => GetComponent<SphereCollider>();

    public override Rigidbody Rb => GetComponent<Rigidbody>();

    private ParticleSystem Explosion => GetComponent<ParticleSystem>();
    // --- Unity Functions --------------------------------------------------------------------------------------------
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(Player.FireKey) && !Explosion.isPlaying)
        {
            Explode();
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _hitPlayer = false;
        _meshRenderer.enabled = true;
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public override void OnHit(Shootable bullet, Collision collision)
    {
        //nothing to do here
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    protected override void Explode()
    {
        Debug.Log("Explode! ");
        StartCoroutine(ExplodeWithParticle());

    }
    private IEnumerator ExplodeWithParticle()
    {
        _currentSpeed = 0;
        _meshRenderer.enabled = false;
        StopCoroutine(_lifetimeRoutine);
        Explosion.Play();
        while (Explosion.time < Explosion.main.duration - 0.5f)
        {
            _radius = NeoxMath.Lerp(0, 1.5f, Explosion.time, InterpolationTypes.Linear);
            CheckCollisionRadius(_radius);
            yield return null;
        }

        ReturnToFactory();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }

    private void CheckCollisionRadius(float radius)
    {

        Collider[] AllCollisions = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in AllCollisions)
        {
            PlayerController _Player = col.GetComponent<PlayerController>();
            if (_Player != null && !_hitPlayer)
            {
                _hitPlayer = true;
                _Player.OnHit(this, null);
                Debug.Log($"Hit Player {_Player.name}");

            }
        }
    }
    protected override void Move()
    {
        base.Move();
    }
    protected override void ApplyPlayerRotation()
    {
        base.ApplyPlayerRotation();
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************