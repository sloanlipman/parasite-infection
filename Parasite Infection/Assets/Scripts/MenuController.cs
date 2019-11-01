using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
  [SerializeField] private Button[] dialogButtons;
  [SerializeField] private GameObject menuPanel;
  [SerializeField] private UIInventory craftingInventory;
  [SerializeField] private UIInventory consumableInventory;
  [SerializeField] private QuestSystem.QuestPanel questPanel;
  [SerializeField] private UIPartyPanel partyPanel;
  [SerializeField] private GameObject playerEquipment;
  [SerializeField] private UIPlayerInfoPanel playerInfo;
  [SerializeField] private Tooltip tooltip;
  [SerializeField] private InventoryController inventoryController;

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
    if (IsTooltipActive()) {
      tooltip.gameObject.SetActive(false);
    }
    Time.timeScale = 1;
  }

  public void CloseAllMenus() {
    ToggleDialogButtons(true);
    craftingInventory.gameObject.SetActive(false);
    consumableInventory.gameObject.SetActive(false);
    questPanel.gameObject.SetActive(false);
    partyPanel.gameObject.SetActive(false);
    partyPanel.ClearPartyMember();
    playerEquipment.gameObject.SetActive(false);
    playerInfo.gameObject.SetActive(false);
    DeselectItem();
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

  private bool IsPartyPanelOpen() {
    return partyPanel.gameObject.activeSelf;
  }

  private bool IsCraftingInventoryOpen() {
    return craftingInventory.gameObject.activeSelf;
  }

  private bool IsConsumableInventoryOpen() {
    return consumableInventory.gameObject.activeSelf;
  }

  private bool IsQuestPanelOpen() {
    return questPanel.gameObject.activeSelf;
  }

  private bool IsTooltipActive() {
    return tooltip.gameObject.activeSelf;
  }

  private void DeselectItem() {
    if (inventoryController.IsAnItemSelected()) {
      partyPanel.ParseUIForCurrentEquipment();
      FindObjectOfType<InventoryController>().DeselectItem();
    }
  }

  public void ToggleCraftingInventory() {
    DeselectItem();
    craftingInventory.gameObject.SetActive(!IsCraftingInventoryOpen());
    consumableInventory.gameObject.SetActive(false);

  }

  public void ToggleConsumableInventory() {
    DeselectItem();
    consumableInventory.gameObject.SetActive(!IsConsumableInventoryOpen());
    craftingInventory.gameObject.SetActive(false);
  }

  public void ToggleQuestPanel() {
    if (IsPartyPanelOpen()) {
      partyPanel.gameObject.SetActive(false);
    }
    questPanel.gameObject.SetActive(!IsQuestPanelOpen());
  }

  public void TogglePartyPanel() {
    if (IsQuestPanelOpen()) {
        questPanel.gameObject.SetActive(false);
      }
    partyPanel.gameObject.SetActive(!IsPartyPanelOpen());
    if (IsPartyPanelOpen()) {
      partyPanel.ResetSelectedPartyMember();
      partyPanel.Populate();
    } else {
      playerEquipment.gameObject.SetActive(false);
      playerInfo.gameObject.SetActive(false);
      partyPanel.ClearPartyMember();
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
