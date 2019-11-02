using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

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
   for (int i = 1; i <= itemDatabase.GetItemDatabaseList().Count; i++) {
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

  public bool UseItem(BattleCharacter target, Item item) {
    bool success = false;
    Debug.Log("PlayerItems contains item? " + playerItems.Contains(item));
    // if (playerItems.Contains(item)) {
      if (item.stats.ContainsKey("Health") && BattleController.Instance.IsValidHealTarget(target)) {
        target.Heal(item.stats["Health"]);
        success = true;
      } else {
        Debug.LogWarning("Not a heal item or was not valid heal target");
      }

      if (item.stats.ContainsKey("Energy") && BattleController.Instance.IsValidEnergyHealTarget(target)) {
        target.RecoverEnergy(item.stats["Energy"]);
        success = true;
      } else {
        Debug.LogWarning("Not an EP recovery item or was not valid heal target");
      }
      if (success) {
        RemoveItemFromUI(item);
      }

      return success;
      // TODO need to remove from UI the next time it's opened
    // } else {
    //   Debug.LogWarning("Item not found in inventory!");
    // }
  }
}
