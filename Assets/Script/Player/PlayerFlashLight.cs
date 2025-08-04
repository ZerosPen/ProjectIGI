using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct RevealedObjectData
{
    public SpriteRenderer sr;
    public Light2D light;
    public Enemy enemy;
    public GameObject go;

}

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

    private List<RevealedObjectData> revealedObjects = new List<RevealedObjectData>();
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
            RevealObjectsInLight();
        }
        else
        {
            HideAllRevealedObjects();
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

    void RevealObjectsInLight()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(flashLightOrigin.position, lightRadius, revealableLayer);
        HideAllRevealedObjects();

        foreach (Collider2D hit in hits)
        {
            if (hit == null || hit.gameObject == null) continue;

            GameObject go = hit.gameObject;

            // Get all SpriteRenderers and Light2Ds in this object and children
            var srs = go.GetComponentsInChildren<SpriteRenderer>(true);
            var lights = go.GetComponentsInChildren<Light2D>(true);
            var enemy = go.GetComponent<Enemy>();

            foreach (var sr in srs)
                sr.enabled = true;

            foreach (var light in lights)
                light.enabled = true;

            if (enemy != null)
                enemy.hitBylight = NarrowFlashLight;

            // Track for hiding
            revealedObjects.Add(new RevealedObjectData
            {
                go = go,
                sr = srs.Length > 0 ? srs[0] : null, // Just for backward compatibility
                light = lights.Length > 0 ? lights[0] : null,
                enemy = enemy
            });
        }
    }

    void HideAllRevealedObjects()
    {
        foreach (var obj in revealedObjects)
        {
            if (obj.go == null) continue;

            try
            {
                // Disable all SpriteRenderers and Light2Ds in the object
                var srs = obj.go.GetComponentsInChildren<SpriteRenderer>(true);
                var lights = obj.go.GetComponentsInChildren<Light2D>(true);

                foreach (var sr in srs)
                {
                    sr.enabled = false;
                }

                foreach (var light in lights)
                    light.enabled = false;

                if (obj.enemy != null)
                    obj.enemy.hitBylight = false;
            }
            catch (MissingReferenceException) { continue; }
        }

        revealedObjects.Clear();
    }



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
