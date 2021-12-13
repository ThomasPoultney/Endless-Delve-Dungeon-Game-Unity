using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
