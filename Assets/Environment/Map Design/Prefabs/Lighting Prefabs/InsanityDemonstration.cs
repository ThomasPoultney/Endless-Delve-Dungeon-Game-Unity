using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityDemonstration : MonoBehaviour
{
    // Start is called before the first frame update


    public float insanityLoss = 0.05f;
    public float minRadius = 0.10f;

    public float insanityLossIncrement = 1f;
    UnityEngine.Rendering.Universal.Light2D light = null;
    void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

    }

    float elapsed = 0f;
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= insanityLossIncrement)
        {
            elapsed = elapsed % insanityLossIncrement;
            InsanityLoss();
        }
    }


    // Update is called once per frame
    private void InsanityLoss()
    {

       if (light.pointLightInnerRadius > minRadius)
        {
            light.pointLightInnerRadius = light.pointLightInnerRadius - insanityLoss;
        }
       if (light.pointLightOuterRadius > minRadius)
        {
            light.pointLightOuterRadius = light.pointLightOuterRadius - insanityLoss;

        }
    }
}

 