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
   private NPC[] NPCs;
   private SlotPanel[] slotPanels;
  // Start is called before the first frame update
  public void Save() {
    inventory.Save();
    questController.Save();
    SavePlayer();
    SaveNPCs();
  }

  public void Load() {
    if (ES3.FileExists() && ES3.FileExists("PlayerInfo.es3")) {
      ClearAll();

      inventory.Load();
      questController.Load();
      menuController.UnpauseGame();
      LoadPlayer();
      LoadNPCs();
      ResetDialog();
    } else {
      Debug.LogWarning("No file to load from!");
    }

  }

  private void ClearAll() {
    slotPanels = Resources.FindObjectsOfTypeAll(typeof(SlotPanel)) as SlotPanel[];
    foreach(SlotPanel panel in slotPanels) {
      panel.EmptyAllSlots();
    }
    inventory.ClearInventory();
    questController.ClearQuests();
  }

  private void GetPlayer() {
    player = FindObjectOfType<Player>();
  }

  private void GetNPCs() {
    NPCs = FindObjectsOfType(typeof(NPC)) as NPC[];
  }

  private void SavePlayer() {
    GetPlayer();
    player.Save();
  }

  private void LoadPlayer() {
    GetPlayer();
    player.Load();
  }

  private void SaveNPCs() {
    GetNPCs();
    ES3.Save<GameObject[]>("NPC", NPCs, "NPCs.es3");

  }

  private void LoadNPCs() {
    GetNPCs();
    ES3.Load<GameObject[]>("NPC", "NPCs.es3");
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
