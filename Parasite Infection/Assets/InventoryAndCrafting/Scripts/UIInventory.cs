using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour {
  [SerializeField] private SlotPanel[] slotPanels;

  public List<Item> GetMainInventoryItems() {
    List<Item> items = new List<Item>();
    foreach(SlotPanel slotPanel in slotPanels) {
      UIItem[] uiItems = slotPanel.GetComponentsInChildren<UIItem>(true);
      foreach(var uiItem in uiItems) {
        if (uiItem.item != null && !uiItem.isCraftingResultSlot) {
          items.Add(uiItem.item);
        }
      }
    }
    return items;
  }

  public List<UIItem> GetUIItems() {
    List<UIItem> items = new List<UIItem>();
    foreach(SlotPanel slotPanel in slotPanels) {
      UIItem[] uiItems = slotPanel.GetComponentsInChildren<UIItem>(true);
      foreach(var uiItem in uiItems) {
        if (uiItem.item != null && !uiItem.isCraftingResultSlot) {
          items.Add(uiItem);
        }
      }
    }
    return items;
  }

  public void AddItemToUI(Item item) {
    foreach(SlotPanel panel in slotPanels) {
      if (panel.ContainsEmptySlot()) {
        panel.AddItem(item);
        break;
      }
    }
  }
}
