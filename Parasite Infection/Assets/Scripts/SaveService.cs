using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class SaveService : MonoBehaviour {

  public static SaveService Instance {get; set;}

  private Inventory inventory;
  private QuestController questController;
  private MenuController menuController;
  private Dialog dialogPanel;
  private SlotPanel[] slotPanels = new SlotPanel[] {};

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

  private Player GetPlayer() {
    return FindObjectOfType<Player>();
  }

  public NPC[] GetNPCs() {
    // NPC[] npcsToGet = FindObjectsOfType(typeof(NPC)) as NPC[];
    // Debug.Log("npcsToGet = " + npcsToGet);
    // Debug.Log("# is: " + npcsToGet.Length);
    // List<GameObject> npcListToGet = new List<GameObject>();
    // foreach(NPC n in npcsToGet) {
    //   Debug.Log("NPC to get is " + n);
    //   npcListToGet.Add(n.gameObject);
    // }
    // return npcListToGet.ToArray();
    return FindObjectsOfType(typeof(NPC)) as NPC[];
  }

  private void SavePlayer() {
    GetPlayer().Save();
  }

  private void LoadPlayer() {
    // if (GetPlayer().gameObject != null) {
    //   Destroy(GetPlayer().gameObject);
    // }
    if (GetPlayer() != null) {
      ES3.Load<GameObject>("Player", "PlayerInfo.es3");
    }
  }

  private void SaveNPCs() {
    NPC[] currentNPCs = GetNPCs();
    // NPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      // ES3.Save<GameObject>("NPC" + i, NPCs[i], "NPCs.es3");
    ES3.Save<GameObject>("NPC" + i, currentNPCs[i].gameObject, "NPCs.es3");
    }
  }

  private void LoadNPCs() {
    // NPCs = GetNPCs();
    NPC[] currentNPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      try {
        ES3.LoadInto<GameObject>("NPC" + i, "NPCs.es3", currentNPCs[i].gameObject);
      } catch {
        Debug.LogWarning("At index " + i + " something went wrong loading an NPC");
      }
      // ES3.Load<GameObject>("NPC" + i, "NPCs.es3");
    }
    Debug.Log("After loading NPCs, we have this many: " + GetNPCs().Length);
  }

  private void ResetDialog() {
    dialogPanel = FindObjectOfType<Dialog>();
    if (dialogPanel != null) {
      dialogPanel.ResetDialog();
    }
  }

  void Awake() {
    inventory = FindObjectOfType<Inventory>();
    questController = FindObjectOfType<QuestController>();
    menuController = FindObjectOfType<MenuController>();
  }

  void Start() {
    if (Instance != null && Instance != this) {
      Destroy(this.gameObject);
    }
    else {
      Instance = this;
    }

    DontDestroyOnLoad(this.gameObject);
  }
}
