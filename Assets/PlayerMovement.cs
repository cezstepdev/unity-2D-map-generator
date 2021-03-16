using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerMovmentSpeed = 5.0f;
    public float playerJumpHeight = 5.0f;
    public float playerMaxJumpCount = 1.0f;
    public float playerJumpCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && playerJumpCount < playerMaxJumpCount) 
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, playerJumpHeight), ForceMode2D.Impulse);
            playerJumpCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Vector3 playerMovment = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        transform.position += playerMovment * Time.deltaTime * playerMovmentSpeed;
    }
}
