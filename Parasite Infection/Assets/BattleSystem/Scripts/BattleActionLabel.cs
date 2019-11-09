using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem {
  public class BattleActionLabel : MonoBehaviour {
    [SerializeField] private Text label;

    public void SetText(string labelToDisplay) {
      label.text = labelToDisplay;
    }

    public void SetPosition(Vector2 positionToShowLabel) {
      Vector2 finalPosition = new Vector2(positionToShowLabel.x + 1f, positionToShowLabel.y + 1f);
      label.rectTransform.position =  Camera.main.WorldToScreenPoint(finalPosition);
    }

    private IEnumerator ShowLabel(string amount, BattleCharacter target) {
      SetText(amount);
      SetPosition(target.transform.position);
      yield return new WaitForSeconds(.75f);
      label.gameObject.SetActive(false);

    }

    public void ShowDefend(string amount, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.green;
      string defenseString = "Defense + " + amount;
      StartCoroutine(ShowLabel(defenseString, target));
    }

    public void ShowAbilityDamage(string amount, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.yellow;
      string abilityString = "HP - " + amount;
      StartCoroutine(ShowLabel(abilityString, target));
    }

    public void ShowHeal(string amount, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.yellow;
      string healString = "HP + " + amount;
      StartCoroutine(ShowLabel(healString, target));
    }

    public void ShowEnergyRecovered(string amount, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.yellow;
      string energyString = "EP + " + amount;
      StartCoroutine(ShowLabel(energyString, target));
    }
    
    public void ShowAttackDamage(string amount, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.red;
      string damageString = "HP - " + amount;
      StartCoroutine(ShowLabel(damageString, target));
    }
  }
}