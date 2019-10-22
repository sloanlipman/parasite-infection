using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPanel : MonoBehaviour {

  public List<UIItem> uiItems = new List<UIItem>();
  private List<GameObject> slots = new List<GameObject>();
  public int numberOfSlots;
  public GameObject slotPrefab;
  
  void Awake() {
    SetUpSlots();
  }

  public void SetUpSlots() {
    for (int i = 0; i < numberOfSlots; i++) {
      GameObject instance = Instantiate(slotPrefab);
      slots.Add(instance);
      instance.transform.SetParent(transform);
      uiItems.Add(instance.GetComponentInChildren<UIItem>());
      uiItems[i].item = null;
    }
  }

  public void DeleteAllSlots() {
    EmptyAllSlots();
    slots.ForEach(slot => {
      if (slot != null) {
        Destroy(slot.gameObject);
      }
    });
    slots.Clear();
  }
  
  public void ClearSlot(int slot) {
    uiItems[slot].UpdateItem(null);
  }

  public void UpdateSlot(int slot, Item item) {
    uiItems[slot].UpdateItem(item);
  }

  public void AddItem(Item item) {
    UpdateSlot(uiItems.FindIndex(i => i.item == null), item);
  }

  public void RemoveItem(Item item) {
    UpdateSlot(uiItems.FindIndex(i => i.item == item), null);
  }

  public void EmptyAllSlots() {
    uiItems.ForEach(item => item.UpdateItem(null));
  }

  public bool ContainsEmptySlot() {
    foreach (UIItem uiItem in uiItems) {
      if (uiItem.item == null && !uiItem.isCraftingSlot) {
        return true;
      }
    }
    return false;
  }


}
