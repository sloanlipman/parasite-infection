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
    // Debug.Log("Just added item " + itemToAdd.itemName);
    // Debug.Log("Its index is " + itemToAdd.index);
    UpdateIndices();
  }

  public void RemoveItem(Item item) {
    playerItems.Remove(item);
    // if (item.index > -1 && playerItems[item.index] != null) {
    //   playerItems[item.index] = null;
    // }
  }

  public void UpdateIndices() {
    // for (int i = playerItems.Count -1; i > -1; i--) {
    //   if (playerItems[i] == null) {
    //     playerItems.RemoveAt(i);
    //     Debug.Log("Removing at index " + i);
    //   }
    // }
    // for (int j = 0; j < playerItems.Count; j++) {
    //   Debug.Log("Item " + playerItems[j].itemName + " gets index of " + j);
    //   playerItems[j].index = j;
    // }
  }

  public void ClearInventory() {
    playerItems.Clear();
  }
}
