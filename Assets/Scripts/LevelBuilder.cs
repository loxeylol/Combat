using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    public static LevelBuilder Instance { get; private set; }
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------
    [Serializable]
    public class CubeLevel
    {
        public List<Vector2> _cubePositions;
        public FactoryTypes type;
    }

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private int _levelIndex;
    [SerializeField] private CubeLevel[] _cubeLevels;
    // --- Properties -------------------------------------------------------------------------------------------------
    public int LevelIndex
    {
        get { return _levelIndex; }
        set { _levelIndex = value; }
    }
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
    private void Start()
    {
       
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_cubeLevels != null)
            _levelIndex = Mathf.Clamp(_levelIndex, 0, _cubeLevels.Length - 1);
    }
#endif

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    public void BuildLevel()
    {
        if (_levelIndex >= 0 && _levelIndex <= _cubeLevels.Length)
        {
            LoadDestructibleOrNormalLevel(_cubeLevels[_levelIndex], _cubeLevels[_levelIndex].type);
        }
    }

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private IEnumerator BuildLevelAgainRoutine()
    {
        yield return new WaitForSeconds(5f);
        //LoadDestructibleCubeLevel(_cubeLevels[_levelIndex]);
    }
    private void LoadCubeLevel<T>(CubeLevel level, FactoryTypes type) where T : LevelObject
    {
        Debug.Log("loading level" + _levelIndex);
        foreach (Vector2 pos in level._cubePositions)
        {
            LevelObject _cube = MonoFactory.GetFactoryObject<T>(type) as T;
            _cube.transform.position = new Vector3(pos.x, 0.5f, pos.y);
            Debug.Log("Building Level!   " + _cube.name + "cube position" + _cube.transform.position);
        }

    }
    private void LoadDestructibleOrNormalLevel(CubeLevel level, FactoryTypes factoryTypes)
    {
        switch (factoryTypes)
        {
            case FactoryTypes.Cube:
            default:
                LoadCubeLevel<LevelObject>(level, FactoryTypes.Cube);
                break;
            case FactoryTypes.DestructibleCube:
                LoadCubeLevel<DestroyableLevelObject>(level, FactoryTypes.DestructibleCube);
                break;
        }
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************