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

    public void SetHitByLight(bool value)
    {
        hitBylight = value;
    }

    void EnemyIsDead()
    {
        Debug.Log("Enemy is dead!");
        Destroy(gameObject);
    }
}
