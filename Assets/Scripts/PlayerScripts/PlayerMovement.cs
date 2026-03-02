using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 0.8f;
    public LayerMask groundLayer;
    public Vector2 groundCheckOffset;
    public Vector2 groundCheckSize;

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
    }

    void FixedUpdate()
    {
        CheckGrounded();

        // Accelerate towards target velocity
        if (targetVelocityX != 0)
        {

            // Keeping correct speed
            // (This causes fulctuations in speed when input is less than max but
            //  provides a smoother feel when slowing down with controler)
            if (!(Math.Abs(RB.velocity.x) > Math.Abs(targetVelocityX)))
            {
                if (Math.Abs(RB.velocity.x) < Math.Abs(targetVelocityX) ||
                    Math.Sign(targetVelocityX) != Math.Sign(RB.velocity.x))
                    RB.AddForceX(Math.Sign(targetVelocityX) * acceleration, ForceMode2D.Impulse);    
            }

            // Clamping to max speed
            if (Math.Abs(RB.velocity.x) > maxSpeed)
            {
                RB.velocityX = targetVelocityX;
            }

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckOffset, transform.localScale * groundCheckSize);
    }

    // Checks if the player is grounded
    void CheckGrounded()
    {
        Vector2 checkPostion = (Vector2)transform.position + groundCheckOffset;
        Vector2 checkSize = transform.localScale * groundCheckSize;

        RaycastHit2D groundHit = Physics2D.BoxCast(checkPostion, checkSize,
                                                   0f, Vector2.down, 0.1f, groundLayer);
        isGrounded = groundHit.collider != null;

    }


    public void Move(InputAction.CallbackContext c)
    {
        float inputX = c.ReadValue<Vector2>().x;
        if (Math.Abs(inputX) > 0.1)
        {
            targetVelocityX = c.ReadValue<Vector2>().x * maxSpeed;
        }
        else
        {
            targetVelocityX = 0;
        }
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
