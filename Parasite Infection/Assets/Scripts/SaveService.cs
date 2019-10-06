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
    // questDatabase.Save();
    player = FindObjectOfType<Player>();
    player.Save();
  }

  public void Load() {
    inventory.Load();
    questController.Load();
    // questDatabase.Load();
    menuController.UnpauseGame();
    player = FindObjectOfType<Player>();
    player.Load();
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
