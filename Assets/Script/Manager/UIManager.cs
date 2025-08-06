using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        if (BetteryUI == null)
        {
            GameObject batteryObj = GameObject.Find("BatteryIndicator");
            if (batteryObj != null)
                BetteryUI = batteryObj.GetComponent<TextMeshProUGUI>();
        }

        if (FragementLightUI == null)
        {
            GameObject fragObj = GameObject.Find("FragmentLight");
            if (fragObj != null)
                FragementLightUI = fragObj.GetComponent<TextMeshProUGUI>();
        }
    }

}
