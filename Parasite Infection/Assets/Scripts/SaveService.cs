using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class SaveService : MonoBehaviour {

   private Inventory inventory;
   private QuestController questController;
   private MenuController menuController;
   private Dialog dialogPanel;
   private Player player;
   private SlotPanel[] slotPanels;
  // Start is called before the first frame update
  public void Save() {
    inventory.Save();
    questController.Save();
    player = FindObjectOfType<Player>();
    player.Save();
  }

  public void Load() {
    ClearAll();
    inventory.Load();
    questController.Load();
    menuController.UnpauseGame();
    LoadPlayer();
    ResetDialog();
  }

  private void ClearAll() {
    slotPanels = Resources.FindObjectsOfTypeAll(typeof(SlotPanel)) as SlotPanel[];
    foreach(SlotPanel panel in slotPanels) {
      panel.EmptyAllSlots();
    }
    inventory.ClearInventory();
    questController.ClearQuests();
  }

  private void LoadPlayer() {
    player = FindObjectOfType<Player>();
    player.Load();
  }

  private void ResetDialog() {
    dialogPanel = FindObjectOfType<Dialog>();
    if (dialogPanel != null) {
      dialogPanel.ResetDialog();
    }
  }

  // Update is called once per frame
  void Awake() {
    inventory = FindObjectOfType<Inventory>();
    questController = FindObjectOfType<QuestController>();
    menuController = FindObjectOfType<MenuController>();
  }

  void Start() {
    if (FindObjectsOfType<SaveService>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
  }
}
