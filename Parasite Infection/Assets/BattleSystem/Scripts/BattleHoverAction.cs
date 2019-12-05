using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BattleSystem {
  public class BattleHoverAction : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler {
    private Ability ability;
    private Button button;
    private string abilityName;
    private CharacterController characterController;
    private BattleTooltip tooltip;

    private void Start() {
      characterController = FindObjectOfType<CharacterController>();
      tooltip = FindObjectOfType<BattleTooltip>();

      button = GetComponent<Button>();
      abilityName = button.GetComponentInChildren<Text>().text;

      ability = characterController.GetAbility(abilityName);
    }

    public void OnPointerEnter(PointerEventData eventData) {
      if (this.ability != null) {
        tooltip.GenerateTooltip(this.ability, eventData.position);
      }
    }

    public void OnPointerExit(PointerEventData eventData) {
      tooltip.HideTooltip();
    }
  }
}