using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
  [SerializeField] private Button[] dialogButtons;
  [SerializeField] private DialogPanel dialogPanel;
  [SerializeField] private GameObject menuPanel;
  [SerializeField] private UIInventory craftingInventory;
  [SerializeField] private UIInventory consumableInventory;
  [SerializeField] private QuestSystem.QuestPanel questPanel;
  [SerializeField] private Transform questPanelTitle;
  [SerializeField] private UIPartyPanel partyPanel;
  [SerializeField] private GameObject playerEquipment;
  [SerializeField] private UIUpgradePointPanel upgradePointPanel;
  [SerializeField] private UIPlayerInfoPanel playerInfo;
  [SerializeField] private Tooltip tooltip;
  [SerializeField] private InventoryController inventoryController;
  
  [SerializeField] private DialogData partyTutorial;
  [SerializeField] private DialogData consumableTutorial;
  [SerializeField] private DialogData craftingTutorial;
  [SerializeField] private DialogData equipmentTutorial;
  [SerializeField] private DialogData questTutorial;
  [SerializeField] private DialogData upgradeTutorial;

  [SerializeField] private DialogData tutorialDialog;
  [SerializeField] private DialogPanel tutorialPanel;

  private bool isPaused;

  public static bool IsBattleCurrentScene() {
    return SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Battle");
  }

  public static bool IsMainMenuCurrentScene() {
    return SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu");
  }

  public static bool IsIntroCurrentScene() {
    return SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Intro");
  }

  public bool IsTutorialPanelOpen() {
    return tutorialPanel.gameObject.activeSelf;
  }

  public bool IsGamePaused() {
    return isPaused;
  }

  private void PauseGame() {
    ToggleDialogButtons(false);
    ToggleMenu(true);
    isPaused = true;
    Time.timeScale = 0;
  }

  public void UnpauseGame() {
    CloseAllMenus();
    if (IsTooltipActive()) {
      tooltip.gameObject.SetActive(false);
    }

    isPaused = false;

    if (!dialogPanel.gameObject.activeSelf) {
      Time.timeScale = 1;
    }
  }

  public void CloseAllMenus() {
    ToggleDialogButtons(true);
    partyPanel.ParseUIForCurrentEquipment();
    craftingInventory.gameObject.SetActive(false);
    consumableInventory.gameObject.SetActive(false);
    questPanel.gameObject.SetActive(false);
    questPanelTitle.gameObject.SetActive(false);
    partyPanel.gameObject.SetActive(false);
    upgradePointPanel.gameObject.SetActive(false);
    partyPanel.ClearPartyMember();
    playerEquipment.gameObject.SetActive(false);
    playerInfo.gameObject.SetActive(false);
    tutorialPanel.gameObject.SetActive(false);
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
    return !IsBattleCurrentScene() && !IsMainMenuCurrentScene() && !IsIntroCurrentScene() && Input.GetKeyDown(KeyCode.Escape);
  }

  private bool IsPaused() {
    return menuPanel.gameObject.activeSelf;
  }

  private bool IsPartyPanelOpen() {
    return partyPanel.gameObject.activeSelf;
  }

  private bool IsPlayerInfoOpen() {
    return playerInfo.gameObject.activeSelf;
  }

  private bool IsPlayerEquipmentOpen() {
    return playerEquipment.gameObject.activeSelf;
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

  private bool IsUpgradePanelOpen() {
    return upgradePointPanel.gameObject.activeSelf;
  }

  private bool IsTutorialOpen() {
    return tutorialPanel.gameObject.activeSelf;
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

    if (IsCraftingInventoryOpen() && IsTutorialOpen()) {
      tutorialPanel.StartDialog(craftingTutorial.dialog);
    }
  }

  public void ToggleConsumableInventory() {
    DeselectItem();
    consumableInventory.gameObject.SetActive(!IsConsumableInventoryOpen());
    craftingInventory.gameObject.SetActive(false);
    if (IsConsumableInventoryOpen() && IsTutorialOpen()) {
      tutorialPanel.StartDialog(consumableTutorial.dialog);
    }
  }

  public void ToggleQuestPanel() {
    questPanelTitle.gameObject.SetActive(!IsQuestPanelOpen());
    questPanel.gameObject.SetActive(!IsQuestPanelOpen());

    if (IsQuestPanelOpen() && IsTutorialOpen()) {
      tutorialPanel.StartDialog(questTutorial.dialog);
    }
  }

  public void TogglePartyPanel() {
    partyPanel.gameObject.SetActive(!IsPartyPanelOpen());
    if (IsPartyPanelOpen()) {
      partyPanel.ResetSelectedPartyMember();
      partyPanel.Populate();
    } else {
      playerEquipment.gameObject.SetActive(false);
      playerInfo.gameObject.SetActive(false);
      upgradePointPanel.gameObject.SetActive(false);
      partyPanel.ClearPartyMember();
    }

    if (IsPartyPanelOpen() && IsTutorialOpen()) {
      tutorialPanel.StartDialog(partyTutorial.dialog);
    }
  }

  public void ToggleUpgradePointPanel() {
    upgradePointPanel.gameObject.SetActive(!IsUpgradePanelOpen());
    upgradePointPanel.Populate();
    if (IsUpgradePanelOpen() && IsTutorialOpen()) {
      tutorialPanel.StartDialog(upgradeTutorial.dialog);
    }
  }

  public void OpenPlayerInfo(BattleSystem.PartyMember member) {
    partyPanel.AddPlayerEquipmentSlots(member);
    if (IsTutorialOpen()) {
      tutorialPanel.StartDialog(equipmentTutorial.dialog);
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

  public void Quit() {

    SceneManager.LoadScene("Main Menu");
    UnpauseGame();
    dialogPanel.gameObject.SetActive(false);
    tutorialPanel.gameObject.SetActive(false);
  }

  public void ShowTutorial() {
    List<string> tutorialList = new List<string>();

    if (IsQuestPanelOpen()) {
      foreach(string t in questTutorial.dialog) {
        tutorialList.Add(t);
      }
    }

    if (IsConsumableInventoryOpen()) {
      foreach(string t in consumableTutorial.dialog) {
        tutorialList.Add(t);
      }
    }

    if (IsCraftingInventoryOpen()) {
      foreach(string t in craftingTutorial.dialog) {
        tutorialList.Add(t);
      }
    }

    if (IsPartyPanelOpen()) {
      foreach(string t in partyTutorial.dialog) {
        tutorialList.Add(t);
      }
    }

    if (IsPlayerEquipmentOpen()) {
      foreach(string t in equipmentTutorial.dialog) {
        tutorialList.Add(t);
      }
    }

    if (IsUpgradePanelOpen()) {
      foreach(string t in upgradeTutorial.dialog) {
        tutorialList.Add(t);
      }
    }

    if (tutorialList.Count == 0) {
      tutorialPanel.StartDialog(tutorialDialog.dialog);
    } else {
      tutorialPanel.StartDialog(tutorialList.ToArray());
    }
  }
}
