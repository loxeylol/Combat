using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelBounds : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] bool _isHorizontalBorder = true;
    [SerializeField] private float _offset = .2f;

    private BoxCollider _collider;
    private AudioSource _wallHitSound;
    // --- Properties -------------------------------------------------------------------------------------------------

    public bool IsHorizontalBorder { get => _isHorizontalBorder; }
    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _wallHitSound = GetComponent<AudioSource>();
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {

                Vector3 playerpos = player.gameObject.transform.position;

                if (_isHorizontalBorder && !player.CanMove )
                {
                    playerpos.x = playerpos.x > 0 ? -playerpos.x + _offset : -playerpos.x - _offset;
                    collision.gameObject.transform.position = playerpos;
                }
                else if (!player.CanMove)
                {
                    playerpos.z = playerpos.z > 0 ? -playerpos.z + _offset : -playerpos.z - _offset;
                    collision.gameObject.transform.position = playerpos;
                }

            }
           

        

    }
    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************