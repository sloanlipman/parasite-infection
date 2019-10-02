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
      if (stat.Key.ToString() != "CoreID") {
        statText += "\n" + stat.Key.ToString() + ": " + GetValue(stat.Key, stat.Value);
      }
    }
    string tooltip = string.Format("<b>{0}</b>\n{1}\n{2}", item.itemName, item.description, statText);
    tooltipText.text = tooltip;
    gameObject.SetActive(true);
  }

  private string GetValue(string key, int value) {
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
      // case "CoreID": {
      //   switch(value) {
      //     case 1: { return "Heavy"; }
      //     case 2: { return "Fire"; }
      //     case 3: { return "Water"; }
      //     case 4: { return "Medic"; }
      //     default: { return "Unknown CoreID"; }
      //   }
      // }
      default: { return value.ToString(); } 
    }
  }
}
