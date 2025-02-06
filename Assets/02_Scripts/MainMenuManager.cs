using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button startGameButton;
    public Button settingsButton;
    public Button quitButton;

    public GameObject settingsPanel;

    private void Start()
    {
        startGameButton.onClick.AddListener(StartNewGame);
        settingsButton.onClick.AddListener(ShowSettings);
        quitButton.onClick.AddListener(QuitGame);

        // 初始时隐藏帮助和设置面板
        settingsPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");
    }


    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}