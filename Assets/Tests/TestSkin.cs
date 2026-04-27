using UnityEngine;

public class TestSkin : MonoBehaviour
{
    [Range(0.04f, 0.08f)] public float hue = 0.04f;        // narrow range
    [Range(0.4f, 0.6f)] public float saturation = 0.4f;    // moderate
    [Range(0.2f, 0.9f)] public float value = 0.6f;         // main driver

    void Update() {
        SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
        sr.color = this.GetHairColor();
    }

    public Color GetHairColor()
    {
        return Color.HSVToRGB(hue, saturation, value);
    }
}
