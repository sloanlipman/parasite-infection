using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {
  public List<Item> playerItems = new List<Item>();
  [SerializeField] protected UIInventory inventoryUI;
  protected ItemDatabase itemDatabase;

  void Awake() {
  }

  private void Start() {

  }

  public UIInventory GetUIInventory() {
    return inventoryUI;
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

  public bool IsCraftingItem(int id) {
    Item item = itemDatabase.GetItem(id);
    return item.stats["Crafting"] == 1;
  }

  public bool IsEquippable(int id) {
    Item item = itemDatabase.GetItem(id);
    return item.stats["Equippable"] == 1;
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

  public void DeselectItem() {
    GameObject uiItem = GameObject.Find("SelectedItem");
    if (uiItem != null) {
      UIItem selectedUIItem = uiItem.GetComponent<UIItem>();
      if (selectedUIItem != null && selectedUIItem.item != null) {
        UIInventory inventoryUIToUse;
        if (IsCraftingItem(selectedUIItem.item.id)) {
          inventoryUIToUse = FindObjectOfType<CraftingInventory>().inventoryUI;
        } else {
          inventoryUIToUse = FindObjectOfType<ConsumableInventory>().inventoryUI;
        }
        inventoryUIToUse.AddItemToUI(selectedUIItem.item);
        selectedUIItem.UpdateItem(null);
      }
    }
  }
}
