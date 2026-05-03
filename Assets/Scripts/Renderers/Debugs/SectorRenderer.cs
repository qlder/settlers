using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Mathematics;
using TMPro;

public class SectorRenderer : MonoBehaviour {

    private GameObject debugPrefab;
    private Dictionary<string, GameObject> debugObjects;

    void OnEnable() {
        debugPrefab = Resources.Load<GameObject>("Debug/DebugText");
        debugObjects = new Dictionary<string, GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Initialize();
    }

    void Initialize() {
        foreach (Tile tile in MapData.Inst().tiles.Values) {
            InstantiateDebug(tile);
        }
    }

    void InstantiateDebug(Tile tile) {
        // Vector3 position = new Vector3(tile.Center.x, 0.5f, tile.Center.y);
        // GameObject debugObject = GameObject.Instantiate(debugPrefab, position, Quaternion.identity, transform);
        // debugObject.name = $"Tile_{tile.key}";
        // debugObject.transform.localScale = Vector3.one * 0.1f;
        // debugObject.transform.rotation = Quaternion.Euler(90, 0, 0); // Rotate to face up
        // debugObjects.Add(tile.key, debugObject);
    }

    void Update() {
        // foreach (var pair in debugObjects) {
        //     string tileKey = pair.Key;
        //     GameObject debugObject = pair.Value;


        //     if (MapData.Inst().tiles.TryGetValue(tileKey, out Tile tile)) {
        //         Rng rng = new Rng((uint)tile.sectorId.GetHashCode(), false);
        //         Color color = new Color(rng.Float01(), rng.Float01(), rng.Float01());



        //         string text = $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{tile.key}</color>\n";

        //         if (MapData.Inst().sectors.TryGetValue(tile.sectorId, out Sector sector)) {
        //             Rng rng2 = new Rng((uint)sector.connectivityId.GetHashCode(), false);
        //             Color color2 = new Color(rng2.Float01(), rng2.Float01(), rng2.Float01());
        //             text += $"<color=#{ColorUtility.ToHtmlStringRGBA(color2)}>{sector.connectivityId}</color>";
        //         }

        //         debugObject.GetComponent<TextMeshPro>().text = text;
        //     }

        //     // // Update color based on sector passability
        //     // if (MapData.Inst().sectors.TryGetValue(tile.sectorId, out Sector sector)) {
        //     //     Color color = sector.passability == SectorPassability.Walkable ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        //     //     debugObject.GetComponent<TextMeshPro>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{tile.key}</color>\n{tile.sectorId}";
        //     // } else {
        //     //     debugObject.GetComponent<TextMeshPro>().text = $"XXX";
        //     // }
        // }
    }



}
