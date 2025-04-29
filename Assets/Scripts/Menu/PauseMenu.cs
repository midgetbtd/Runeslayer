using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;      // A szünet menü panel
    [SerializeField] private Button resumeButton;            // Játék folytatása gomb
    [SerializeField] private Button mainMenuButton;          // Főmenü gomb
    [SerializeField] private string mainMenuSceneName = "MenuScene";  // Főmenü jelenet neve

    private bool isPaused = false;

    void Start()
    {
        // Kezdetben a szünet menü rejtett
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // Gombok eseménykezelőinek beállítása
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    // Szünet kapcsoló
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Játék folytatása
    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // Visszatérés a főmenübe
    public void ReturnToMainMenu()
    {
        // Visszaállítjuk az időt normál értékre
        Time.timeScale = 1f;

        // Főmenübe ugrás
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Szünet gomb megnyomásának kezelése
    public void OnPauseButtonPressed()
    {
        TogglePause();
    }

    // Billentyűvel való pausálás kezelése (opcionális)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
