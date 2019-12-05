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
      string tooltip = string.Format("<b>{0}</b>\nEP Cost:{1}\n{2}", ability.abilityName, ability.energyCost, ability.description);
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