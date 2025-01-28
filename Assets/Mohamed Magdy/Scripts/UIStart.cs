using UnityEngine;

public class UIStart : MonoBehaviour
{
    public void Click()
    {
        GameManager.Instance.NewGame();
    }
    public void Resume()
    {
        GameManager.Instance.Resume();
    }
    public void MainMenue()
    {
        GameManager.Instance.MainMenu();
    }
    public void unpause()
    {
        GameManager.Instance.unpause();
    }
    public void QuitGame()
    {
        // Log a message for debugging
        Debug.Log("Application is quitting...");

        // Quit the application
        Application.Quit();

        // For the Unity Editor (only for testing purposes)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
