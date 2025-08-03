using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    public Transform player;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isDashing = false;
    private bool canDash = true;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

        if (canDash && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition((Vector2)transform.position + (dashDirection * dashSpeed * Time.fixedDeltaTime));
        }
        else
        {
            MoveEnemy(movement);
        }
    }

    void MoveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.fixedDeltaTime));
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        
        float perpendicularAngle = Random.value > 0.5f ? 90f : -90f;
        dashDirection = Quaternion.Euler(0, 0, perpendicularAngle) * movement;
        dashDirection.Normalize();

        yield return new WaitForSeconds(dashDuration);
        
        isDashing = false;
        
        yield return new WaitForSeconds(dashCooldown);
        
        canDash = true;
    }
}