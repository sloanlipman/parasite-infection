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
      tooltipText.gameObject.SetActive(false);
      panel.gameObject.SetActive(false);
    }

    public void GenerateTooltip (Ability ability, Vector2 position) {
      tooltipText.gameObject.SetActive(true);
      panel.gameObject.SetActive(true);

      string tooltip = string.Format("<b>{0}</b>\nEP Cost:{1}\n{2}", ability.abilityName, ability.energyCost, ability.description);
      tooltipText.text = tooltip;
    }

    public void HideTooltip() {
      tooltipText.gameObject.SetActive(false);
      panel.gameObject.SetActive(false);
    }
  }
}