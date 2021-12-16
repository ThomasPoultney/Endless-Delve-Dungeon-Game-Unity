using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityDemonstration : MonoBehaviour
{
    // Start is called before the first frame update


    public float insanityLoss = 0.01f;
    public float minRadius = 0.10f;
    public float startOuterRadius = 9;
    public float startInnerRadius = 4;
    public float insanityLossIncrement = 0.2f;
    UnityEngine.Rendering.Universal.Light2D light = null;

    public InsanityBar insanityBar;
    void Start()
    {

        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        resetInsanityLight();
    }

    public void resetInsanityLight()
    {
        light.pointLightOuterRadius = startOuterRadius;
        light.pointLightInnerRadius = startInnerRadius;
        insanityBar.SetMaxInsanity(light.pointLightOuterRadius);
    }

    float elapsed = 0f;
    void Update()
    {
        insanityBar.SetInsanity(light.pointLightOuterRadius);
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

        insanityBar.SetInsanity(light.pointLightOuterRadius);
    }
}

 