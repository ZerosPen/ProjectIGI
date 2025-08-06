using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerFlashLight : Player
{
    [Header("Details FlashLight")]
    public float lightRadius;
    public LayerMask revealableLayer;
    public Transform flashLightOrigin;
    public Light2D flashLight;

    [Header("Status FlashLight")]
    public float battery;
    public float maxBattery;
    public float fragmentLight;
    public bool WideFlashLight;
    public bool NarrowFlashLight;
    private Vector3 smoothedDirection = Vector3.right;
    private Vector3 targetWorldPos;

    [Header("refereance")]
    private PlayerAnimator playerAnimator;
    private HashSet<items> lastDetectFlashLightItem = new HashSet<items>();
    private HashSet<Enemy> lastDetectFlashLightEnemy = new HashSet<Enemy>();
    private HashSet<Enemy> currentDetectedFlashLightEnemy = new HashSet<Enemy>();
    private HashSet<items> currentDetectedFlashLightItem = new HashSet<items>();

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        flashLight.enabled = FlashLightOn = false;
        smoothedDirection = Vector3.right;
        battery = maxBattery;
    }

    private void Update()
    {
        if (battery > maxBattery)
        {
            battery = maxBattery;
        }

        if (FlashLightOn && NarrowFlashLight)
        {
            battery -= 1.2f * Time.deltaTime;
        }
        else if (FlashLightOn)
        {
            battery -= 0.2f * Time.deltaTime;
        }


        if (FlashLightOn)
        {
            RevealObjectsWithFlashlight();
        }
        else
        {
            HideAllObject();
        }

        if (flashLight == null)
        {
            Debug.LogError("flashLight is not assigned!");
        }

        battery = Mathf.Clamp(battery, 0f, maxBattery);

        if (flashLightOrigin != null)
        {
            flashLightOrigin.position = transform.position;
        }

        if (battery <= 0f)
        {
            FlashLightOn = false;
            NarrowFlashLight = false;
            WideFlashLight = false;
            flashLight.enabled = false;
            Debug.Log("Battery depleted — flashlight turned off.");
        }

        UIManager.Instance.UpdateBattery(battery);
    }

    #region HideAndShowObject

    private void RevealObjectsWithFlashlight()
    {
        if (flashLight == null || !flashLight.enabled)
            return;

        Collider2D[] detected = Physics2D.OverlapCircleAll(flashLightOrigin.position, lightRadius, revealableLayer);
        HashSet<Enemy> currentDetectedFlashLightEnemy = new HashSet<Enemy>();
        HashSet<items> currentDetectedFlashLightItem = new HashSet<items>();

        foreach (Collider2D collide in detected)
        {
            if (collide.CompareTag("FragmentLight") || collide.CompareTag("Battery"))
            {
                items item = collide.GetComponent<items>();
                if (item != null)
                {
                    item.SetAvaible(true);
                    currentDetectedFlashLightItem.Add(item);
                }
            }

            if (collide.CompareTag("Enemy"))
            {
                Enemy enemy = collide.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.SetAvaible(true);
                    if (NarrowFlashLight)
                    {
                        enemy.hitBylight = true;
                    }
                    else
                    {
                        enemy.hitBylight = false;
                    }
                    currentDetectedFlashLightEnemy.Add(enemy);
                }
            }
        }

        // Hide previously visible items not detected anymore
        foreach (items prevItem in lastDetectFlashLightItem)
        {
            if (!currentDetectedFlashLightItem.Contains(prevItem))
            {
                prevItem.SetAvaible(false);
            }
        }

        // Hide previously visible enemies not detected anymore
        foreach (Enemy prevEnemy in lastDetectFlashLightEnemy)
        {
            if (!currentDetectedFlashLightEnemy.Contains(prevEnemy))
            {
                prevEnemy.SetAvaible(false);
            }
        }

        lastDetectFlashLightItem = currentDetectedFlashLightItem;
        lastDetectFlashLightEnemy = currentDetectedFlashLightEnemy;
    }


void HideAllObject()
    {
        foreach (items prevItem in lastDetectFlashLightItem)
        {
            if (!currentDetectedFlashLightItem.Contains(prevItem))
            {
                prevItem.SetAvaible(false);
            }
        }

        foreach (Enemy prevEnemy in lastDetectFlashLightEnemy)
        {
            if (!currentDetectedFlashLightEnemy.Contains(prevEnemy))
            {
                prevEnemy.SetAvaible(false);
            }
        }
    }

    #endregion

    #region (Input System)
    public void TurnFlashLight(InputAction.CallbackContext context)
    {
        if (context.performed && HasFlashLight)
        {
            if (battery <= 0f)
            {
                flashLight.enabled  = false;
                Debug.Log("Battery empty. Cannot turn on flashlight.");
                return;
            }

            FlashLightOn = !FlashLightOn;

            if (FlashLightOn)
            {
                WideFlashLight = true;
                NarrowFlashLight = false;
                flashLight.enabled = FlashLightOn;
                Debug.Log("Flashlight is now ON and in Wide Mode! ");
            }
            else
            {
                Debug.Log("Flashlight is now OFF!");
                WideFlashLight = false;
                NarrowFlashLight = false;
                flashLight.enabled = FlashLightOn;
            }
            SoundManager.instance.PlaySound2D("FlashLight");
            SetFlashlightMode();
        }
        else
        {
            Debug.Log("Find the Flash Light");
        }
    }

    public void OnSwitchModeFlashLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!FlashLightOn)
            {
                Debug.Log("Flashlight is OFF! Turn it ON first.");
                return;
            }

            if (FlashLightOn && WideFlashLight)
            {
                WideFlashLight = false;
                NarrowFlashLight = true;
                Debug.Log("Switched to Narrow mode.");
            }
            else if (FlashLightOn && NarrowFlashLight)
            {

                NarrowFlashLight = false;
                WideFlashLight = true;
                Debug.Log("Switched to Wide mode.");
            }
        }
        SetFlashlightMode();
    }

    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        Vector2 pointerScreenPos = context.ReadValue<Vector2>();
        Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(pointerScreenPos);
        pointerWorldPos.z = 0f;

        // Direction from player to cursor
        Vector3 direction = (pointerWorldPos - transform.position).normalized;

        // Get angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (FlashLightOn)
        {
            playerAnimator.AnimationOnFlashLight(angle);
        }

        // Apply angle with correct offset (adjust this based on your setup)
        flashLightOrigin.rotation = Quaternion.Euler(0, 0, angle -90);

    }

    #endregion

    void SetFlashlightMode()
    {
        if (flashLight == null) return;

        if (NarrowFlashLight)
        {
            flashLight.lightType = Light2D.LightType.Point;
            flashLight.pointLightOuterRadius = 6f;
            flashLight.pointLightInnerRadius = 0;
            flashLight.pointLightOuterAngle = 55f;
            flashLight.pointLightInnerAngle = 0;
            flashLight.falloffIntensity = 0.4f;
            flashLight.intensity = 1f;
        }
        else if (WideFlashLight)
        {
            flashLight.lightType = Light2D.LightType.Point;
            flashLight.pointLightOuterRadius = 3f;
            flashLight.pointLightInnerRadius = 0;
            flashLight.pointLightOuterAngle = 130f;
            flashLight.pointLightInnerAngle = 110f;
            flashLight.falloffIntensity = 0.55f;
            flashLight.intensity = 1f;
        }
    }
}
