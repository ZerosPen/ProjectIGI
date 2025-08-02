using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("Status Player")]
    public float maxHealthpoint;
    
    public float healthPoint { get; set; }

    [Header("References")]
    public Slider healthbar;

    private void Start()
    {
        healthPoint = maxHealthpoint;
    }

    private void Update()
    {
        healthbar.value = healthPoint / maxHealthpoint;
        
    }
}
