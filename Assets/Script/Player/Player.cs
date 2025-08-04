using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct RevealableObject
{
    public GameObject obj;
    public SpriteRenderer sr;
    public Light2D light;

    public RevealableObject(GameObject obj)
    {
        this.obj = obj;
        this.sr = obj.GetComponentInChildren<SpriteRenderer>();
        this.light = obj.GetComponentInChildren<Light2D>();
    }

    public void Show()
    {
        if (sr != null)
        {
            sr.enabled = true;
        }

        if (light != null)
        {
            light.enabled = true;
        }
    }

    public void Hide()
    {
        if (sr != null)
        {
            sr.enabled = false;
        }

        if (light != null)
        {
            light.enabled = false;
        }
    }
}

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

    private Dictionary<GameObject, RevealableObject> previouslyDetected = new Dictionary<GameObject, RevealableObject>();

    private void Start()
    {
        healthPoint = maxHealthpoint;
    }

    private void Update()
    {
        healthbar.value = healthPoint / maxHealthpoint;
        lightAroundPlayer.pointLightOuterRadius = radiusAroundPlayer;

        Collider2D[] detected = Physics2D.OverlapCircleAll(sourecLightAroundPlayer.position, radiusAroundPlayer, detectableLayer);
        Dictionary<GameObject, RevealableObject> currentlyDetected = new Dictionary<GameObject, RevealableObject>();

        foreach (Collider2D col in detected)
        {
            GameObject obj = col.gameObject;

            if (!previouslyDetected.ContainsKey(obj))
            {
                RevealableObject revealObj = new RevealableObject(obj);
                revealObj.Show();
                currentlyDetected[obj] = revealObj;
            }
            else
            {
                // Still in range, reuse previous reference
                currentlyDetected[obj] = previouslyDetected[obj];
            }
        }

        // Hide objects no longer detected
        foreach (var pair in previouslyDetected)
        {
            if (!currentlyDetected.ContainsKey(pair.Key))
            {
                pair.Value.Hide();
            }
        }

        // Update
        previouslyDetected = currentlyDetected;
        GameManager.Instance.totalFargementLight = FragmentLight;
        Debug.Log($"HP {healthPoint}");
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
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Battery"))
        {
            Debug.Log("Try grab Battery");
            PlayerFlashLight battery = GetComponent<PlayerFlashLight>();
            battery.battery += 25;
            Destroy(collision.gameObject);
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
