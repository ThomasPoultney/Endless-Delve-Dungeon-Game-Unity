using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// destorys the object the script is attached too when the script is run
/// </summary>
public class DeleteMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject);
    }
}
