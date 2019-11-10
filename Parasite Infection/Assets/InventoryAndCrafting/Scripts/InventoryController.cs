using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

public class InventoryController : MonoBehaviour {

  private Inventory[] inventories;
  private ItemDatabase itemDatabase;
  private UIPartyPanel partyPanel;
  [SerializeField] private UIItem selectedItem;
  [SerializeField] private UIItem itemToConsume;

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

  void Update() {
    if (itemToConsume.item != null) {
      if (IsCraftingItem(itemToConsume.item)) {
        Inventory craftingInventory = PerformSelectCorrectInventory(itemToConsume.item);
        craftingInventory.GiveItem(itemToConsume.item.id);
        itemToConsume.UpdateItem(null);
      } else {
        ConsumeItemFromUI();
      }
    }
  }

  public void GiveItem(string itemName) {
    Item item = itemDatabase.GetItem(itemName);
    Inventory inventoryToUse = PerformSelectCorrectInventory(item);
    inventoryToUse.GiveItem(item.id);
  }

  public void GiveItem(int id) {
    Item item = itemDatabase.GetItem(id);
    Inventory inventoryToUse = PerformSelectCorrectInventory(item);
    inventoryToUse.GiveItem(item.id);
  }

  public void GiveItem(Item item) {
    Inventory inventoryToUse = PerformSelectCorrectInventory(item);
    inventoryToUse.GiveItem(item.id);
  }

  public void PrepareForSave() {
    partyPanel.ParseUIForCurrentEquipment();
    PutItemBackBeforeSave();
  }

  public void PrepareForLoad() {
    DeselectItem(false);
    ClearIndices();
  }

  public void ClearIndices() {
    itemDatabase.ClearCurrentItemList();
  }

  public bool IsItemOnCurrentItemList(Item item) {
    return itemDatabase.GetCurrentItemList().Contains(item);
  }

  public void AddToListOfCurrentItems(Item item) {
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
    }
    return inventory;
  }

  public void PutItemBackBeforeSave() {
    PartyMember member = partyPanel.LookUpSelectedPartyMember();

    if (IsAnItemSelected()) {
      GiveItem(selectedItem.item);

      if (member != null) {
        int i = 0;
        foreach(Item item in member.GetEquipment()) {
          if (item != null && item == selectedItem.item) {
            member.equipment[i] = null;
            break;
          }
          i++;
        }
      }
    }
    selectedItem.DirectlyNullifyItem();
  }

  public void ConsumeItemFromUI() {
    PartyMember member = partyPanel.LookUpSelectedPartyMember();
      if (!IsCraftingItem(itemToConsume.item) && member != null) {
        ConsumableInventory consumableInventory = FindObjectOfType<ConsumableInventory>();
        bool success = consumableInventory.UseItemOutsideOfBattle(member, itemToConsume.item);
        if (success) {
          consumableInventory.ClearAndParseUInventory();
          UIPlayerInfoPanel infoPanel = FindObjectOfType<UIPlayerInfoPanel>();
          if (!infoPanel.gameObject.activeSelf) {
            infoPanel.gameObject.SetActive(true);
          }
          infoPanel.Populate(member.characterName);
      } else {
        GiveItem(itemToConsume.item);
      }
    }
      itemToConsume.UpdateItem(null);

  }

  public void DeselectItem(bool returnToInventory = true) {
    PartyMember member = partyPanel.LookUpSelectedPartyMember();
    HashSet<Item> playerItems = new HashSet<Item>();
    List<Item> items = new List<Item>();
    if (member != null) {
      foreach(Item item in member.GetEquipment()) {
        items.Add(item);
      }
      playerItems = FindObjectOfType<CraftingInventory>().playerItems;
    }

    if (selectedItem != null && IsAnItemSelected()) {
      Inventory inventoryToUse = PerformSelectCorrectInventory(selectedItem.item);
      if (returnToInventory && !items.Contains(selectedItem.item)) {
        inventoryToUse.GiveItem(selectedItem.item.id);
      }
      selectedItem.DirectlyNullifyItem();
    }
  }
}
