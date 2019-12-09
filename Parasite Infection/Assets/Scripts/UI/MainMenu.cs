using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  [SerializeField] private GameObject creditsPanel;

  public void NewGame() {
    SceneManager.LoadScene("Intro");
  }

  public void LoadGame() {
    bool loadedFromMenu = false;
    SaveService.Instance.Load(loadedFromMenu);
  }

  public void ToggleCreditsPanel(bool state) {
    creditsPanel.SetActive(state);
  }

  public void ViewCreditsOnline() {
    Application.OpenURL("https://github.com/sloanlipman/parasite-infection");
  }

  public void Quit() {
    Application.Quit();
  }
}
