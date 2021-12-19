using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Script which can be attached to a 2D light to simulate the effect of a flickering torch.
/// </summary>
public class FlickerTorch : MonoBehaviour
{
    ///The minimum intensity of the tourch.
    public float minIntensity = 0.25f;
    ///The maximum intensity of the tourch.
    public float maxIntensity = 0.5f;
    ///The light to flicker
    UnityEngine.Rendering.Universal.Light2D light = null;
    
    float random;

    void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        random = Random.Range(0.0f, 65535.0f);
    }

    void Update()
    {
        flickerLight();
    }


    /// <summary>
    /// Flickers the light between the minimum and maximum intensity at random durations 
    /// to simulate the effect of a flickering torch
    /// </summary>
    private void flickerLight()
    {
        float noise = Mathf.PerlinNoise(random, Time.time);

        light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }


}
