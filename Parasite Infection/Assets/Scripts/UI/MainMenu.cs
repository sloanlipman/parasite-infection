using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public void NewGame() {
    SceneManager.LoadScene("Intro");
  }

  public void LoadGame() {
    bool loadedFromMenu = false;
    SaveService.Instance.Load(loadedFromMenu);
  }

  public void ShowCredits() {
    Application.OpenURL("https://github.com/sloanlipman/parasite-infection");
  }

  public void Quit() {
    Application.Quit();
  }
}
