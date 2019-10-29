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

    if (isCraftingSlot) {
      craftingSlots.UpdateRecipe();
    }

    if (isPlayerEquipmentSlot) {
      if (item != null) {
        Debug.Log("Printing info before updating the equipment");
        partyPanel.PrintCurrentEquipment(100);
        Debug.Log("End of print");
        partyPanel.UpdatePartyMemberEquipment(item);
        partyPanel.PrintCurrentEquipment(200);
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

  public void OnPointerDown(PointerEventData eventData) {
    if (this.item != null) {
      UICraftResult craftResult = GetComponent<UICraftResult>();
      if (craftResult != null && selectedItem.item == null) { // Successful craft
        craftResult.PickItem();
        selectedItem.UpdateItem(this.item);
        craftResult.ClearSlots();
      } else if (!isCraftingResultSlot) {
          if (selectedItem.item != null) {
            if (isPlayerEquipmentSlot) {
              if (inventoryController.IsEquippable(selectedItem.item)) {
                Debug.Log("Selected item is equippable");
                SwapItems();
                Debug.Log("After the swap: Selected item is " + selectedItem.item.itemName);
                Debug.Log("After the swap: Item in slot is: " + this.item.itemName);
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
        if (isPlayerEquipmentSlot && !inventoryController.IsEquippable(selectedItem.item)) {
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
    if (isPlayerEquipmentSlot && selectedItem.item != null) {
        GameObject[] slots = equipmentSlots.GetSlots();
        for (int i = 0; i < slots.Length; i++) {
          UIItem item = slots[i].GetComponentInChildren<UIItem>();
          if (this == item) {
            partyPanel.RemoveItem(i);
            break;
          }
        }
        UpdateItem(null);
      }
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
