using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BulletBehaviour : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField, Range(0f, 10f)] private float _speed = 7f;
    [SerializeField, Range(0.1f, 5f)] private float _lifeTime = 2f;

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
        Destroy(this.gameObject, _lifeTime);
       
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

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void ApplyPlayerRotation()
    {
        transform.Rotate(Vector3.up, Player.RotationInput * _rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        PlayerController hitPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (hitPlayer != null)
        {
            if (hitPlayer != Player || SettingsManager.FriendlyFire)
            {
                if (!hitPlayer.IsInvincible)
                {
                    Player.Score += Player == hitPlayer ? -1 : +1;
                    
                    Player.StartCoroutineWhenHitting();
                    //hitPlayer.GetHit(collision);
                    hitPlayer.GetHitInDirecTionFromBullet(transform.forward, collision);
                }
                
            }
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (!SettingsManager.BouncyBullets)
            {
                Destroy(this.gameObject);
                return;
            }

            Vector3 newDir = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
            newDir.y = 0f;
            transform.forward = newDir.normalized;
        }

    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************