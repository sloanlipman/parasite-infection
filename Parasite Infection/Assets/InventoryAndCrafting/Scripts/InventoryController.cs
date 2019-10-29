using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

  private Inventory[] inventories;
  private ItemDatabase itemDatabase;

  private void Awake() {
    if (FindObjectsOfType<InventoryController>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
    inventories = GetComponentsInChildren<Inventory>(true);
    itemDatabase = FindObjectOfType<ItemDatabase>();
  }
  // Start is called before the first frame update
  void Start() {
    foreach(Inventory inventory in inventories) {
      DeselectItem();
    }

  }

  // Update is called once per frame
  void Update() {

  }

  public bool IsCraftingItem(int id) {
    Item item = itemDatabase.GetItem(id);
    return item.stats["Crafting"] == 1;
  }

  public bool IsEquippable(int id) {
    Item item = itemDatabase.GetItem(id);
    return item.stats["Equippable"] == 1;
  }

  public bool IsCraftingItem(Item item) {
    return item.stats["Crafting"] == 1;
  }

  public bool IsEquippable(Item item) {
    return item.stats["Equippable"] == 1;
  }

  public void DeselectItem(bool returnToInventory = true) {
    GameObject uiItem = GameObject.Find("SelectedItem");
    if (uiItem != null) {
      UIItem selectedUIItem = uiItem.GetComponent<UIItem>();
      if (selectedUIItem != null && selectedUIItem.item != null) {
        UIInventory inventoryUIToUse;
        if (IsCraftingItem(selectedUIItem.item)) {
          inventoryUIToUse = FindObjectOfType<CraftingInventory>().GetUIInventory();
        } else {
          inventoryUIToUse = FindObjectOfType<ConsumableInventory>().GetUIInventory();
        }
        if (returnToInventory) {
          inventoryUIToUse.AddItemToUI(selectedUIItem.item);
        }
        selectedUIItem.DirectlyNullifyItem();
      }
    }
  }
}
