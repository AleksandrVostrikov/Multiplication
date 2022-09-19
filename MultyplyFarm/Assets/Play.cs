using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    private void Update()
    {
        ExitGame();
    }
    private void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
    private void OnMouseDown()
    {
        SceneManager.LoadScene(1);
    }
}
