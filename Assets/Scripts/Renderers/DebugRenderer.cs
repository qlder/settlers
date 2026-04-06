using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugRenderer : MonoBehaviour {

    private GameObject debugPrefab;
    private Dictionary<Tile, GameObject> debugObjects;

    void OnEnable() {
        debugPrefab = Resources.Load<GameObject>("Debug/Debug");
        debugObjects = new Dictionary<Tile, GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Initialize();
    }

    void Initialize() {
        foreach (Tile tile in Map.Inst().tiles) {
            if (tile.isWalkable == false) {
                InstantiateDebug(tile);
            }
        }
    }

    void InstantiateDebug(Tile tile) {
        Vector3 position = tile.GetPosition().GetVector3();
        GameObject debugObject = GameObject.Instantiate(debugPrefab, position, Quaternion.identity, transform);
        debugObject.name = $"Tile_{tile.X}_{tile.Z}";
        debugObjects.Add(tile, debugObject);
    }



}
