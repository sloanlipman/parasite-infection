using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class SaveService : MonoBehaviour {

   private Inventory inventory;
   private QuestDatabase questDatabase;
   private QuestController questController;
   private MenuController menuController;
   private Player player;
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
    player = FindObjectOfType<Player>();
    player.Load();  }

  private void ClearAll() {
    inventory.ClearInventory();
    questController.assignedQuests.Clear();
    questController.completedQuests.Clear();
    questDatabase.quests.Clear();
  }

  // Update is called once per frame
  void Awake() {
    inventory = FindObjectOfType<Inventory>();
    questDatabase = FindObjectOfType<QuestDatabase>();
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
