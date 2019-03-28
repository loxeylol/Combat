using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------
    [Serializable]
    public class CubeLevel
    {
        public List<Vector2> _cubePositions;
    }

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private int _levelIndex = 0;
    [SerializeField] private CubeLevel[] _cubeLevels;
    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Start()
    {
        if (_levelIndex >= 0 && _levelIndex <= _cubeLevels.Length)
        {
            LoadCubeLevel(_cubeLevels[_levelIndex]);
            //StartCoroutine(BuildLevelAgainRoutine());
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_cubeLevels != null)
            _levelIndex = Mathf.Clamp(_levelIndex, 0, _cubeLevels.Length - 1);
    }
#endif

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private IEnumerator BuildLevelAgainRoutine()
    {
        yield return new WaitForSeconds(5f);
        LoadCubeLevel(_cubeLevels[_levelIndex]);
    }
    private void LoadCubeLevel(CubeLevel level)
    {
        foreach (Vector2 pos in level._cubePositions)
        {
            DestroyableLevelObject _cube = MonoFactory.GetFactoryObject<DestroyableLevelObject>(FactoryTypes.DestructibleCube) as DestroyableLevelObject;
            _cube.transform.position = new Vector3(pos.x, 0.5f, pos.y);
            Debug.Log("Building Level!   " + _cube.name + "cube position" + _cube.transform.position);
        }

    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************