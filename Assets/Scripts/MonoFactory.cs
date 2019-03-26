using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MonoFactory : MonoBehaviour
{
    public static MonoFactory Instance { get; private set; }

    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------
    [Serializable]
    public class FactoryType
    {
        public MonoBehaviour prefab;
        [Range(0, 99)] public int startAmount = 10;
        public bool allowGrowth = true;
        [HideInInspector] public Type type;
        [HideInInspector] public Queue<IFactoryObject> queue;

        public int counter = 0;

        public void Populate()
        {
            type = prefab.GetType();
            queue = new Queue<IFactoryObject>();

            for (int i = 0; i < startAmount; i++)
            {
                Add();
            }
        }

        public void Add()
        {
            queue.Enqueue(Instance.CreateFactoryObject(prefab, ++counter));
        }
    }

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private List<FactoryType> _factories;


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

        PopulateFactory();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (FactoryType ft in _factories)
        {
            if (ft.prefab is IFactoryObject == false)
            {
                ft.prefab = null;
            }
        }
    }
#endif

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public static T GetFactoryObject<T>() where T : MonoBehaviour, IFactoryObject
    {
        return Instance._GetFactoryObject<T>();
    }

    public static void ReturnFactoryObject<T>(T obj) where T : MonoBehaviour, IFactoryObject
    {
        Instance._ReturnFactoryObject<T>(obj);
    }
    public static void ReturnAllChildren()
    {
        Instance._ReturnAllChildren();
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void _ReturnAllChildren()
    {
        foreach (IFactoryObject returnable in GetComponentsInChildren<IFactoryObject>(false))
        {
            returnable.ReturnToFactory();
        }
    }
    private void PopulateFactory()
    {
        foreach (FactoryType ft in _factories)
        {
            if (ft.prefab is IFactoryObject == false)
            {
                Debug.LogError($"Prefab {ft.prefab.name} of Type {ft.prefab.GetType().Name} is no IFactoryObject!");
                continue;
            }

            ft.Populate();
        }
    }


    private IFactoryObject CreateFactoryObject(MonoBehaviour prefab, int counter = 1)
    {
        MonoBehaviour obj = Instantiate(prefab, this.transform, true);
        obj.name = $"{prefab.name}_{counter.ToString("D3")}";
        obj.transform.position = Vector3.one * 10;
        obj.gameObject.SetActive(false);

        return obj as IFactoryObject;
    }

    // --------------------------------------------------------------------------------------------
    private void _ReturnFactoryObject<T>(T obj) where T : MonoBehaviour, IFactoryObject
    {

        FactoryType ft = _factories.FirstOrDefault(f => f.type == typeof(T));
        if (ft == null)
        {
            Debug.LogWarning($"Type {typeof(T).Name} does not exists in the factory.");

        }

        obj.gameObject.SetActive(false);
        obj.transform.position = Vector3.one * 10;
        ft.queue.Enqueue(obj);
    }
    private T _GetFactoryObject<T>() where T : MonoBehaviour, IFactoryObject
    {
        FactoryType ft = _factories.FirstOrDefault(f => f.type == typeof(T));
        if (ft == null)
        {
            Debug.LogWarning($"Type {typeof(T).Name} does not exists in the factory.");
            return null;
        }

        if (ft.queue.Count == 0)
        {
            if (ft.allowGrowth == false)
                return null;

            ft.Add();
        }

        T obj = ft.queue.Dequeue() as T;
        obj.gameObject.SetActive(true);
        return obj;
    }



    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************