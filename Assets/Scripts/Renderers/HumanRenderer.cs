using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HumanRenderer : MonoBehaviour {

    private GameObject humanPrefab;

    static public HumanRenderer Inst { get; private set; }


    // Replace int with whatever key type Game.inst.data.Humans uses
    private readonly Dictionary<int, HumanMono> humanMonos = new();
    public Dictionary<string, Sprite> humanSprites; // Assign in inspector or load at runtime

    void OnEnable() {
        humanPrefab = Resources.Load<GameObject>("Human/Human");
        var sprites = Resources.LoadAll<Sprite>("Human/Sprites");
        humanSprites = new Dictionary<string, Sprite>();
        foreach (var sprite in sprites) {
            Debug.Log($"Loaded sprite: {sprite.name}");
            humanSprites[sprite.name] = sprite;
        }
        HumanRenderer.Inst = this;
    }

    private void Update() {
        if (Game.Inst == null || Game.Inst.data == null || Game.Inst.data.livingData.Entities == null)
            return;

        SyncHumans();
    }

    private void SyncHumans() {
        var humans = LivingData.Inst().Entities; // Assuming only humans for now, TODO - FIX later

        // Add/update/remove based on position validity
        foreach (var pair in humans) {
            int id = pair.Key;
            var human = pair.Value;

            // 🚫 No position → ensure renderer is removed
            if (Position.Get(id) == null) {
                if (humanMonos.TryGetValue(id, out var existing) && existing != null) {
                    Destroy(existing.gameObject);
                }

                humanMonos.Remove(id);
                continue;
            }

            float2 pos = Position.Get(id).Value.coords;

            // ✅ Create if missing (or destroyed)
            if (!humanMonos.TryGetValue(id, out var mono) || mono == null) {
                mono = CreateMonoObject(id);
                humanMonos[id] = mono;
            }

            // ✅ Update position
            UpdateRendererPosition(mono, pos);
            UpdateMono(mono, human);
        }

        // Remove deleted humans
        List<int> toRemove = null;

        foreach (var pair in humanMonos) {
            if (!humans.ContainsKey(pair.Key)) {
                toRemove ??= new List<int>();
                toRemove.Add(pair.Key);
            }
        }

        if (toRemove != null) {
            foreach (var id in toRemove) {
                if (humanMonos.TryGetValue(id, out var sr) && sr != null) {
                    Destroy(sr.gameObject);
                }

                humanMonos.Remove(id);
            }
        }
    }

    private HumanMono CreateMonoObject(int id) // change type if needed
    {
        var go = GameObject.Instantiate(humanPrefab);

        go.name = $"Human_{id}";
        go.transform.SetParent(transform, false);
        go.transform.localRotation = Quaternion.Euler(90, 0, 0);
        var humanMono = go.GetComponent<HumanMono>();
        return humanMono;
    }

    private void UpdateRendererPosition(HumanMono sr, float2 pos) {
        sr.transform.position = new Vector3(
            pos.x,
            0f,
            pos.y
        );
    }

    private void UpdateMono(HumanMono mono, Entity human) {
        mono.UpdateVisuals(human.Id);
    }
}