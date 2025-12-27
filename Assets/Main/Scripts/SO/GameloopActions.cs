#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameloopActions", menuName = "Scriptable Objects/GameloopActions")]
public class GameloopActions : ScriptableObject
{
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _winUiPrefab;

    public void DisplayWinUi()
    {
        Instantiate(_winUiPrefab, SceneManager.GetActiveScene());
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
