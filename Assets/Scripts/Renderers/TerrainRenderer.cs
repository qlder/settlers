using UnityEngine;

public class TerrainRenderer : MonoBehaviour {

    private GameObject terrainPrefab;
    private Terrain[,] terrains;

    private const float TERRAIN_HEIGHT = 128f;

    void OnEnable() {
        terrainPrefab = Resources.Load<GameObject>("Terrain/Terrain");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Initialize();
    }

    void Initialize() {
        Map map = Map.Inst();
        CreateTerrains(map);
        StitchTerrains(map);
        for (int x = 0; x < map.terrainsLength; x++) {
            for (int z = 0; z < map.terrainsLength; z++) {
                RenderTerrain(x, z);
            }
        }
    }


    void CreateTerrains(Map map) {
        terrains = new Terrain[map.terrainsLength, map.terrainsLength];
        for (int x = 0; x < map.terrainsLength; x++) {
            for (int z = 0; z < map.terrainsLength; z++) {
                Vector3 position = new Vector3(x * Map.TERRAIN_SIZE, 0, z * Map.TERRAIN_SIZE);
                GameObject terrainObject = GameObject.Instantiate(terrainPrefab, position, Quaternion.identity, transform);
                terrainObject.name = $"Terrain_{x}_{z}";

                Terrain terrain = terrainObject.GetComponent<Terrain>();

                // Clone TerrainData so each terrain tile can be edited independently.
                TerrainData terrainData = Instantiate(terrain.terrainData);
                terrainData.name = $"{terrainObject.name}_Data";
                terrain.terrainData = terrainData;

                TerrainCollider terrainCollider = terrainObject.GetComponent<TerrainCollider>();
                if (terrainCollider != null) {
                    terrainCollider.terrainData = terrainData;
                }

                terrains[x, z] = terrain;
            }
        }
    }

    void StitchTerrains(Map map) {
        for (int x = 0; x < map.terrainsLength; x++) {
            for (int z = 0; z < map.terrainsLength; z++) {
                Terrain left = x > 0 ? terrains[x - 1, z] : null;
                Terrain right = x < map.terrainsLength - 1 ? terrains[x + 1, z] : null;
                Terrain bottom = z > 0 ? terrains[x, z - 1] : null;
                Terrain top = z < map.terrainsLength - 1 ? terrains[x, z + 1] : null;
                terrains[x, z].SetNeighbors(left, top, right, bottom);
                // Debug.Log($"Set neighbors for Terrain_{x}_{z}: Left={left?.name}, Top={top?.name}, Right={right?.name}, Bottom={bottom?.name}");
            }
        }
    }



    void RenderTerrain(int x, int z) {
        Terrain terrain = terrains[x, z];
        TerrainData terrainData = terrain.terrainData;

        int terrainSize = Map.TERRAIN_SIZE + 1;
        float[,] heights = new float[terrainSize, terrainSize];
        // opposite order of x and z because Unity's terrain data uses [z, x] indexing

        for (int i = 0; i < terrainSize; i++) {
            for (int j = 0; j < terrainSize; j++) {
                TileCorner corner = TileCorner.Get(x * Map.TERRAIN_SIZE + i, z * Map.TERRAIN_SIZE + j);
                heights[j, i] = (float)corner.H / (float)TERRAIN_HEIGHT;
            }
        }
        terrainData.SetHeights(0, 0, heights);
    }
}
