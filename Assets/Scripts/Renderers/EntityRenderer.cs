using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntityRenderer : MonoBehaviour {

    private GameObject entityPrefab;
    private Dictionary<Entity, GameObject> entityObjects;

    void OnEnable() {
        entityPrefab = Resources.Load<GameObject>("Human/Human");
        entityObjects = new Dictionary<Entity, GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Initialize();
    }

    void Initialize() {
        foreach (Entity entity in Game.Inst.entities) {
            InstantiateHuman(entity);
        }
    }

    void InstantiateHuman(Entity entity) {
        if (entity.GetTile() == null) {
            Debug.LogError($"Human {entity.name} has no tile assigned.");
            return;
        }
        Tile tile = entity.GetTile();
        Vector3 position = entity.currentPosition.GetVector3();
        GameObject humanObject = GameObject.Instantiate(entityPrefab, position, Quaternion.identity, transform);
        humanObject.name = entity.name;
        entityObjects.Add(entity, humanObject);
    }

    void Update() {
        foreach (Entity entity in Game.Inst.entities) {
            if (entityObjects.ContainsKey(entity)) {
                GameObject humanObject = entityObjects[entity];
                humanObject.transform.position = entity.currentPosition.GetVector3();
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        return;
        GUIStyle style = new GUIStyle();
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        foreach (Entity entity in Game.Inst.entities) {
            if (entityObjects != null && entityObjects.ContainsKey(entity)) {
                GameObject humanObject = entityObjects[entity];
                string debugText = "";
                if (entity.currentJob == null) {
                    debugText = "🔴 No Job";
                } else {
                    debugText = $"🟢 {entity.currentJob.GetType().Name} : {entity.currentJob.actionName}";//: 
                }
                Vector3 worldPos = humanObject.transform.position + new Vector3(0, 2.5f, 0);
                // Debug.Log($"Drawing label for {human.name}");
                Handles.Label(worldPos, debugText, style);
            }
        }
    }
#endif


}
