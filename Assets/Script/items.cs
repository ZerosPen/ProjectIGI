using UnityEngine;
using UnityEngine.Rendering.Universal;

public class items : MonoBehaviour
{
    [Header("States")]
    public bool isPickUp;
    public bool isAvaiable;

    [Header("References")]
    private SpriteRenderer sr;
    private Light2D light2d;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        light2d = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        SetAvaible(false);
    }

    public void SetAvaible(bool state)
    {
        Debug.Log($"{gameObject.name} SetAvaible called with: {state}");
        if (sr != null) sr.enabled = state;
        if (light2d != null) light2d.enabled = state;
    }

    public void ItemGetPickUp()
    {
        Destroy(this.gameObject);
    }
}
