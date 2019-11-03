using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
  private Text tooltipText;

  void Start() {
    tooltipText = GetComponentInChildren<Text>();
    gameObject.SetActive(false);
  }

  public void GenerateTooltip(Item item) {
    string statText = "";
    foreach(var stat in item.stats) {
      if (
        stat.Key.ToString() != "CoreID" ||
        stat.Key.ToString() != "Energy" ||
        stat.Key.ToString() != "Health"
      ) {
        if (stat.Key.ToString() == "Crafting" || stat.Key.ToString() == "Equippable") {
          statText += "\n" + TranslateValue(stat.Key, stat.Value);
        } else {
          statText += "\n" + stat.Key.ToString() + ": " + TranslateValue(stat.Key, stat.Value);
        }
      }
    }
    string tooltip = string.Format("<b>{0}</b>\n{1}\n{2}", item.itemName, item.description, statText);
    tooltipText.text = tooltip;
    gameObject.SetActive(true);
  }

  private string TranslateValue(string key, int value) {
    switch (key) {
      case "Ability": {
        switch(value) {
          case 1: { return "Barrage"; }
          case 2: { return "Fireball"; }
          case 3: { return "Hydroblast"; }
          case 4: { return "Heal"; }
          default: { return "Ability Unknown";}
        }
      }

      case "Crafting": {
        switch(value) {
          case 1: { return "Crafting Item"; }
          default: { return "Consumable Item"; }
        }
      }

      case "Equippable": {
        switch(value) {
          case 1: { return "Equippable Item"; }
          default: { return "Non-equppable Item"; }
        }
      }
      default: { return value.ToString(); } 
    }
  }
}
