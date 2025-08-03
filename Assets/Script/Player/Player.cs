using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


public class Player : MonoBehaviour
{
    [Header("Status Player")]
    public float maxHealthpoint;
    public float radiusAroundPlayer;
    public Transform sourecLightAroundPlayer;
    public Light2D lightAroundPlayer;
    public LayerMask detectableLayer;

    public float healthPoint { get; set; }

    [Header("References")]
    public Slider healthbar;
    private HashSet<GameObject> previouslyDetected = new HashSet<GameObject>();

    private void Start()
    {
        healthPoint = maxHealthpoint;
    }

    private void Update()
    {
        healthbar.value = healthPoint / maxHealthpoint;

        lightAroundPlayer.pointLightOuterRadius = radiusAroundPlayer;
        Collider2D[] detected = Physics2D.OverlapCircleAll(sourecLightAroundPlayer.position, radiusAroundPlayer, detectableLayer);
        HashSet<GameObject> currentlyDetected = new HashSet<GameObject>();

        foreach (Collider2D col in detected)
        {
            GameObject obj = col.gameObject;
            currentlyDetected.Add(obj);

            if (!previouslyDetected.Contains(obj))
            {
                // Show effect (e.g. highlight)
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.enabled = true;
                    sr.color = Color.yellow; // or your highlight color
                }
                    
            }
        }

        // Find objects that are no longer in range
        foreach (GameObject obj in previouslyDetected)
        {
            if (!currentlyDetected.Contains(obj))
            {
                // Hide effect
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.enabled = false;
                    sr.color = Color.white; // or reset to original
                }
                    
            }
        }

        // Update previous set
        previouslyDetected = currentlyDetected;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MusicManager.instance.playMusic("BGM");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radiusAroundPlayer);
    }
}
