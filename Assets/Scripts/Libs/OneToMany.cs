using System;
using System.Collections.Generic;

public class OneToMany {
    // element -> owner
    private Dictionary<long, long> ownerMap = new();

    // owner -> elements
    private Dictionary<long, List<long>> elementMap = new();

    public void SetOwnerOf(long elementId, long? newOwnerId) {
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
            long ownerId = newOwnerId.Value;

            ownerMap[elementId] = ownerId;

            if (!elementMap.TryGetValue(ownerId, out var list)) {
                list = new List<long>();
                elementMap[ownerId] = list;
            }

            if (!list.Contains(elementId))
                list.Add(elementId);
        }
    }

    public void RemoveOwner(long elementId) {
        SetOwnerOf(elementId, null);
    }

    public bool TryGetOwner(long elementId, out long ownerId) {
        return ownerMap.TryGetValue(elementId, out ownerId);
    }

    public IReadOnlyList<long> GetElements(long ownerId) {
        if (elementMap.TryGetValue(ownerId, out var list))
            return list;

        return Array.Empty<long>();
    }

    public bool HasOwner(long elementId) {
        return ownerMap.ContainsKey(elementId);
    }

    public bool HasElements(long ownerId) {
        return elementMap.ContainsKey(ownerId);
    }

    public void Clear() {
        ownerMap.Clear();
        elementMap.Clear();
    }
}