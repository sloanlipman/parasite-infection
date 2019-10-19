using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using BattleSystem;

public class SaveService : MonoBehaviour {

  public static SaveService Instance {get; set;}
  private BattleSystem.CharacterController characterController;
  private Inventory inventory;
  private QuestController questController;
  private MenuController menuController;
  private DialogPanel dialogPanel;
  private SlotPanel[] slotPanels = new SlotPanel[] {};

  // Start is called before the first frame update
  public void Save() {
    inventory.Save();
    questController.Save();
    characterController.Save();

    SavePlayer();
    SaveNPCs();
  }

  public void Load() {
    if (ES3.FileExists() && ES3.FileExists("PlayerInfo.es3")) {
      ClearAll();
      menuController.CloseAllMenus();
      inventory.Load();
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
    slotPanels = Resources.FindObjectsOfTypeAll(typeof(SlotPanel)) as SlotPanel[];
    foreach(SlotPanel panel in slotPanels) {
      panel.EmptyAllSlots();
    }
    inventory.ClearInventory();
    questController.ClearQuests();
    characterController.ClearCharacters();
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
      ES3.Load<GameObject>("Player", "PlayerInfo.es3");
    }
  }

  private void SaveNPCs() {
    NPC[] currentNPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      ES3.Save<GameObject>("NPCGameObject" + i, currentNPCs[i].gameObject, "NPCs.es3");
      ES3.Save<NPC>("NPC" + i, currentNPCs[i], "NPCs.es3");
    }
  }

  private void LoadNPCs() {
    NPC[] currentNPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      try {
        ES3.LoadInto<NPC>("NPC" + i, "NPCs.es3", currentNPCs[i]);
        ES3.LoadInto<GameObject>("NPCGameObject" + i, "NPCs.es3", currentNPCs[i].gameObject);
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
    inventory = FindObjectOfType<Inventory>();
    questController = FindObjectOfType<QuestController>();
    menuController = FindObjectOfType<MenuController>();
    characterController = FindObjectOfType<CharacterController>();
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
