using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    public TextMeshProUGUI BetteryUI;
    public TextMeshProUGUI FragementLightUI;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateBattery(float bettery)
    {
        BetteryUI.text = $"battery : {(int)Mathf.Abs(bettery)}%";
    }

    public void UpdayeFragmentLight(int total, int need)
    {
        FragementLightUI.text = $"{total}/{need}";
    }

}
