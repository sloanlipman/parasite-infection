using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoPanel : MonoBehaviour {
  [SerializeField] private Text characterName;
  [SerializeField] private Text hp;
  [SerializeField] private Text ep;
  [SerializeField] private Text attack;
  [SerializeField] private Text defense;
  [SerializeField] private Text level;
  [SerializeField] private Text levelUpIn;
  [SerializeField] private UIPartyPanel partyPanel;
  [SerializeField] private SpendUpgradePointsPanel spendUpgradePointsPanel;


  private BattleSystem.PartyMember partyMember;
  private BattleSystem.CharacterController characterController;

  public void Populate(string partyMemberName) {
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    partyMemberName = partyPanel.GetSelectedPartyMember();
    partyMember = characterController.FindPartyMemberByName(partyMemberName);
    characterName.text = "Name: " + partyMember.characterName;
    hp.text = string.Format("HP: {0}/{1}", partyMember.health, partyMember.maxHealth);
    ep.text = string.Format("Energy: {0}/{1}", partyMember.energyPoints, partyMember.maxEnergyPoints);

    attack.text = string.Format("Attack Power {0}", partyMember.attackPower);
    defense.text = string.Format("Defense: {0}", partyMember.defensePower);

    level.text = "Level: " + partyMember.level;

    int levelUpInAmount = (characterController.NextLevel(partyMember.level) - partyMember.experience);
    if (levelUpInAmount == 0) {
      levelUpInAmount = (characterController.NextLevel(partyMember.level + 1) - partyMember.experience);
    }
    levelUpIn.text = "Level Up In: " + levelUpInAmount;
  }

  void Update() {
    if (partyMember != null) {
      bool hasUpgradePoints = partyMember.HasUpgradePointsToSpend();
      spendUpgradePointsPanel.GetComponentInChildren<Text>(true).text = hasUpgradePoints? "Spend Upgrade Points (" + partyMember.GetUpgradePoints() + ")" : "View Upgrade Points";
    }
  }
}
