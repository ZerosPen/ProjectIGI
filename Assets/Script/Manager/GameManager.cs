using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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
        SpawnManager.Instance.GetStartSpawning(fargementLight);
    }

    public void GameIsFinish()
    {

    }

}
