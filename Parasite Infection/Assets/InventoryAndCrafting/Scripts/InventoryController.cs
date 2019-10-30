using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

public class InventoryController : MonoBehaviour {

  private Inventory[] inventories;
  private ItemDatabase itemDatabase;
  private UIPartyPanel partyPanel;

  private void Awake() {
    if (FindObjectsOfType<InventoryController>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
    inventories = GetComponentsInChildren<Inventory>(true);
    itemDatabase = FindObjectOfType<ItemDatabase>();
    partyPanel = FindObjectOfType<UIPartyPanel>();
  }
  // Start is called before the first frame update
  void Start() {
    foreach(Inventory inventory in inventories) {
      DeselectItem();
    }
  }

  public bool IsCraftingItem(Item item) {
    return CheckIfCrafting(item);
  }

  public bool IsEquippable(Item item) {
    return CheckIfEquippable(item);
  }

  public bool IsCraftingItem(int id) {
    Item item = itemDatabase.GetItem(id);
    return CheckIfCrafting(item);
  }

  public bool IsEquippable(int id) {
    Item item = itemDatabase.GetItem(id);
    return CheckIfEquippable(item);
  }

  public bool IsCraftingItem(string itemName) {
    Item item = itemDatabase.GetItem(itemName);
    return CheckIfCrafting(item);
  }

  public bool IsEquippable(string itemName) {
    Item item = itemDatabase.GetItem(itemName);
    return CheckIfEquippable(item);
  }

  public bool CheckIfEquippable(Item item) {
    return item.stats["Equippable"] == 1;
  }

  public bool CheckIfCrafting(Item item) {
    return item.stats["Crafting"] == 1;
  }

  public UIInventory SelectCorrectInventoryUI(Item item) {
    return PerformSelectCorrectInventoryUI(item);
  }

  public UIInventory SelectCorrectInventoryUI(string itemName) {
    Item item = itemDatabase.GetItem(itemName);
    UIInventory uIInventory = FindObjectOfType<ConsumableInventory>().GetUIInventory();
    if (IsCraftingItem(item)) {
      uIInventory = FindObjectOfType<CraftingInventory>().GetUIInventory();
    } 
    return uIInventory;
  }

  public UIInventory PerformSelectCorrectInventoryUI(Item item) {
    UIInventory uIInventory = FindObjectOfType<ConsumableInventory>().GetUIInventory();
    if (IsCraftingItem(item)) {
      uIInventory = FindObjectOfType<CraftingInventory>().GetUIInventory();
    } 
    return uIInventory;
  }

  public void DeselectItem(bool returnToInventory = true) {
    PartyMember member = partyPanel.LookUpSelectedPartyMember();
    if (member != null) {
      List<Item> items = new List<Item>();
      foreach(Item item in member.GetEquipment()) {
        items.Add(item);
      }
      GameObject uiItem = GameObject.Find("SelectedItem");
      
      if (uiItem != null) {
        UIItem selectedUIItem = uiItem.GetComponent<UIItem>();
        // Debug.Log("Player has selected ui item equipped? " + items.Contains(selectedUIItem.item));
        // Debug.Log("first item is: " + items[0].itemName + " " + items[0].index);
      
        List<Item> playerItems = FindObjectOfType<CraftingInventory>().playerItems;
        int count = playerItems.Count - 1;
        // Debug.Log("Last item in inventory is: " + playerItems[count].itemName + " " + playerItems[count].index);
        if (selectedUIItem != null && selectedUIItem.item != null) {
          UIInventory inventoryUIToUse = PerformSelectCorrectInventoryUI(selectedUIItem.item);

          if (returnToInventory && !items.Contains(selectedUIItem.item)) {
            inventoryUIToUse.AddItemToUI(selectedUIItem.item);
          }
          selectedUIItem.DirectlyNullifyItem();
        }
      }
    }
  }
}
