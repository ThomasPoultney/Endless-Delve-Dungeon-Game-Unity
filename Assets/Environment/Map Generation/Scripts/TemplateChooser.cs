using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateChooser : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] roomTemplates;
    void Start()
    {
        int rand = Random.Range(0, roomTemplates.Length);
        GameObject instance = Instantiate(roomTemplates[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
