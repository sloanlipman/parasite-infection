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
  [SerializeField] private UIPartyPanel partyPanel;

  public static bool IsBattleCurrentScene() {
    return SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Battle");
  }

  private void PauseGame() {
    ToggleDialogButtons(false);
    ToggleMenu(true);
    Time.timeScale = 0;
  }

  public void UnpauseGame() {
    CloseAllMenus();
    Time.timeScale = 1;
  }

  public void CloseAllMenus() {
    ToggleDialogButtons(true);
    inventory.gameObject.SetActive(false);
    questPanel.gameObject.SetActive(false);
    partyPanel.gameObject.SetActive(false);
    ToggleMenu(false);
  }

  private void Awake() {
    if (FindObjectsOfType<MenuController>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
  }

  private void Start() {
    CloseAllMenus();
    ToggleMenu(false);
  }

  private void Update() {
    if (CanPause()) {
      if (IsPaused()) {
        UnpauseGame();
      } else {
        PauseGame();
      }
    }
  }

  private bool CanPause() {
    return !IsBattleCurrentScene() && Input.GetKeyDown(KeyCode.Escape);
  }

  private bool IsPaused() {
    return menuPanel.gameObject.activeSelf;
  }

  public void ToggleUIInventory() {
    inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
  }

  public void ToggleQuestPanel() {
    if (partyPanel.gameObject.activeSelf) {
      partyPanel.gameObject.SetActive(false);
    }
    questPanel.gameObject.SetActive(!questPanel.gameObject.activeSelf);
  }

  public void TogglePartyPanel() {
    if (questPanel.gameObject.activeSelf) {
        questPanel.gameObject.SetActive(false);
      }
    partyPanel.gameObject.SetActive(!partyPanel.gameObject.activeSelf);
    if (partyPanel.gameObject.activeSelf) {
      partyPanel.Populate();
    }
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
