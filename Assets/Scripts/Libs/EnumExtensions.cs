using System;

public static class EnumExtensions {
    public static T GetRandomValue<T>(this System.Random random) where T : Enum {
        var values = (T[])Enum.GetValues(typeof(T));
        return values[random.Next(values.Length)];
    }
}