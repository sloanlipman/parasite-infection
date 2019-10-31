using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
  public Item item;
  private Image spriteImage;
  private UIItem selectedItem;
  private CraftingSlots craftingSlots;
  private UIPartyPanel partyPanel;
  private EquipmentSlots equipmentSlots;
  private ConsumableInventory consumableInventory;
  private CraftingInventory craftingInventory;
  private InventoryController inventoryController;
  private Tooltip tooltip;
  public bool isCraftingSlot = false;
  public bool isCraftingResultSlot = false;
  public bool isPlayerEquipmentSlot = false;
  public bool isConsumableInventorySlot = false;
  public bool isCraftingInventorySlot;

  private void Awake() {
    craftingSlots = FindObjectOfType<CraftingSlots>();
    equipmentSlots = FindObjectOfType<EquipmentSlots>();
    partyPanel = FindObjectOfType<UIPartyPanel>();
    tooltip = FindObjectOfType<Tooltip>();
    selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
    spriteImage = GetComponent<Image>();
    consumableInventory = FindObjectOfType<ConsumableInventory>();
    craftingInventory = FindObjectOfType<CraftingInventory>();
    inventoryController = FindObjectOfType<InventoryController>();
    UpdateItem(null);
  }

  public void DirectlyNullifyItem() {
    this.item = null;
    SetSprite(null);
  }
  
  public void UpdateItem(Item item) {
    this.item = item;
    SetSprite(item);

    if (item != null && item.index < 0) {
      Debug.Log("Item did not have index. Getting index now.");
      inventoryController.AddToListOfCurrentItems(item);
      Debug.Log("Index is " + item.index);
    }

    if (isCraftingSlot) {
      craftingSlots.UpdateRecipe();
    }

    if (isPlayerEquipmentSlot) {
      if (item != null) {
        partyPanel.ParseUIForCurrentEquipment();
        craftingInventory.RemoveItem(item);
       }
    }

    if (item != null && isConsumableInventorySlot) {
      if (inventoryController.IsCraftingItem(item)) {
        UpdateItem(null);
        inventoryController.DeselectItem();
      }
    } else if (item != null && isCraftingInventorySlot) {
      if (!inventoryController.IsCraftingItem(item)) {
        UpdateItem(null);
        inventoryController.DeselectItem();
      }
    }
  }

  public void SetSprite(Item item) {
    if (this.item != null) {
      spriteImage.color = Color.white;
      spriteImage.sprite = item.icon;
    } else {
      spriteImage.color = Color.clear;
    }
  }

  private bool IsEquippable(UIItem item) {
    return inventoryController.IsEquippable(item.item);
  }

  public void OnPointerDown(PointerEventData eventData) {
    if (this.item != null) {
      Debug.Log("Item's index is: " + item.index);
      UICraftResult craftResult = GetComponent<UICraftResult>();
      if (craftResult != null && selectedItem.item == null) { // Successful craft
        craftResult.PickItem();
        selectedItem.UpdateItem(this.item);
        craftResult.ClearSlots();
      } else if (!isCraftingResultSlot) {
          if (selectedItem.item != null) {
            if (isPlayerEquipmentSlot) {
              if (IsEquippable(selectedItem)) {
                SwapItems();
              } else {
                inventoryController.DeselectItem();
              }
            } else {
              SwapItems();
            }
          } else {
            selectedItem.UpdateItem(this.item);
            UpdateItem(null);
        }
      }
    } else if (selectedItem.item != null && !isCraftingResultSlot) {
        if (isPlayerEquipmentSlot && !IsEquippable(selectedItem)) {
          Debug.Log("is equip slot && selected item is not equippable");
          inventoryController.DeselectItem();
        } else {
          UpdateItem(selectedItem.item);
          selectedItem.UpdateItem(null);
      }
    }
  }

  public void SwapItems() {
    // Swap the item you clicked with the item currently being dragged
    Item clone = new Item(selectedItem.item);
    selectedItem.UpdateItem(this.item);
    UpdateItem(clone);
  }

// Remove item from player's equipment slots
  public void OnPointerUp(PointerEventData eventData) {
  //   if (isPlayerEquipmentSlot && selectedItem.item != null) {
  //     GameObject[] slots = equipmentSlots.GetSlots();
  //     for (int i = 0; i < slots.Length; i++) {
  //       UIItem item = slots[i].GetComponentInChildren<UIItem>();
  //       if (this == item) {
  //         partyPanel.RemoveItem(i);
  //         break;
  //       }
  //     }
  //   }
  }

  public void OnPointerEnter(PointerEventData eventData) {
    if (this.item != null) {
      tooltip.GenerateTooltip(item);
    }
  }

  public void OnPointerExit(PointerEventData eventData) {
    tooltip.gameObject.SetActive(false);
  }

  public UIItem GetSelectedItem() {
    return selectedItem;
  }

  public void SetSelectedItem(Item item) {
    selectedItem.item = item;
  }
}
