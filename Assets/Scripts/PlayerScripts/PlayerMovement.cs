using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float acceleration = 0.6f;
    [SerializeField] public LayerMask groundLayer;

    Rigidbody2D RB;
    PlayerHandler playerHandler;
    float maxSpeed;
    float jumpForce;
    float targetVelocityX = 0f;
    bool isGrounded = false;

    public void SetupVars()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
        playerHandler = gameObject.GetComponent<PlayerHandler>();

        maxSpeed = playerHandler.playerState.roundStats.moveSpeed;
        jumpForce = playerHandler.playerState.roundStats.jumpForce;
        Debug.Log(jumpForce);
    }

    void FixedUpdate()
    {
        CheckGrounded();

        // Accelerate towards target velocity
        if (targetVelocityX != 0)
        {
            if (Math.Abs(RB.velocity.x) < Math.Abs(targetVelocityX) ||
                Math.Sign(targetVelocityX) != Math.Sign(RB.velocity.x))
                RB.AddForceX(Math.Sign(targetVelocityX) * acceleration, ForceMode2D.Impulse);


            // Clamping to max speed
            if (Math.Abs(RB.velocity.x) > Math.Abs(targetVelocityX))
            {
                RB.velocityX = targetVelocityX;
            }
        }
    }

    // Checks if the player is grounded
    void CheckGrounded()
    {
        RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, 1.1f, groundLayer);
        if (groundHit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //Debug.Log("Is Grounded: " + isGrounded);
    }


    public void Move(InputAction.CallbackContext c)
    {
        targetVelocityX = c.ReadValue<Vector2>().x * maxSpeed;
    }

    public void Jump(InputAction.CallbackContext c)
    {
        if (isGrounded)
        {
            if (c.performed)
            {
                RB.AddForceY(jumpForce, ForceMode2D.Impulse);
            }
        }
        if (c.canceled && RB.velocity.y > 0)
        {
            RB.velocityY = 0;
        }
    }
}
