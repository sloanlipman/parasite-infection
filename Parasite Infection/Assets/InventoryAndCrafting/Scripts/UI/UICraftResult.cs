using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftResult : MonoBehaviour {
  public SlotPanel slotPanel;
  public CraftingInventory craftingInventory;
  [SerializeField] private CraftingSlots craftingSlots;

  public void PickItem() {
    craftingSlots.ClearCraftingSlots();
    craftingInventory.playerItems.Add(GetComponent<UIItem>().item);
    GetComponent<UIItem>().item.Collect();
  }

  public void ClearSlots() {
    slotPanel.EmptyAllSlots();
  }
}
