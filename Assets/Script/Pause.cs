using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] private bool isPauseOpen;

    [Header("References")]
    public GameObject pausePanel;
    public GameObject UIGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClosePause();
        }
    }

    public void OpenClosePause()
    {
        if (!isPauseOpen)
        {
            isPauseOpen = true;
            UIGame.SetActive(false);
            pausePanel.SetActive(true);
            
        }
        else
        {
            isPauseOpen = false;
            UIGame.SetActive(true);
            pausePanel.SetActive(false);
        }
    }

    public void ChangeScene()
    {
        GameManager.Instance.isGameActive = false;
        SceneManager.LoadScene("MainMenu");
    }
}
