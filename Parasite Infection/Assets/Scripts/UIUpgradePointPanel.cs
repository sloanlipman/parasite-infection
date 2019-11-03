using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem;

public class UIUpgradePointPanel : MonoBehaviour {
  private UIPartyPanel partyPanel;
  private PartyMember partyMember;

  [SerializeField] Button upgradePointButton;
  private List<Button> buttonList = new List<Button>();


  private void Awake() {
    gameObject.SetActive(false);
    partyPanel = FindObjectOfType<UIPartyPanel>();
  }

  public void Populate() {
    partyMember = partyPanel.LookUpSelectedPartyMember();
    buttonList.ForEach(button => {
      Destroy(button.gameObject);
    });
    buttonList.Clear();

  foreach(var multiplier in partyMember.multipliers) {
    Button button = Instantiate<Button>(upgradePointButton, this.transform);
    buttonList.Add(button);
    UpdateButtonLabel(button, multiplier.Key.ToString(), multiplier.Value.ToString());
    button.GetComponent<Button>().onClick.AddListener(() => {
      string key = multiplier.Key.ToString();
      partyMember.multipliers[key]++;
      UpdateButtonLabel(button, key, partyMember.multipliers[key].ToString());
      partyMember.SpendUpgradePoint();
      gameObject.SetActive(partyMember.HasUpgradePointsToSpend());
    });
    }
  }

  private void UpdateButtonLabel(Button button, string property, string value) {
    button.GetComponentInChildren<Text>().text = string.Format("{0} ({1})", property, value);

  }
}
