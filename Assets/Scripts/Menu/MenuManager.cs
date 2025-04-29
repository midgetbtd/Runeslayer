using UnityEngine;
using UnityEngine.UI; // Add this for Button
using UnityEngine.SceneManagement; // Add this for SceneManager

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private string gameSceneName = "GameScene"; // A játék jelenetének neve

    void Start()
    {
        // Play gomb eseménykezelő beállítása
        if (playButton != null)
        {
            playButton.onClick.AddListener(StartGame);
        }
    }

    // Játék indítása
    public void StartGame()
    {
        Debug.Log("Játék indítása...");
        SceneManager.LoadScene(gameSceneName);
    }
}
