using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class SaveService : MonoBehaviour {

  private Inventory inventory;
  private QuestDatabase questDatabase;
  // Start is called before the first frame update
  void Save() {
    inventory.Save();
    // questDatabase.Save();
    
  }

  void Load() {
    inventory.Load();
    // questDatabase.Load();
  }

  // Update is called once per frame
  void Awake() {
    inventory = FindObjectOfType<Inventory>();
    questDatabase = FindObjectOfType<QuestDatabase>();

    Load();
  }
}
