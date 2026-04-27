using UnityEngine;
using Newtonsoft.Json;

public struct Tint {

    private float r;
    private float g;
    private float b;

    public float R {
        get => r;
        set => r = Mathf.Clamp01(value);
    }

    public float G {
        get => g;
        set => g = Mathf.Clamp01(value);
    }

    public float B {
        get => b;
        set => b = Mathf.Clamp01(value);
    }

    #region Helpers
    public Color ToColor() {
        return new Color(R, G, B);
    }
    #endregion
}