using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem;

public class UIPartyPanel : MonoBehaviour {
  [SerializeField] private Button partyMemberButton;
  [SerializeField] private UIPlayerInfoPanel playerInfo;
  [SerializeField] private GameObject[] slots = new GameObject[]{};
  [SerializeField] private GameObject equipmentSlots;
  private UIInventory uiInventory;
  private Inventory inventory;
  private string selectedPartyMember;

  private List<Button> buttonList = new List<Button>();
  private List<GameObject> slotList = new List<GameObject>();
  [SerializeField] private SlotPanel slotPanel;
  private ItemDatabase itemDatabase;

  private List<PartyMember> party = new List<PartyMember>();
  private BattleSystem.CharacterController characterController;
  void Awake() {
    inventory = FindObjectOfType<Inventory>();
    uiInventory = FindObjectOfType<UIInventory>();
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    itemDatabase = FindObjectOfType<ItemDatabase>();
    ClearSlots();
  }

  public GameObject GetPlayerEquipmentPanel() {
    return equipmentSlots.gameObject;
  }

  public void ResetSelectedPartyMember() {
    selectedPartyMember = "";
  }

  public string GetSelectedPartyMember() {
    return selectedPartyMember;
  }

  void ClearSlots() {
    foreach (var slot in slots) {
      UIItem uiItem = slot.GetComponentInChildren<UIItem>();
      uiItem.UpdateItem(null);
    }
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
        uiInventory.gameObject.SetActive(true);
        if (selectedPartyMember != member.characterName) {
          Debug.LogWarning("Clicked on: " + member.characterName);
          AddPlayerEquipmentSlots(member);
        }
      });
    });
  }

  private void AddPlayerEquipmentSlots(PartyMember member) {
    if (!equipmentSlots.gameObject.activeSelf) {
      equipmentSlots.gameObject.SetActive(true);
    }
    if (!playerInfo.gameObject.activeSelf) {
      playerInfo.gameObject.SetActive(true);
    }
    Debug.LogWarning("# equipment: " + member.equipment.Length);

    ClearSlots();

// Set the right # of slots
    selectedPartyMember = member.characterName;
    int numberOfSlots = member.GetModSlots();
    for (int i = 0; i < slots.Length; i++) {
      if (i < numberOfSlots) {
        slots[i].SetActive(true);
      } else {
        slots[i].SetActive(false);
      }
    }
    for (int j = 0; j < member.equipment.Length; j++) {
      UIItem uiItem = slots[j].GetComponentInChildren<UIItem>();
      uiItem.UpdateItem(member.equipment[j]);
    }
    playerInfo.Populate(selectedPartyMember);
  }

  public void RemoveItem(int index) {
    PartyMember member = characterController.FindPartyMemberByName(selectedPartyMember);
    Item item = member.equipment[index];
    if (item != null) {
      member.equipment[index] = null;
    }
  }

  public void UpdatePartyMemberEquipment(Item item) {
    if (selectedPartyMember != null) {
      PartyMember member = characterController.FindPartyMemberByName(selectedPartyMember);

      for (int i = 0; i < slots.Length; i++) {
        if (slots[i].gameObject.activeSelf) {
          UIItem uiItem = slots[i].GetComponentInChildren<UIItem>();
          if (uiItem.item == item) {
            inventory.RemoveItem(item.index);
            member.equipment[i] = item;
            uiItem.item = member.equipment[i];
          }
        }
      }
    }
  }
}