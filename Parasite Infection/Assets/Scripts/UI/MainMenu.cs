using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public void NewGame() {
    SceneManager.LoadScene("Command Center");

  }

  public void LoadGame() {
    // SceneManager.LoadScene("Command Center");
    SaveService.Instance.Load();

  }

  public void ShowCredits() {
    Application.OpenURL("http://www.google.com");

  }
}
