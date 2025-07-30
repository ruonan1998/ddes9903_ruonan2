using UnityEngine;

public class EmissionPulse : MonoBehaviour
{
    public Material targetMaterial;  // 拖入画的材质
    public Color emissionColor = Color.white;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1f;

    private void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        targetMaterial.SetColor("_EmissionColor", emissionColor * intensity);
    }
}