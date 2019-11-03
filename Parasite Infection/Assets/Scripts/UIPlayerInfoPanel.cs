using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoPanel : MonoBehaviour {
  [SerializeField] private Text characterName;
  [SerializeField] private Text hp;
  [SerializeField] private Text ep;
  [SerializeField] private Text level;
  [SerializeField] private Text levelUpIn;
  [SerializeField] private UIPartyPanel partyPanel;
  [SerializeField] private SpendUpgradePointsPanel spendUpgradePointsPanel;


  private BattleSystem.PartyMember partyMember;
  // private string partyMemberName;
  private BattleSystem.CharacterController characterController;

  public void Populate(string partyMemberName) {
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    partyMemberName = partyPanel.GetSelectedPartyMember();
    partyMember = characterController.FindPartyMemberByName(partyMemberName);
    characterName.text = "Name: " + partyMember.characterName;
    hp.text = string.Format("HP: {0}/{1}", partyMember.health, partyMember.maxHealth);
    ep.text = string.Format("Energy: {0}/{1}", partyMember.energyPoints, partyMember.maxEnergyPoints);
    level.text = "Level: " + partyMember.level;
    levelUpIn.text = "Level Up In: " + characterController.NextLevel(partyMember.level);
  }

  void Update() {
    bool hasUpgradePoints = partyMember.HasUpgradePointsToSpend();
    spendUpgradePointsPanel.GetComponentInChildren<Text>().text = hasUpgradePoints? "Spend Upgrade Points (" + partyMember.GetUpgradePoints() + ")" : "View Upgrade Points";
  }
}
