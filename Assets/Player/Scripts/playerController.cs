using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private float speedConstant;
    private Rigidbody2D playerBody;
    private Animator anim;
    private bool playerGrounded = true;

    [SerializeField] private float playerSizeConstant;

    // Start is called before the first frame update
    void Start()
    {
        // Grab references for rigidbody (player object) and animator from respective objects.
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
        
    
    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerBody.velocity = new Vector2(speedConstant * horizontalInput, playerBody.velocity.y);

        
        // Flips player when moving left and right to direction of movement.
        if(horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(playerSizeConstant, playerSizeConstant, 1);
        } 
        else if(horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-playerSizeConstant, playerSizeConstant, 1);
        }

        if(Input.GetKey(KeyCode.Space) && playerGrounded)
        {
            Jump();
        }

        // Set animator parameters
        anim.SetBool("running", horizontalInput != 0);
        anim.SetBool("grounded", playerGrounded);
    }
    
    private void Jump()
    {
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y + speedConstant);
        anim.SetTrigger("jump");
        playerGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) playerGrounded = true;
    }

}
