using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neox.Helpers;
public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance { get; private set; }
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField ]private List<GameObject> _bulletList;
    
	// --- Properties -------------------------------------------------------------------------------------------------
    
	// --- Unity Functions --------------------------------------------------------------------------------------------
	private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public bool GetAFreeBullet(PlayerController player)
    {
        foreach(GameObject bullet in _bulletList)
        {
             if(!bullet.activeSelf)
            {
                bullet.GetComponent<BulletBehaviour>().Player = player;
                bullet.transform.position = player.BulletSpawn.position;
                bullet.transform.rotation = player.BulletSpawn.rotation;
                bullet.SetActive(true);
                return (true);
            }
        }
        return false;
    }
	// --- Protected/Private Methods ----------------------------------------------------------------------------------
   
	// --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************