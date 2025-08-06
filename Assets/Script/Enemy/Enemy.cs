using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("status")]
    public float DurationHitByLight;
    [SerializeField] private float currTime; 
    public bool hitBylight;
    public bool isDead;
    public float damage;

    [Header("Attack")]
    public float attackCooldown = 2f;
    private bool canAttack = true;

    [Header("Retreat")]
    public float retreatForce = 3f;
    public float retreatDuration = 1f;
    private Rigidbody2D rb;

    [Header("References")]
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetAvaible(false);
    }

    private void Update()
    {
        if (isDead) return;

        if (hitBylight)
        {
            currTime -= Time.deltaTime;
            if (currTime <= 0)
            {
                currTime = 0f;
                isDead = true;
                EnemyIsDead();
            }
        }
        else
        {
            currTime = DurationHitByLight;
        }
    }

    public void SetAvaible(bool state)
    {
        if (sr != null)
        {
            sr.enabled = state;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canAttack || isDead) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("try to hit player");
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakingDamage(damage);
                canAttack = false;
                StartCoroutine(AttackCooldown());

                Vector2 retreatDir = (transform.position - player.transform.position).normalized;
                StartCoroutine(WalkAwayFromPlayer(retreatDir));
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator WalkAwayFromPlayer(Vector2 direction)
    {
        float timer = 0f;
        while (timer < retreatDuration)
        {
            rb.velocity = direction * retreatForce;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
    }

    void EnemyIsDead()
    {

        SpawnManager.Instance.ReduceEnemyCount();
        Debug.Log("Enemy is dead!");
        Destroy(gameObject);
    }
}
