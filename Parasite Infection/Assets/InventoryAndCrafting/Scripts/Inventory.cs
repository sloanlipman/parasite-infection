using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {
  public List<Item> playerItems = new List<Item>();
  [SerializeField] private UIInventory inventoryUI;
  ItemDatabase itemDatabase;

  public void Save() {
    UpdateIndices();
    List<Item> itemsToSave = new List<Item>();
    itemsToSave = inventoryUI.GetMainInventoryItems();
    int[] itemIds = new int[itemsToSave.Count];
    int i = 0;
    itemsToSave.ForEach(item => {
      itemIds[i] = (itemDatabase.GetItemId(item));
      i++;
    }); 
    ES3.Save<int[]>("Inventory", itemIds, "Inventory.es3");
  }

  public void Load() {
    int[] itemsToLoad = ES3.Load<int[]>("Inventory", "Inventory.es3");
    foreach(int id in itemsToLoad) {
      GiveItem(id);
    };
  }

  void Awake() {
    if (FindObjectsOfType<Inventory>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
    itemDatabase = FindObjectOfType<ItemDatabase>();
  }

  private void Start() {
    for (int i = 1; i <= itemDatabase.items.Count; i++) {
      GiveItem(i);
    }
    // GiveItem(6);
    // GiveItem(9);
    // GiveItem(10);
    // GiveItem(2);
    // GiveItem(2);
    GiveItem(9);
    GiveItem(9);
    GiveItem(10);
    GiveItem(10);
    inventoryUI.gameObject.SetActive(false);
  }

  public void GiveItem(int id) {
    Item itemToAdd = itemDatabase.GetItem(id);
    itemToAdd.index = playerItems.Count;
    PerformGiveItem(itemToAdd);
  }

  public void GiveItem(string itemName) {
    Item itemToAdd = itemDatabase.GetItem(itemName);
    itemToAdd.index = playerItems.Count;
    PerformGiveItem(itemToAdd);
  }

  private void PerformGiveItem(Item itemToAdd) {
    inventoryUI.AddItemToUI(itemToAdd);
    playerItems.Add(itemToAdd);
    UpdateIndices();
  }

  public void RemoveItem(int index) {
    playerItems[index] = null;
  }



  public void UpdateIndices() {
    for (int i = playerItems.Count -1; i > -1; i--) {
      if (playerItems[i] == null) {
        playerItems.RemoveAt(i);
      }
    }
    for (int j = 0; j < playerItems.Count; j++) {
      playerItems[j].index = j;
    }
  }

  public void ClearInventory() {
    playerItems.Clear();
  }
}
