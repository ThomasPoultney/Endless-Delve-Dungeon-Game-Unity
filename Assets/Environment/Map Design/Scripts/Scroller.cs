using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private float m_Speed = -1.5f;
    [SerializeField]
    private bool stopScrolling;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stopScrolling == true)
        {
            rigidBody.velocity = Vector2.zero;
        } else
        {
            rigidBody.velocity = new Vector2(0, m_Speed);
        }
    } 
}
