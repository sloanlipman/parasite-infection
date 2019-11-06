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
    int index = 0;
    int indexToRemove = -1;
    for (int j = 0; j < itemsList.Count; j++) {
      if (item.itemName == itemsList[j].itemName) {
        indexToRemove = index;
        break;
      }
      index++;
    }

    if (indexToRemove > -1) {
      uiItemsList[indexToRemove].UpdateItem(null);
      RemoveItem(item);
    }
  }

  public void ClearInventory() {
    playerItems.Clear();
  }

  public void ClearAndParseUInventory() {
    List<UIItem> uiItemsList = inventoryUI.GetUIItems();
    List<int> uiItemIdList = new List<int>();
    uiItemsList.ForEach(uiItem => uiItemIdList.Add(uiItem.item.id));
    playerItems.Clear();
    inventoryUI.ClearSlots();

    uiItemIdList.ForEach(id => {
      GiveItem(id);
    });
  }
}
