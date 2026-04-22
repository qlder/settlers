using System.Collections.Generic;
using UnityEngine;

public class TerrainRenderer : MonoBehaviour {

    private GameObject landPrefab;
    // private GameObject waterPrefab;
    // private Terrain[,] terrains;
    private Dictionary<string, SpriteRenderer> terrainObjects = new();

    // private const float TERRAIN_HEIGHT = 128f;

    void OnEnable() {
        landPrefab = Resources.Load<GameObject>("Terrain/Land");
        // waterPrefab = Resources.Load<GameObject>("Terrain/Water");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Initialize();
    }

    void Initialize() {
        MapData mapData = Game.Inst.data.mapData;
        CreateTerrains(mapData);

    }


    void CreateTerrains(MapData mapData) {
        foreach (var tile in mapData.tiles.Values) {
            GameObject terrainObj = GameObject.Instantiate(landPrefab);
            terrainObj.transform.position = new Vector3(tile.position.x + 0.5f, 0, tile.position.y + 0.5f);
            terrainObj.transform.parent = transform;
            terrainObj.name = $"Terrain_{tile.position.x}_{tile.position.y}";
            terrainObjects[terrainObj.name] = terrainObj.GetComponent<SpriteRenderer>();
            Color tint = Color.white;
            if (tile.groundType == GroundType.Water) {
                tint = Color.blue;
            } else {
                tint = Color.green;
            }
            float r = tint.r;
            float g = tint.g;
            float b = tint.b;
            r += Random.Range(-0.15f, 0.15f);
            g += Random.Range(-0.15f, 0.15f);
            b += Random.Range(-0.15f, 0.15f);
            r = Mathf.Clamp01(r);
            g = Mathf.Clamp01(g);
            b = Mathf.Clamp01(b);
            terrainObjects[terrainObj.name].color = new Color(r, g, b);
        }
    }
}
