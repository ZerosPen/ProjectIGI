using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Status Game")]
    public bool isGameActive;

    [SerializeField] private int fargementLight;
    public int totalFargementLight;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Update()
    {
        UIManager.Instance.UpdayeFragmentLight(totalFargementLight, fargementLight);
        SpawnManager.Instance.GetStartSpawning();

        if (totalFargementLight == fargementLight)
        {
            GameIsFinish();
        }
    }

    public void GameIsFinish()
    {
        Debug.Log("The game is done");
    }
}
