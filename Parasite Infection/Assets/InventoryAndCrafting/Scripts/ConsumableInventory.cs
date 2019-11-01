﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableInventory : Inventory {

  public void Save() {
    List<Item> itemsToSave = new List<Item>();
    itemsToSave = inventoryUI.GetMainInventoryItems();
    int[] itemIds = new int[itemsToSave.Count];
    int i = 0;
    itemsToSave.ForEach(item => {
      itemIds[i] = (itemDatabase.GetItemId(item));
      i++;
    }); 
    ES3.Save<int[]>("ConsumableInventory", itemIds, "Inventory.json");
  }

  public void Load() {
    int[] itemsToLoad = ES3.Load<int[]>("ConsumableInventory", "Inventory.json");
    foreach(int id in itemsToLoad) {
      GiveItem(id);
    };
  }

  void Start() {
   for (int i = 1; i <= itemDatabase.itemDatabaseList.Count; i++) {
     if (!inventoryController.IsCraftingItem(i)) {
      GiveItem(i);
     }
    }
    inventoryUI.gameObject.SetActive(false);
    UIItem[] consumableInventorySlots = inventoryUI.GetComponentsInChildren<UIItem>();
    foreach(UIItem item in consumableInventorySlots) {
      item.isConsumableInventorySlot = true;
    }
  }

  void Awake() {
    if (FindObjectsOfType<ConsumableInventory>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
    inventoryController = FindObjectOfType<InventoryController>();
    itemDatabase = FindObjectOfType<ItemDatabase>();
  }
}
