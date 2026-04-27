using UnityEngine;

public class TestHair : MonoBehaviour
{
    [Range(0.02f, 0.15f)] public float hue = 0.1f;        // 0–1 color wheel
    [Range(0.3f, 0.9f)] public float saturation = 0.5f; // dull ↔ vivid
    [Range(0.1f, 0.9f)] public float value = 0.3f;      // dark ↔ light

    void Update() {
        SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();
        sr.color = this.GetHairColor();
    }

    public Color GetHairColor()
    {
        return Color.HSVToRGB(hue, saturation, value);
    }
}
