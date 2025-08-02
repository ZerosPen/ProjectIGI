using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float battery;
    public float maxBattery;
    public float fragmentLight;
    public bool FlashLight;
    public bool WideFlashLight;
    public bool NarrowFlashLight;

    private List<SpriteRenderer> revealedObjects = new List<SpriteRenderer>();
    private void Start()
    {
        flashLight.enabled = FlashLight = false;
        battery = maxBattery;
    }

    private void Update()
    {
        if (FlashLight && NarrowFlashLight)
        {
            battery -= 2f * Time.deltaTime;
        }
        else if (FlashLight)
        {
            battery -= 1f * Time.deltaTime;
        }
        if (FlashLight)
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

        if (battery <= 0f)
        {
            FlashLight = false;
            NarrowFlashLight = false;
            WideFlashLight = false;
            flashLight.enabled = false;
            Debug.Log("Battery depleted — flashlight turned off.");
        }
    }

    void RevealObjectsInLight()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(flashLightOrigin.position, lightRadius, revealableLayer);

        // Hide previously revealed objects first
        HideAllRevealedObjects();

        foreach (Collider2D hit in hits)
        {
            SpriteRenderer sr = hit.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
                sr. color = Color.yellow;
                revealedObjects.Add(sr);
            }
        }
    }

    void HideAllRevealedObjects()
    {
        foreach (SpriteRenderer sr in revealedObjects)
        {
            if (sr != null)
            {
                sr.enabled = false;
                sr.color = Color.yellow;
            }
        }
        revealedObjects.Clear();
    }

    public void TurnFlashLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (battery <= 0f)
            {
                flashLight.enabled = FlashLight;
                Debug.Log("Battery empty. Cannot turn on flashlight.");
                return;
            }

            FlashLight = !FlashLight;

            if (FlashLight)
            {
                WideFlashLight = true;
                NarrowFlashLight = false;
                flashLight.enabled = FlashLight;
                Debug.Log("Flashlight is now ON and in Wide Mode! ");
            }
            else
            {
                Debug.Log("Flashlight is now OFF!");
                WideFlashLight = false;
                NarrowFlashLight = false;
                flashLight.enabled = FlashLight;
            }
            SetFlashlightMode();
        }
    }


    public void OnSwitchModeFlashLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!FlashLight)
            {
                Debug.Log("Flashlight is OFF! Turn it ON first.");
                return;
            }

            if (FlashLight && WideFlashLight)
            {
                WideFlashLight = false;
                NarrowFlashLight = true;
                Debug.Log("Switched to Narrow mode.");
            }
            else if (FlashLight && NarrowFlashLight)
            {

                NarrowFlashLight = false;
                WideFlashLight = true;
                Debug.Log("Switched to Wide mode.");
            }
        }
        SetFlashlightMode();
    }

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
