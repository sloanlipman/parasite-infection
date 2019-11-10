using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventory : Inventory {

  public void Save() {
    List<Item> itemsToSave = new List<Item>();
    itemsToSave = inventoryUI.GetMainInventoryItems();
    int[] itemIds = new int[itemsToSave.Count];
    int i = 0;
    itemsToSave.ForEach(item => {
      itemIds[i] = (itemDatabase.GetItemId(item));
      i++;
    }); 
    ES3.Save<int[]>("CraftingInventory", itemIds, "Inventory.json");
  }

  public void Load() {
    int[] itemsToLoad = ES3.Load<int[]>("CraftingInventory", "Inventory.json");
    foreach(int id in itemsToLoad) {
      GiveItem(id);
    };
  }

  public void InitializeCraftingInventory() {
    GiveItem(1);
  }

  // Start is called before the first frame update
  void Start() {
    UIItem[] craftingInventorySlots = inventoryUI.GetComponentsInChildren<UIItem>();
    foreach(UIItem item in craftingInventorySlots) {
      item.isCraftingInventorySlot = true;
    }
    inventoryUI.gameObject.SetActive(false);
  }

  // Update is called once per frame
  void Awake() {
    inventoryController = FindObjectOfType<InventoryController>();
    itemDatabase = FindObjectOfType<ItemDatabase>();
  }
}
