using System;
using System.Collections.Generic;

[System.Serializable]
public class Rng {

    public bool stable;
    public uint state;

    public static Rng Inst() {
        if (Game.Inst == null) {
            return new Rng(0, false);
        } else {
            return new Rng(Game.Inst.data.rngState, true);
        }
    }

    public Rng(uint seed, bool stable = true) {
        state = seed != 0 ? seed : 1u;
        this.stable = stable;
    }

    private uint NextUInt() {
        state = state * 1664525u + 1013904223u;
        if (stable) {
            Game.Inst.data.rngState = state;
        }
        return state;
    }

    public int Int(int min, int max) {
        if (max <= min)
            throw new System.ArgumentException("max must be greater than min");

        return min + (int)(NextUInt() % (uint)(max - min));
    }

    public float Float(float min, float max) {
        if (max <= min)
            throw new System.ArgumentException("max must be greater than min");

        return min + (NextUInt() / (float)uint.MaxValue) * (max - min);
    }

    public float Float01() {
        return this.Float(0f, 1f);
    }

    public T Item<T>(ICollection<T> collection) {
        int index = Int(0, collection.Count);

        foreach (var item in collection) {
            if (index-- == 0)
                return item;
        }

        throw new System.InvalidOperationException("Collection was empty");
    }

    public T EnumValue<T>() where T : Enum {
        var values = (T[])Enum.GetValues(typeof(T));
        return values[Int(0, values.Length)];
    }


}