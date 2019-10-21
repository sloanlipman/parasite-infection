using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem;

public class UIPartyPanel : MonoBehaviour {
  [SerializeField] private Button partyMemberButton;
  private List<Button> buttonList = new List<Button>();
  private List<PartyMember> party = new List<PartyMember>();
  private BattleSystem.CharacterController characterController;
  void Awake() {
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
  }

  public void Populate() {
    buttonList.ForEach(button => {
      Destroy(button.gameObject);
    });
    buttonList.Clear();
    party = characterController.GetActiveParty();
    party.ForEach(member => {
      Button button = Instantiate<Button>(partyMemberButton, this.transform);
      buttonList.Add(button);
      button.GetComponentInChildren<Text>().text = member.characterName;
    });

  }
}
