﻿using System.Collections.Generic;
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

  public void InitializeConsumableInventory() {
  for (int i = 1; i <= itemDatabase.GetItemDatabaseList().Count; i++) {
     if (!inventoryController.IsCraftingItem(i)) {
      GiveItem(i);
     }

    }
  }

  void Start() {
    inventoryUI.gameObject.SetActive(false);
    UIItem[] consumableInventorySlots = inventoryUI.GetComponentsInChildren<UIItem>();
    foreach(UIItem item in consumableInventorySlots) {
      item.isConsumableInventorySlot = true;
    }
  }

  void Awake() {
    inventoryController = FindObjectOfType<InventoryController>();
    itemDatabase = FindObjectOfType<ItemDatabase>();
  }

  public bool UseItem(BattleCharacter target, Item item) {
    bool success = false;
    string tooltipString = string.Format("{0} recovered with {1}.", target.characterName, item.itemName);

    if (item.stats.ContainsKey("Health") && item.stats.ContainsKey("Energy") && BattleController.Instance.IsValidEnergyHealTarget(target)) {
      target.Heal(item.stats["Health"]);
      target.RecoverEnergy(item.stats["Energy"]);

      BattleController.Instance.SetCureAllAmount(item.stats["Health"], item.stats["Energy"]);
      BattleController.Instance.ShowLabel("cureAll");
      success = true;

    } else if (item.stats.ContainsKey("Health") && BattleController.Instance.IsValidHealTarget(target)) {
      target.Heal(item.stats["Health"]);
      BattleController.Instance.SetHealAmount(item.stats["Health"]);
      BattleController.Instance.ShowLabel("heal");
      success = true;

    } else if (item.stats.ContainsKey("Energy") && BattleController.Instance.IsValidEnergyHealTarget(target)) {
      target.RecoverEnergy(item.stats["Energy"]);
      success = true;
      BattleController.Instance.SetEnergyRecoveryAmount(item.stats["Energy"]);
      BattleController.Instance.ShowLabel("EP");
    }

    if (success) {
      BattleController.Instance.GetTooltip().GenerateAutoDismissTooltip(tooltipString);

      RemoveItemFromUI(item);
    } else {
        BattleController.Instance.GetTooltip().GenerateAutoDismissTooltip("Invalid target for that item!");
    }

    return success;
  }

  public bool UseItemOutsideOfBattle(BattleCharacter target, Item item) {
    bool success = false;
    if (item.stats.ContainsKey("Health") && target.IsCharacterDamaged()) {
      target.Heal(item.stats["Health"]);
      success = true;
    }

    if (item.stats.ContainsKey("Energy") && target.IsCharacterMissingEnergy()) {
      target.RecoverEnergy(item.stats["Energy"]);
      success = true;
    }

    return success;
  }
}
