using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour {
  [SerializeField] private SlotPanel[] slotPanels;

  public void AddItemToUI(Item item) {
    foreach(SlotPanel panel in slotPanels) {
      if (panel.ContainsEmptySlot()) {
        panel.AddItem(item);
        break;
      }
    }
  }
  public void ClearSlots() {
    for (int i = 0; i < slotPanels.Length; i++){
      slotPanels[i].ClearSlot(i);
    }
  }
}
