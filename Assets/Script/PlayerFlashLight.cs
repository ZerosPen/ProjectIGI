using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlashLight : Player
{
    [Header("Status Player")]
    [SerializeField] private float battery;
    public float maxBattery;
    public float fragmentLight;
    public bool FlashLight;
    public bool WideFlashLight;
    public bool NarrowFlashLight;

    private void Start()
    {
        FlashLight = false;
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

        battery = Mathf.Clamp(battery, 0f, maxBattery);

        if (battery <= 0f)
        {
            FlashLight = false;
            NarrowFlashLight = false;
            WideFlashLight = false;
            Debug.Log("Battery depleted — flashlight turned off.");
        }
    }

    public void TurnFlashLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (battery <= 0f)
            {
                Debug.Log("Battery empty. Cannot turn on flashlight.");
                return;
            }

            FlashLight = !FlashLight;

            if (FlashLight)
            {
                WideFlashLight = true;
                NarrowFlashLight = false;
                Debug.Log("Flashlight is now ON and in Wide Mode! ");
            }
            else
            {
                Debug.Log("Flashlight is now OFF!");
                WideFlashLight = false;
                NarrowFlashLight = false;
            }
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
    }
}
