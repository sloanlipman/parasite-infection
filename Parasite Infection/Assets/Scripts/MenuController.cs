using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
  [SerializeField] private Button[] dialogButtons;
  [SerializeField] private GameObject menuPanel;
  [SerializeField] private UIInventory inventory;
  [SerializeField] private QuestSystem.QuestPanel questPanel;
  private bool wasDialogActive;

  private void Awake() {
    if (FindObjectsOfType<MenuController>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
  }

  void Start() {
    menuPanel.gameObject.SetActive(false);

  }

  void Update() {
    if (!Helper.isBattleCurrentScene() && Input.GetKeyDown(KeyCode.Escape)) {
      foreach(Button b in dialogButtons) {
        if (b != null) {
          b.interactable = false;
        }
      }
      menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
      Time.timeScale = Time.timeScale == 1 ? 0 : 1;
      if (!menuPanel.gameObject.activeSelf) {
      foreach(Button b in dialogButtons) {
        if (b != null) {
          b.interactable = true;
        }
      }
        inventory.gameObject.SetActive(false);
        questPanel.gameObject.SetActive(false);
      }
    }


  }

  public void ToggleInventoryCanvas() {
    inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
  }

  public void ToggleQuestCanvas() {
    questPanel.gameObject.SetActive(!questPanel.gameObject.activeSelf);
  }
}
