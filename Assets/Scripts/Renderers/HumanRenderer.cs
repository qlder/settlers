using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HumanRenderer : MonoBehaviour {
    [Header("Rendering")]
    [SerializeField] private Sprite humanSprite;
    [SerializeField] private Vector2 spriteOffset = Vector2.zero;
    [SerializeField] private string sortingLayerName = "Default";
    [SerializeField] private int sortingOrder = 0;

    // Replace int with whatever key type Game.inst.data.Humans uses
    private readonly Dictionary<long, SpriteRenderer> _renderers = new();

    private void Update() {
        if (Game.Inst == null || Game.Inst.data == null || Game.Inst.data.humanData.Humans == null)
            return;

        SyncHumans();
    }

    private void SyncHumans() {
        var humans = Game.Inst.data.humanData.Humans;

        // Add/update/remove based on position validity
        foreach (var pair in humans) {
            long id = pair.Key;
            var human = pair.Value;

            // 🚫 No position → ensure renderer is removed
            if (!human.position.HasValue) {
                Debug.Log($"Removing renderer for Human {id} due to missing position: {human.position}");
                if (_renderers.TryGetValue(id, out var existing) && existing != null) {
                    Destroy(existing.gameObject);
                }

                _renderers.Remove(id);
                continue;
            }

            float2 pos = human.position.Value;

            // ✅ Create if missing (or destroyed)
            if (!_renderers.TryGetValue(id, out var sr) || sr == null) {
                Debug.Log($"Creating renderer for Human {id} at position {pos}");
                sr = CreateRendererObject(id);
                _renderers[id] = sr;
            }

            // ✅ Update position
            UpdateRendererPosition(sr, pos);
        }

        // Remove deleted humans
        List<long> toRemove = null;

        foreach (var pair in _renderers) {
            if (!humans.ContainsKey(pair.Key)) {
                toRemove ??= new List<long>();
                toRemove.Add(pair.Key);
            }
        }

        if (toRemove != null) {
            foreach (var id in toRemove) {
                if (_renderers.TryGetValue(id, out var sr) && sr != null) {
                    Destroy(sr.gameObject);
                }

                _renderers.Remove(id);
            }
        }
    }

    private SpriteRenderer CreateRendererObject(long id) // change type if needed
    {
        var go = new GameObject($"Human_{id}");
        go.transform.SetParent(transform, false);
        go.transform.localRotation = Quaternion.Euler(90, 0, 0);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = humanSprite;
        sr.sortingLayerName = sortingLayerName;
        sr.sortingOrder = sortingOrder;

        return sr;
    }

    private void UpdateRendererPosition(SpriteRenderer sr, float2 pos) {
        sr.transform.position = new Vector3(
            pos.x + spriteOffset.x,
            0f,
            pos.y + spriteOffset.y
        );
    }
}