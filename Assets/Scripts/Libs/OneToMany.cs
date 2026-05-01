using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class OneToMany {
    // element -> owner

    [JsonProperty]
    private Dictionary<int, int> ownerMap = new();

    [JsonIgnore]
    public IReadOnlyDictionary<int, int> GetOwnerMap => ownerMap;

    // owner -> elements
    [JsonProperty]
    private Dictionary<int, List<int>> elementMap = new();

    [JsonIgnore]
    public IReadOnlyDictionary<int, List<int>> GetElementMap => elementMap;

    public void SetOwnerOf(int elementId, int? newOwnerId) {
        // Remove old relationship
        if (ownerMap.TryGetValue(elementId, out var oldOwnerId)) {
            if (elementMap.TryGetValue(oldOwnerId, out var oldList)) {
                oldList.Remove(elementId);

                if (oldList.Count == 0)
                    elementMap.Remove(oldOwnerId);
            }

            ownerMap.Remove(elementId);
        }

        // Add new relationship
        if (newOwnerId.HasValue) {
            int ownerId = newOwnerId.Value;

            ownerMap[elementId] = ownerId;

            if (!elementMap.TryGetValue(ownerId, out var list)) {
                list = new List<int>();
                elementMap[ownerId] = list;
            }

            if (!list.Contains(elementId))
                list.Add(elementId);
        }
    }

    public void RemoveOwner(int elementId) {
        SetOwnerOf(elementId, null);
    }

    public bool TryGetOwner(int elementId, out int ownerId) {
        return ownerMap.TryGetValue(elementId, out ownerId);
    }

    public IReadOnlyList<int> GetElements(int ownerId) {
        if (elementMap.TryGetValue(ownerId, out var list))
            return list;

        return Array.Empty<int>();
    }

    public bool HasOwner(int elementId) {
        return ownerMap.ContainsKey(elementId);
    }

    public bool HasElements(int ownerId) {
        return elementMap.ContainsKey(ownerId);
    }

    public void Clear() {
        ownerMap.Clear();
        elementMap.Clear();
    }
}