using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem;

public class UIPartyPanel : MonoBehaviour {
  [SerializeField] private Button partyMemberButton;
  [SerializeField] private GameObject playerEquipment;
  [SerializeField] private UIPlayerInfoPanel playerInfo;
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
        Debug.Log("Selected member is: " + selectedPartyMember);
        Debug.Log("Clicked on: " + member.characterName);
        if (selectedPartyMember != member.characterName) {
          AddPlayerEquipmentSlots(member);
        }
      });
    });

  }

  private void AddPlayerEquipmentSlots(PartyMember member) {
    Debug.Log("setting up green panels for: " + member.characterName);
    selectedPartyMember = member.characterName;
    // slotList.ForEach(slot => {
    //   Destroy(slot.gameObject);
    // });
    // slotList.Clear();
    // if (slotPanel != null) {
    //   slotPanel.DeleteAllSlots();
    // }
    if (!playerEquipment.gameObject.activeSelf) {
      playerEquipment.SetActive(true);
    }
    if (!playerInfo.gameObject.activeSelf) {
      playerInfo.gameObject.SetActive(true);
    }
    playerInfo.Populate(selectedPartyMember);

    // slotPanel = Instantiate(Resources.Load("Prefabs/PlayerEquipmentPanel"), playerEquipment.transform) as SlotPanel; 

    // slotPanel.numberOfSlots = member.GetModSlots();
    // slotPanel.gameObject.SetActive(true);
    // slotPanel.SetUpSlots();
  }
}
