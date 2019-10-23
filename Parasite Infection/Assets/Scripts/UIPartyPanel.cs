using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem;

public class UIPartyPanel : MonoBehaviour {
  [SerializeField] private Button partyMemberButton;
  [SerializeField] private UIPlayerInfoPanel playerInfo;
  [SerializeField] private GameObject[] slots = new GameObject[]{};
  [SerializeField] private GameObject equipmentSlots;
  private string selectedPartyMember;

  private List<Button> buttonList = new List<Button>();
  private List<GameObject> slotList = new List<GameObject>();
  [SerializeField] private SlotPanel slotPanel;
  private ItemDatabase itemDatabase;

  private List<PartyMember> party = new List<PartyMember>();
  private BattleSystem.CharacterController characterController;
  void Awake() {
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    itemDatabase = FindObjectOfType<ItemDatabase>();
  }

  public GameObject GetPlayerEquipmentPanel() {
    return equipmentSlots.gameObject;
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
    if (!equipmentSlots.gameObject.activeSelf) {
      equipmentSlots.gameObject.SetActive(true);
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

  public void UpdatePlayerAbilities() {
    if (selectedPartyMember != null) {
      PartyMember member = characterController.FindPartyMemberByName(selectedPartyMember);
      member.abilities.Clear();
      member.abilitiesList.Clear();
      foreach(GameObject slot in slots) {
        if (slot.gameObject.activeSelf) {
          UIItem uiItem = slot.GetComponentInChildren<UIItem>();
          Item item = uiItem.item;
          string abilityToAdd;
          if (item.stats.ContainsKey("Ability")) {
            switch (item.stats["Ability"]) {
              case 1: {
                abilityToAdd = "Barrage";
                break;
              }
              
              case 2: {
                abilityToAdd = "Fireball";
                break;
              }
              case 3: {
                abilityToAdd = "Hydroblast";
                break;
              }
              case 4: {
                abilityToAdd = "Heal";
                break;
              }
              default: {
                abilityToAdd = null;
                break;
              }
            }

            if (abilityToAdd != null && !member.abilitiesList.Contains(abilityToAdd)) {
              member.abilitiesList.Add(abilityToAdd);
            }
          }
        }
      }
    }
  }
}