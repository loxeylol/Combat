using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyableLevelObject : LevelObject
{

    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private float _explosionForce;
    private DestroyableLevelObject _prefab;

    // --- Properties -------------------------------------------------------------------------------------------------
    public override bool ExplodeShootable => false;
    public override bool ReflectShootable => false;

    public override FactoryTypes ObjectType => FactoryTypes.DestructibleCube;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {

        _prefab = MonoFactory.GetFactoryPrefab(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Shootable bullet = other.gameObject.GetComponent<Shootable>();
        if (bullet != null)
        {
            Explode();
            this.DoAfterSeconds(2f, false, ReturnToFactory);
            
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public override void ReturnToFactory()
    {
        Debug.Log($"{this.GetType().Name} returning to factory!");
        MonoFactory.ReturnFactoryObject(this);

        for (int i = 0; i < _prefab.transform.childCount; i++)
        {
            this.transform.GetChild(i).localPosition = _prefab.transform.GetChild(i).localPosition;
            this.transform.GetChild(i).localRotation = _prefab.transform.GetChild(i).localRotation;
        }
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void Explode()
    {
        foreach(Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb.gameObject == this.gameObject)
                continue;

            rb.AddForce(rb.transform.localPosition.normalized * _explosionForce, ForceMode.Impulse);
        }
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************