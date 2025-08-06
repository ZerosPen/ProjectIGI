using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    [Header("Status Player")]
    public int FragmentLight;
    public float maxHealthpoint;
    public float radiusAroundPlayer;
    public Transform sourecLightAroundPlayer;
    public Light2D lightAroundPlayer;
    public LayerMask detectableLayer;
    [SerializeField] private bool getHit;

    public static bool FlashLightOn { get; protected set; }
    public static bool HasFlashLight { get; protected set; }
    public float healthPoint;

    [Header("References")]
    public Slider healthbar;
    
    private HashSet<items> lastDetectItem = new HashSet<items>();
    private HashSet<Enemy> lastDetectEnemy = new HashSet<Enemy>();
    private HashSet<Enemy> currentDetectedEnemy = new HashSet<Enemy>();

    private void Start()
    {
        healthPoint = maxHealthpoint;
    }

    private void Update()
    {
        healthbar.value = healthPoint / maxHealthpoint;
        lightAroundPlayer.pointLightOuterRadius = radiusAroundPlayer;

        Collider2D[] detected = Physics2D.OverlapCircleAll(sourecLightAroundPlayer.position, radiusAroundPlayer, detectableLayer);
        HashSet<items> currentDetectedItem = new HashSet<items>();
        HashSet<Enemy> currentDetectedEnemy = new HashSet<Enemy>();


        foreach (Collider2D collide in detected)
        {
            if (collide.CompareTag("FragmentLight") || collide.CompareTag("Battery"))
            {
                items item = collide.GetComponent<items>();
                if (item != null)
                {
                    item.SetAvaible(true);
                    currentDetectedItem.Add(item);
                }
            }

            if (collide.CompareTag("Enemy"))
            {
                Enemy enemy = collide.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.SetAvaible(true);
                    currentDetectedEnemy.Add(enemy);
                }
            }
        }

        foreach (items prevItem in lastDetectItem)
        {
            if (!currentDetectedItem.Contains(prevItem))
            {
                prevItem.SetAvaible(false);
            }
        }

        foreach (Enemy prevEnemy in lastDetectEnemy)
        {
            if (!currentDetectedEnemy.Contains(prevEnemy))
            {
                prevEnemy.SetAvaible(false);
            }
        }

        lastDetectItem = currentDetectedItem;
        lastDetectEnemy = currentDetectedEnemy;
        GameManager.Instance.totalFargementLight = FragmentLight;
    }

    public void OnIntreaction(InputAction.CallbackContext context)
    {
        if (context.performed && !HasFlashLight)
        {
            HasFlashLight = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FragmentLight"))
        {
            FragmentLight++;
            items item = collision.GetComponent<items>();

            if (item! != null)
            {
                item.ItemGetPickUp();
            }
        }
        else if (collision.CompareTag("Battery"))
        {
            Debug.Log("Try grab Battery");
            PlayerFlashLight batteryValue = GetComponent<PlayerFlashLight>();
            if (batteryValue != null) batteryValue.battery += 25;

            items item = collision.GetComponent<items>();
            if (item! != null)
            {
                item.ItemGetPickUp();
            }
        }
    }

    public void TakingDamage(float damage)
    {
        Debug.Log("hit by enemy");
        healthPoint -= damage;
        if (healthPoint <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radiusAroundPlayer);
    }
}
