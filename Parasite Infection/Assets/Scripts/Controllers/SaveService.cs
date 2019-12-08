using UnityEngine;
using QuestSystem;
using BattleSystem;
using UnityEngine.SceneManagement;

public class SaveService : MonoBehaviour {

  public static SaveService Instance {get; set;}
  private BattleSystem.CharacterController characterController;
  private BattleSystem.BattleLauncher battleLauncher;
  private InventoryController inventoryController;
  private SceneController sceneController;
  private CraftingInventory craftingInventory;
  private ConsumableInventory consumableInventory;
  private QuestController questController;
  private MenuController menuController;
  private DialogPanel[] dialogPanels;
  private SlotPanel[] slotPanels = new SlotPanel[] {};
  [SerializeField] private AlertPanel alertPanel;

  private bool needToLoadPlayer = false;
  private bool needToLoadNPCs = false;

  private Vector2 playerPosition;
  private int sceneIndex;

  // Start is called before the first frame update
  public void Save() {
    inventoryController.PrepareForSave();
    craftingInventory.Save();
    consumableInventory.Save();
    questController.Save();
    characterController.Save();
    sceneController.Save();

    SavePlayer();
    SaveNPCs();
    SaveScene();
    ShowAlert("Saved game!");
  }

  public void Load(bool loadedFromMenu) {
    if (ES3.FileExists()) {
      ClearAll();
      questController.Load();
      menuController.CloseAllMenus();
      craftingInventory.Load();
      consumableInventory.Load();
      characterController.Load();
      sceneController.Load();

      CloseDialog();
      LoadSavedScene();
      LoadPlayer();
      LoadNPCs();
      menuController.UnpauseGame();

      if (loadedFromMenu) {
        ShowAlert("Loaded game!");
      }

    } else {
      ShowAlert("No file to load from!");
    }
  }

  private void ShowAlert(string message) {
    if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Intro")) {
      alertPanel.gameObject.SetActive(true);

      string[] dialog = new string[]{
        message
      };

      alertPanel.StartDialog(dialog);
    }
  }

  public void StartNewGame() {
    ClearAll();
    consumableInventory.InitializeConsumableInventory();
    craftingInventory.InitializeCraftingInventory();
    SaveService.Instance.Save();
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
    sceneIndex = ES3.Load<int>("sceneIndex");

    // Load the scene if it's a different one
    if (SceneManager.GetActiveScene().buildIndex != sceneIndex) {
      SceneManager.LoadScene(sceneIndex);
    } else {
      // Otherwise run scene initialization methods
      sceneController.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
  }

  private void SavePlayer() {
    Player player = GetPlayer();
    if (player != null) {
      ES3.Save<Player>("Player", player, "PlayerInfo.json");
      ES3.Save<Vector2>("Position", player.GetRigidbody().position, "PlayerInfo.json");
    }
  }

  private void LoadPlayer() {
    Player player = GetPlayer();
    if (player != null && ES3.FileExists("PlayerInfo.json")) {
      ES3.LoadInto<Player>("Player", "PlayerInfo.json", player);
      player.GetRigidbody().position = ES3.Load<Vector2>("Position", "PlayerInfo.json");
    } else {
      needToLoadPlayer = true;
    }
  }

  private void SaveNPCs() {
    NPC[] currentNPCs = GetNPCs();
    for (int i = 0; i < currentNPCs.Length; i++) {
      ES3.Save<NPC>("npc", currentNPCs[i], "NPCs/" + currentNPCs[i].npcName + ".json");
    }
  }

  private void LoadNPCs() {
    NPC[] currentNPCs = GetNPCs();
    if (GetNPCs().Length > 0) {
      for (int i = 0; i < currentNPCs.Length; i++) {
        try {
          ES3.LoadInto<NPC>("npc", "NPCs/" + currentNPCs[i].npcName + ".json", currentNPCs[i]);
        } catch {
          Debug.LogWarning("At index " + i + " something went wrong loading an NPC");
        }
      }
    } else {
      needToLoadNPCs = true;
    }
  }

  private void CloseDialog() {
    dialogPanels = FindObjectsOfType<DialogPanel>();
    foreach(DialogPanel d in dialogPanels) {
      if (d != null) {
        d.CloseDialog();
      }
    }
  }

  private void Awake() {
    craftingInventory = FindObjectOfType<CraftingInventory>();
    consumableInventory = FindObjectOfType<ConsumableInventory>();
    questController = FindObjectOfType<QuestController>();
    menuController = FindObjectOfType<MenuController>();
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    inventoryController = FindObjectOfType<InventoryController>();
    sceneController = FindObjectOfType<SceneController>();
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
