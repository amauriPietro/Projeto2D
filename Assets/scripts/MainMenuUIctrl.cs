using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIctrl : MonoBehaviour {

	public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
