using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using BattleSystem;
using UnityEngine.SceneManagement;

public class SaveService : MonoBehaviour {

  public static SaveService Instance {get; set;}
  private BattleSystem.CharacterController characterController;
  private BattleSystem.BattleLauncher battleLauncher;
  private InventoryController inventoryController;
  private CraftingInventory craftingInventory;
  private ConsumableInventory consumableInventory;
  private QuestController questController;
  private MenuController menuController;
  private DialogPanel dialogPanel;
  private SlotPanel[] slotPanels = new SlotPanel[] {};

  private bool needToLoadPlayer = false;
  private bool needToLoadNPCs = false;

  // Start is called before the first frame update
  public void Save() {
    inventoryController.PrepareForSave();
    craftingInventory.Save();
    consumableInventory.Save();
    questController.Save();
    characterController.Save();

    SavePlayer();
    SaveNPCs();
    SaveScene();
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
      LoadSavedScene();
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
    battleLauncher.ResetSteps();
  }

  private Player GetPlayer() {
    return FindObjectOfType<Player>();
  }

  public NPC[] GetNPCs() {
    return FindObjectsOfType(typeof(NPC)) as NPC[];
  }

  private void SaveScene() {
    ES3.Save<int>("sceneIndex", SceneManager.GetActiveScene().buildIndex);
  }

  private void LoadSavedScene() {
    int sceneIndex = ES3.Load<int>("sceneIndex");
    if (SceneManager.GetActiveScene().buildIndex != sceneIndex) {
      SceneManager.LoadScene(sceneIndex);
    }
  }

  private void SavePlayer() {
    GetPlayer().Save();
  }

  private void LoadPlayer() {
    if (GetPlayer() != null) {
      ES3.Load<GameObject>("Player", "PlayerInfo.json");
    } else {
      needToLoadPlayer = true;
    }
  }

  private void SaveNPCs() {
    NPC[] currentNPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      ES3.Save<NPC>("NPC" + i, currentNPCs[i], "NPCs.json");
    }
  }

  private void LoadNPCs() {
    NPC[] currentNPCs = GetNPCs();
    if (GetNPCs().Length > 0) {
      for (int i = 0; i < currentNPCs.Length; i++) {
        try {
          ES3.LoadInto<NPC>("NPC" + i, "NPCs.json", currentNPCs[i]);
        } catch {
          Debug.LogWarning("At index " + i + " something went wrong loading an NPC");
        }
      }
    } else {
      needToLoadNPCs = true;
    }
  }

  private void ResetDialog() {
    dialogPanel = FindObjectOfType<DialogPanel>();
    if (dialogPanel != null) {
      dialogPanel.ResetDialog();
    }
  }

  private void Awake() {
    craftingInventory = FindObjectOfType<CraftingInventory>();
    consumableInventory = FindObjectOfType<ConsumableInventory>();
    questController = FindObjectOfType<QuestController>();
    menuController = FindObjectOfType<MenuController>();
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    inventoryController = FindObjectOfType<InventoryController>();
    battleLauncher = FindObjectOfType<BattleLauncher>();

    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void Start() {
    if (Instance != null && Instance != this) {
      Destroy(this.gameObject);
    } else {
      Instance = this;
    }
    DontDestroyOnLoad(this.gameObject);
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    if (needToLoadNPCs) {
      LoadNPCs();
    }

    if (needToLoadPlayer) {
      LoadPlayer();
    }

    needToLoadPlayer = false;
    needToLoadNPCs = false;
  }
}
