using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Player
{
    [Header("Stats Movement")]
    public Vector2 movementInput;
    private Rigidbody2D rb;
    public float speedMovement;
    private float directionX;
    private float directionY;

    [Header("refereance")]
    private PlayerAnimator playerAnimator;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        rb.velocity = movementInput.normalized * speedMovement;

        playerAnimator.directionX = directionX;
        playerAnimator.directionY = directionY;

        playerAnimator.AnimationOnWalk(FlashLightOn);
    }

    public void onMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        directionX = movementInput.x;
        directionY = movementInput.y;
    }
}
