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
  private Tooltip tooltip;
  public bool isCraftingSlot = false;
  public bool isCraftingResultSlot = false;
  public bool isPlayerEquipmentSlot = false;
  public bool isConsumableInventorySlot = false;

  private void Awake() {
    craftingSlots = FindObjectOfType<CraftingSlots>();
    equipmentSlots = FindObjectOfType<EquipmentSlots>();
    partyPanel = FindObjectOfType<UIPartyPanel>();
    tooltip = FindObjectOfType<Tooltip>();
    selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
    spriteImage = GetComponent<Image>();
    UpdateItem(null);
  }
  
  public void UpdateItem(Item item) {
    this.item = item;
    SetSprite(item);

    if (isCraftingSlot) {
      craftingSlots.UpdateRecipe();
    }

    if (isPlayerEquipmentSlot) {
      if (item != null) {
        partyPanel.UpdatePartyMemberEquipment(item);
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
      Debug.Log("Item clicked was: " + this.item.itemName);
      UICraftResult craftResult = GetComponent<UICraftResult>();
      if (craftResult != null && this.item != null && selectedItem.item == null) { // Successful craft
        craftResult.PickItem();
        selectedItem.UpdateItem(this.item);
        craftResult.ClearSlots();
      } else if (!isCraftingResultSlot) {
          if (selectedItem.item != null) {
            // Swap the item you clicked with the item currently being dragged
            Item clone = new Item(selectedItem.item);
            selectedItem.UpdateItem(this.item);
            UpdateItem(clone);
          } else {
            selectedItem.UpdateItem(this.item);
            UpdateItem(null);
        }
      }
    } else if (selectedItem.item != null && !isCraftingResultSlot) {
      UpdateItem(selectedItem.item);
      selectedItem.UpdateItem(null);
    }
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
}
