using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// starts the animation of the attached object at a random time between 0 and 1s. 
/// Used to offset the lavas looping animation so it is different to its neighbours.
/// </summary>
public class RandomAnimationDelay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Animator>().Play(0, -1, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
