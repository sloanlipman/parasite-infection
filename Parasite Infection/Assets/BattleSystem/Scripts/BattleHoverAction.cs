using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BattleSystem {
  public class BattleHoverAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Ability ability;
    private Item item;
    private Button button;
    private string buttonText;
    private CharacterController characterController;
    private BattleTooltip tooltip;
    private InventoryController inventoryController;

    private void Start() {
      characterController = FindObjectOfType<CharacterController>();
      inventoryController = FindObjectOfType<InventoryController>();
      BattleTooltip[] tooltips = FindObjectsOfType<BattleTooltip>();
      foreach (BattleTooltip t in tooltips) {
        if (t.tag == "Bottom Tooltip") {
          this.tooltip = t;
        }
      }

      button = GetComponent<Button>();
      buttonText = button.GetComponentInChildren<Text>().text;

      ability = characterController.GetAbility(buttonText);
      if (ability == null) {
        item = inventoryController.GetItem(buttonText);
      }
    }

    public void OnPointerEnter(PointerEventData eventData) {
      if (this.ability != null) {
        tooltip.GenerateTooltip(this.ability);
      } else if (this.item != null) {
        tooltip.GenerateTooltip(this.item);
      }
    }

    public void OnPointerExit(PointerEventData eventData) {
      tooltip.HideTooltip();
    }
  }
}