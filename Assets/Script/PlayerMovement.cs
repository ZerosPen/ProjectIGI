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


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = movementInput * speedMovement;
    }

    public void onMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log(movementInput);
    }
}
