using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerTorch : MonoBehaviour
{
    public float minIntensity = 0.25f;
    public float maxIntensity = 0.5f;
    UnityEngine.Rendering.Universal.Light2D light = null;
    float random;

    void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        random = Random.Range(0.0f, 65535.0f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(random, Time.time);

        light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }


}
