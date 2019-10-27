using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {
  public List<Item> playerItems = new List<Item>();
  [SerializeField] protected UIInventory inventoryUI;
  [SerializeField] private UIItem selectedUIItem;
  protected ItemDatabase itemDatabase;

  void Awake() {
  }

  private void Start() {

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
    inventoryUI.AddItemToUI(selectedUIItem.GetSelectedItem().item);
    selectedUIItem.GetSelectedItem().UpdateItem(null);
  }
}
