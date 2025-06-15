using UnityEngine;
using UnityEngine.SceneManagement;
public class MeinMenu : MonoBehaviour
{
   public void Play()
    {
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
