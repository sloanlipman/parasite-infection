using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventory : Inventory {

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
    ES3.Save<int[]>("CraftingInventory", itemIds, "Inventory.es3");
  }

  public void Load() {
    int[] itemsToLoad = ES3.Load<int[]>("CraftingInventory", "Inventory.es3");
    foreach(int id in itemsToLoad) {
      GiveItem(id);
    };
  }

  // Start is called before the first frame update
  void Start() {
    UIItem[] craftingInventorySlots = inventoryUI.GetComponentsInChildren<UIItem>();
    foreach(UIItem item in craftingInventorySlots) {
      item.isCraftingInventorySlot = true;
    }
   for (int i = 1; i <= itemDatabase.items.Count; i++) {
     if (IsCraftingItem(i)) {
      GiveItem(i);
     }
    }
    // GiveItem(6);
    // GiveItem(9);
    // GiveItem(10);
    // GiveItem(2);
    // GiveItem(2);
    GiveItem(9);
    GiveItem(9);
    GiveItem(10);
    inventoryUI.gameObject.SetActive(false);
  }

  // Update is called once per frame
  void Awake() {
    if (FindObjectsOfType<CraftingInventory>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
    itemDatabase = FindObjectOfType<ItemDatabase>();
  }
}
