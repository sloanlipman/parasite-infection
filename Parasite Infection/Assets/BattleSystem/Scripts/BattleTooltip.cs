using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem {

  public class BattleTooltip : MonoBehaviour {
    private Text tooltipText;
    private Image panel;

    // Start is called before the first frame update
    void Start() {
      tooltipText = GetComponentInChildren<Text>();
      panel = GetComponentInChildren<Image>();
      HideTooltip();
    }

    public void GenerateTooltip (Ability ability) {
      PersistTooltip();
      string tooltip = string.Format("EP Cost: {0}\n{1}", ability.energyCost, ability.description);
      tooltipText.text = tooltip;
    }

    public void GenerateTooltip (Item item) {
      PersistTooltip();
      string tooltip = "";
      string health = "";
      string energy = "";
      foreach(var stat in item.stats) {
        if (stat.Key.ToString() == "Health") {
          health = string.Format("Restores {0} Health", stat.Value.ToString());
        } else if (stat.Key.ToString() == "Energy") {
          energy = string.Format("Restores {0} Energy", stat.Value.ToString());
        }
      }

      if (health.Length > 0 && energy.Length > 0) {
        tooltip = string.Format("{0}\n{1}", health, energy);
      } else if (health.Length > 0 && energy.Length < 1) {
        tooltip = health;
      } else if (energy.Length > 0) {
        tooltip = energy;
      }
      tooltipText.text = tooltip;
    }


    public void GenerateTooltip (string tooltip) {
      PersistTooltip();
      tooltipText.text = tooltip;
    }

    public void GenerateAutoDismissTooltip (string tooltip) {
      ShowTooltip();
      tooltipText.text = tooltip;
    }

    private void PersistTooltip() {
      tooltipText.gameObject.SetActive(true);
      panel.gameObject.SetActive(true);
    }

    private void ShowTooltip() {
      tooltipText.gameObject.SetActive(true);
      panel.gameObject.SetActive(true);
      StartCoroutine(DismissTooltip());
    }

    public void HideTooltip() {
      tooltipText.gameObject.SetActive(false);
      panel.gameObject.SetActive(false);
    }

    IEnumerator DismissTooltip() {
      yield return new WaitForSeconds(2f);
      HideTooltip();
    }
  }
}