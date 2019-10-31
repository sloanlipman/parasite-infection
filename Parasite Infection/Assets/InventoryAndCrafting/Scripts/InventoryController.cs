using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

public class InventoryController : MonoBehaviour {

  private Inventory[] inventories;
  private ItemDatabase itemDatabase;
  private UIPartyPanel partyPanel;
  [SerializeField] private UIItem selectedItem;

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

  public void PrepareForSave() {
    partyPanel.ParseUIForCurrentEquipment();
    PutItemBackBeforeSave();
  }

  public void PrepareForLoad() {
    DeselectItem(false);
    ClearIndices();
  }

  public int GetIndex() {
    return itemDatabase.GetNextIndex();
  }

  public void ClearIndices() {
    itemDatabase.ClearCurrentItemList();
  }

  public void AddToListOfCurrentItems(Item item) {
    item.index = GetIndex();
    itemDatabase.GetCurrentItemList().Add(new Item(item));
  }

  public bool IsAnItemSelected() {
    return selectedItem.item != null;
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

  public Inventory SelectCorrectInventory(Item item) {
    return PerformSelectCorrectInventory(item);
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

  public Inventory SelectCorrectInventory(string itemName) {
    Item item = itemDatabase.GetItem(itemName);
    Inventory inventory = FindObjectOfType<ConsumableInventory>();
    if (IsCraftingItem(item)) {
      inventory = FindObjectOfType<CraftingInventory>();
    }
    return inventory;
  }

  public Inventory PerformSelectCorrectInventory(Item item) {
    Inventory inventory = FindObjectOfType<ConsumableInventory>();
    if (IsCraftingItem(item)) {
      inventory = FindObjectOfType<CraftingInventory>();
      Debug.Log("It was a crafting item!");
    }
    return inventory;
  }

  public void PutItemBackBeforeSave() {
    PartyMember member = partyPanel.LookUpSelectedPartyMember();

    if (selectedItem.item != null) {
      Inventory inventoryToUse = PerformSelectCorrectInventory(selectedItem.item);
      inventoryToUse.GiveItem(selectedItem.item.id);

      if (member != null) {
        int i = 0;
        foreach(Item item in member.GetEquipment()) {
          if (item != null && item == selectedItem.item) {
            Debug.Log("Removed item " + item.itemName);
            Debug.Log("Selected item was: " + selectedItem.item.itemName);
            Debug.Log("Index was " + i + " , and item index was " + item.index);
            member.equipment[i] = null;
            break;
          }
          i++;
        }
      }
    }
    selectedItem.DirectlyNullifyItem();
  }

  public void DeselectItem(bool returnToInventory = true) {
    PartyMember member = partyPanel.LookUpSelectedPartyMember();
    if (member != null) {
      List<Item> items = new List<Item>();

      foreach(Item item in member.GetEquipment()) {
        items.Add(item);
      }
      
        // Debug.Log("Player has selected ui item equipped? " + items.Contains(selectedUIItem.item));
        // Debug.Log("first item is: " + items[0].itemName + " " + items[0].index);
      
        HashSet<Item> playerItems = FindObjectOfType<CraftingInventory>().playerItems;
        int count = playerItems.Count - 1;
        // Debug.Log("Last item in inventory is: " + playerItems[count].itemName + " " + playerItems[count].index);
        if (selectedItem != null && selectedItem.item != null) {
          // UIInventory inventoryUIToUse = PerformSelectCorrectInventoryUI(selectedUIItem.item);
          Inventory inventoryToUse = PerformSelectCorrectInventory(selectedItem.item);

          if (returnToInventory && !items.Contains(selectedItem.item)) {
            // inventoryUIToUse.AddItemToUI(selectedUIItem.item);
            inventoryToUse.GiveItem(selectedItem.item.id);
          }
          selectedItem.DirectlyNullifyItem();
        }
      }
  }
}
