using System.Collections.Generic;

public static class CollectionExtensions
{
    public static T GetRandom<T>(this ICollection<T> collection, System.Random random)
    {
        int index = random.Next(collection.Count);

        foreach (var item in collection)
        {
            if (index-- == 0)
                return item;
        }

        throw new System.InvalidOperationException("Collection was empty");
    }
}
