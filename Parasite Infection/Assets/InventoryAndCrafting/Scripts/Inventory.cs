using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {
  public HashSet<Item> playerItems = new HashSet<Item>();
  [SerializeField] protected UIInventory inventoryUI;
  [SerializeField] protected ItemDatabase itemDatabase;
  [SerializeField] protected InventoryController inventoryController;

  public UIInventory GetUIInventory() {
    return inventoryUI;
  }

  public void GiveItem(int id) {
    Item itemToAdd = itemDatabase.GetItem(id);
    PerformGiveItem(itemToAdd);
  }

  public void GiveItem(string itemName) {
    Item itemToAdd = itemDatabase.GetItem(itemName);
    PerformGiveItem(itemToAdd);
  }

  private void PerformGiveItem(Item itemToAdd) {
    inventoryController.AddToListOfCurrentItems(itemToAdd);
    inventoryUI.AddItemToUI(itemToAdd);
    playerItems.Add(new Item(itemToAdd));
  }

  public void RemoveItem(Item item) {
    playerItems.Remove(item);
  }

  public void RemoveItemFromUI(Item item) {
    List<Item> itemsList = inventoryUI.GetMainInventoryItems();
    List<UIItem> uiItemsList = inventoryUI.GetUIItems();
    int index = -1;
    foreach (Item i in playerItems) {
      if (itemsList.Contains(i)) {
        break;
      }
      index++;
    }
    if (index > -1) {
      uiItemsList[index].UpdateItem(null);
      RemoveItem(item);
    }
  }

  public void ClearInventory() {
    playerItems.Clear();
  }

  public void UpdateUIInventory() {
    inventoryUI.ClearSlots();
    foreach(Item item in playerItems) {
      inventoryUI.AddItemToUI(item);
    }
  }
}
