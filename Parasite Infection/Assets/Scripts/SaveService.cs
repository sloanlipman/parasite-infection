using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using BattleSystem;

public class SaveService : MonoBehaviour {

  public static SaveService Instance {get; set;}
  private BattleSystem.CharacterController characterController;
  private InventoryController inventoryController;
  private CraftingInventory craftingInventory;
  private ConsumableInventory consumableInventory;
  private QuestController questController;
  private MenuController menuController;
  private DialogPanel dialogPanel;
  private SlotPanel[] slotPanels = new SlotPanel[] {};

  // Start is called before the first frame update
  public void Save() {
    inventoryController.PrepareForSave();
    craftingInventory.Save();
    consumableInventory.Save();
    questController.Save();
    characterController.Save();

    SavePlayer();
    SaveNPCs();
  }

  public void Load() {
    if (ES3.FileExists() && ES3.FileExists("PlayerInfo.json")) {
      ClearAll();
      menuController.CloseAllMenus();
      craftingInventory.Load();
      consumableInventory.Load();
      questController.Load();
      characterController.Load();
      LoadPlayer();
      LoadNPCs();
      ResetDialog();
      menuController.UnpauseGame();

    } else {
      Debug.LogWarning("No file to load from!");
    }

  }

  private void ClearAll() {
    inventoryController.PrepareForLoad();
    slotPanels = Resources.FindObjectsOfTypeAll(typeof(SlotPanel)) as SlotPanel[];
    foreach(SlotPanel panel in slotPanels) {
      panel.EmptyAllSlots();
    }
    craftingInventory.ClearInventory();
    consumableInventory.ClearInventory();
    questController.ClearQuests();
  }

  private Player GetPlayer() {
    return FindObjectOfType<Player>();
  }

  public NPC[] GetNPCs() {
    return FindObjectsOfType(typeof(NPC)) as NPC[];
  }

  private void SavePlayer() {
    GetPlayer().Save();
  }

  private void LoadPlayer() {
    if (GetPlayer() != null) {
      ES3.Load<GameObject>("Player", "PlayerInfo.json");
    }
  }

  private void SaveNPCs() {
    NPC[] currentNPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      // ES3.Save<NPC>("NPCGameObject" + i, currentNPCs[i], "NPCs.json");
      ES3.Save<NPC>("NPC" + i, currentNPCs[i], "NPCs.json");
    }
  }

  private void LoadNPCs() {
    NPC[] currentNPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      try {
        ES3.LoadInto<NPC>("NPC" + i, "NPCs.json", currentNPCs[i]);
        // ES3.LoadInto<GameObject>("NPCGameObject" + i, "NPCs.json", currentNPCs[i].gameObject);
      } catch {
        Debug.LogWarning("At index " + i + " something went wrong loading an NPC");
      }
    }
  }

  private void ResetDialog() {
    dialogPanel = FindObjectOfType<DialogPanel>();
    if (dialogPanel != null) {
      dialogPanel.ResetDialog();
    }
  }

  void Awake() {
    craftingInventory = FindObjectOfType<CraftingInventory>();
    consumableInventory = FindObjectOfType<ConsumableInventory>();
    questController = FindObjectOfType<QuestController>();
    menuController = FindObjectOfType<MenuController>();
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    inventoryController = FindObjectOfType<InventoryController>();
  }

  void Start() {
    if (Instance != null && Instance != this) {
      Destroy(this.gameObject);
    } else {
      Instance = this;
    }

    DontDestroyOnLoad(this.gameObject);
  }
}
