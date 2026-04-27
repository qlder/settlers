using System;
using System.Collections.Generic;

public static class CollectionExtensions {
    public static T GetRandom<T>(this ICollection<T> collection) {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (collection.Count == 0)
            throw new InvalidOperationException("Collection was empty");

        int index = Rng.Inst().Int(0, collection.Count);

        foreach (var item in collection) {
            if (index-- == 0)
                return item;
        }

        throw new InvalidOperationException("Collection was empty");
    }
}