using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
  [SerializeField] private Button[] dialogButtons;
  [SerializeField] private GameObject menuPanel;
  [SerializeField] private UIInventory inventory;
  [SerializeField] private QuestSystem.QuestPanel questPanel;

  public static bool IsBattleCurrentScene() {
    return SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Battle");
  }

  private void Awake() {
    if (FindObjectsOfType<MenuController>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
  }

  void Start() {
    ToggleMenu(false);
  }

  void Update() {
    if (CanPause()) {
      if (menuPanel.gameObject.activeSelf) {
        UnpauseGame();
      } else {
        PauseGame();
      }
    }
  }

  public bool CanPause() {
    return !IsBattleCurrentScene() && Input.GetKeyDown(KeyCode.Escape);
  }
  public void PauseGame() {
    ToggleDialogButtons(false);
    ToggleMenu(true);
    Time.timeScale = 0;
  }

  public void UnpauseGame() {
    ToggleDialogButtons(true);
    inventory.gameObject.SetActive(false);
    questPanel.gameObject.SetActive(false);
    ToggleMenu(false);
    Time.timeScale = 1;
  }

  public void ToggleUIInventory() {
    inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
  }

  public void ToggleQuestPanel() {
    questPanel.gameObject.SetActive(!questPanel.gameObject.activeSelf);
  }

  public void ToggleMenu(bool state) {
    menuPanel.gameObject.SetActive(state);
  }

  public void ToggleDialogButtons(bool state) {
    foreach(Button b in dialogButtons) {
      if (b != null) {
        b.interactable = state;
      }
    }
  }
}
