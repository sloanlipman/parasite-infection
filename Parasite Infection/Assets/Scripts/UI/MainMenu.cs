using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public void NewGame() {
    SceneManager.LoadScene("Intro");

  }

  public void LoadGame() {
    SaveService.Instance.Load();

  }

  public void ShowCredits() {
    Application.OpenURL("http://www.google.com");

  }
}
