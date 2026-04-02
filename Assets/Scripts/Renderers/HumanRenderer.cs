using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HumanRenderer : MonoBehaviour {

    private GameObject humanPrefab;
    private Dictionary<Human, GameObject> humanObjects;

    void OnEnable() {
        humanPrefab = Resources.Load<GameObject>("Human/Human");
        humanObjects = new Dictionary<Human, GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Initialize();
    }

    void Initialize() {
        foreach (Human human in Game.Inst.humans) {
            InstantiateHuman(human);
        }
    }

    void InstantiateHuman(Human human) {
        if (human.currentTile == null) {
            Debug.LogError($"Human {human.name} has no tile assigned.");
            return;
        }
        Tile tile = human.currentTile;
        Vector3 position = tile.GetPosition();
        GameObject humanObject = GameObject.Instantiate(humanPrefab, position, Quaternion.identity, transform);
        humanObject.name = human.name;
        humanObjects.Add(human, humanObject);
    }

    void Update() {
        foreach (Human human in Game.Inst.humans) {
            if (humanObjects.ContainsKey(human)) {
                GameObject humanObject = humanObjects[human];
                humanObject.transform.position = human.currentTile.GetPosition();
            }
        }
    }

    void OnDrawGizmos() {

        GUIStyle style = new GUIStyle();
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        foreach (Human human in Game.Inst.humans) {
            if (humanObjects.ContainsKey(human)) {
                GameObject humanObject = humanObjects[human];
                string debugText = "";
                if (human.currentJob == null) {
                    debugText = "🔴 No Job";
                }
                else {
                    debugText = $"🟢 {human.currentJob.GetType().Name} : {human.currentJob.actionName}";//: 
                }
                Vector3 worldPos = humanObject.transform.position + new Vector3(0, 2.5f, 0);
                Debug.Log($"Drawing label for {human.name}");
                Handles.Label(worldPos, debugText, style);
            }
        }
    }



}
