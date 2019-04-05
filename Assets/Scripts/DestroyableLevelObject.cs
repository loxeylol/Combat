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

            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                if (rb.gameObject == this.gameObject)
                    continue;
                rb.isKinematic = false;
            }
            Explode();
            this.DoAfterSeconds(4f, false, ReturnToFactory);

        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public override void ReturnToFactory()
    {
        Debug.Log($"{this.GetType().Name} returning to factory!");
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb.gameObject == this.gameObject)
                continue;
            rb.isKinematic = true;
        }
        MonoFactory.ReturnFactoryObject(this);

        //for (int i = 0; i < _prefab.transform.childCount; i++)
        //{
        //    for (int j = 0; j < _prefab.transform.GetChild(i).childCount; j++)
        //    {
        //        for (int k = 0; k < _prefab.transform.GetChild(i).GetChild(j).childCount; k++)
        //        {
        //            transform.GetChild(i).GetChild(j).GetChild(k).localRotation = _prefab.transform.GetChild(i).GetChild(j).GetChild(k).localRotation;
        //            transform.GetChild(i).GetChild(j).GetChild(k).localPosition = _prefab.transform.GetChild(i).GetChild(j).GetChild(k).localPosition;
        //        }

        //    }
        //}

        BoxCollider[] prefabCols =_prefab.GetComponentsInChildren<BoxCollider>(true);
        BoxCollider[] thisCols = transform.GetComponentsInChildren<BoxCollider>(true);
        for (int i = 0; i < prefabCols.Length; i++)
        {
            thisCols[i].transform.localPosition = prefabCols[i].transform.localPosition;
            thisCols[i].transform.localRotation = prefabCols[i].transform.localRotation;
        }
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void Explode()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb.gameObject == this.gameObject)
                continue;
            rb.AddForce(rb.transform.localPosition.normalized * _explosionForce, ForceMode.Impulse);
        }
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************