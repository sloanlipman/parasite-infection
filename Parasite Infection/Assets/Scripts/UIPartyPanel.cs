using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem;

public class UIPartyPanel : MonoBehaviour {
  [SerializeField] private Button partyMemberButton;
  [SerializeField] private GameObject playerEquipment;
  [SerializeField] private UIPlayerInfoPanel playerInfo;
  [SerializeField] private GameObject[] slots = new GameObject[]{};
  private string selectedPartyMember;

  private List<Button> buttonList = new List<Button>();
  private List<GameObject> slotList = new List<GameObject>();
  [SerializeField] private SlotPanel slotPanel;
  private List<PartyMember> party = new List<PartyMember>();
  private BattleSystem.CharacterController characterController;
  void Awake() {
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
  }

  public GameObject GetPlayerEquipmentPanel() {
    return playerEquipment;
  }

  public void ResetActiveEquipment() {
    selectedPartyMember = "";
  }

  public string GetSelectedPartyMember() {
    return selectedPartyMember;
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
      button.GetComponent<Button>().onClick.AddListener(() => {
        if (selectedPartyMember != member.characterName) {
          AddPlayerEquipmentSlots(member);
        }
      });
    });

  }

  private void AddPlayerEquipmentSlots(PartyMember member) {
    selectedPartyMember = member.characterName;
    int numberOfSlots = member.GetModSlots();
    if (!playerEquipment.gameObject.activeSelf) {
      playerEquipment.SetActive(true);
    }
    if (!playerInfo.gameObject.activeSelf) {
      playerInfo.gameObject.SetActive(true);
    }

    for (int i = 0; i < slots.Length; i++) {
      if (i < numberOfSlots) {
        slots[i].SetActive(true);
      } else {
        slots[i].SetActive(false);
      }
    }
    playerInfo.Populate(selectedPartyMember);
  }
}
