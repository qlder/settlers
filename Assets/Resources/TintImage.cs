using UnityEngine;
using UnityEngine.UI;

public class TintImage : MonoBehaviour
{

    [SerializeField]
    private Image image;
    [SerializeField]
    private Color tint = Color.red;

    void Start()
    {

    }

    void Update()
    {
        float r = tint.r;
        float g = tint.g;
        float b = tint.b;

        r += Random.Range(-1f, 1f) * Time.deltaTime;
        g += Random.Range(-1f, 1f) * Time.deltaTime;
        b += Random.Range(-1f, 1f) * Time.deltaTime;

        tint.r = Mathf.Clamp01(r);
        tint.g = Mathf.Clamp01(g);
        tint.b = Mathf.Clamp01(b);

        image.color = tint;
    }
}