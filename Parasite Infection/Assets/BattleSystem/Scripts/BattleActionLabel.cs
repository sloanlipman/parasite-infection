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

    public void ShowDefend(string defense, string EP, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.green;
      string defenseString = string.Format("Defense + {0}\nEP + {1}", defense, EP);
      StartCoroutine(ShowLabel(defenseString, target));
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

    public void ShowCureAll(string HP, string EP, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.cyan;
      string cureAllString = string.Format("HP + {0}\nEP + {1}", HP, EP);
      StartCoroutine(ShowLabel(cureAllString, target));
    }

    public void ShowDamage(string amount, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.red;
      string damageString = "HP - " + amount;
      StartCoroutine(ShowLabel(damageString, target));
    }

    public void ShowHydroblastEffects(string damage, string defense, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.blue;

      string hydroblastEffects = string.Format("HP - {0}\nDefense - {1}", damage, defense);
      StartCoroutine(ShowLabel(hydroblastEffects, target));
    }

    public void ShowFireballEffects(string damage, string EP, BattleCharacter target) {
      label.gameObject.SetActive(true);
      label.color = Color.magenta;
      string fireballEffects = string.Format("HP - {0}\nEP - {1}", damage, EP);
      StartCoroutine(ShowLabel(fireballEffects, target));
    }
  }
}
