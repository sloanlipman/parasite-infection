using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleSystem;

public class UIPartyPanel : MonoBehaviour {
  [SerializeField] private Button partyMemberButton;
  [SerializeField] private UIPlayerInfoPanel playerInfo;
  [SerializeField] private GameObject[] slots = new GameObject[]{};
  [SerializeField] private GameObject equipmentSlots;
  [SerializeField] UIUpgradePointPanel upgradePointPanel;
  private CraftingInventory craftingInventory;
  private ConsumableInventory consumableInventory;
  private InventoryController inventoryController;
  private MenuController menuController;

  private string selectedPartyMember;

  private List<Button> buttonList = new List<Button>();
  private List<GameObject> slotList = new List<GameObject>();
  [SerializeField] private SlotPanel slotPanel;
  private ItemDatabase itemDatabase;

  private List<PartyMember> party = new List<PartyMember>();
  private BattleSystem.CharacterController characterController;
  void Awake() {
    craftingInventory = FindObjectOfType<CraftingInventory>();
    consumableInventory = FindObjectOfType<ConsumableInventory>();
    characterController = FindObjectOfType<BattleSystem.CharacterController>();
    inventoryController = FindObjectOfType<InventoryController>();
    itemDatabase = FindObjectOfType<ItemDatabase>();
    menuController = FindObjectOfType<MenuController>();
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

  public PartyMember LookUpSelectedPartyMember() {
    return selectedPartyMember != "" ?  characterController.FindPartyMemberByName(selectedPartyMember) : null;
  }

  public void ClearPartyMember() {
    selectedPartyMember = null;
  }

  void ClearSlots() {
    foreach (var slot in slots) {
      UIItem uiItem = slot.GetComponentInChildren<UIItem>();
      uiItem.UpdateItem(null);
    }
  }

  bool AreEquipmentSlotsOpen() {
    return equipmentSlots.gameObject.activeSelf;
  }

  bool IsPlayerInfoOpen() {
    return playerInfo.gameObject.activeSelf;
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
          if (AreEquipmentSlotsOpen()) {
            upgradePointPanel.gameObject.SetActive(false);
            ParseUIForCurrentEquipment(); // If another party member's equipment is open, save the currenytly opened one
          }
          menuController.OpenPlayerInfo(member);
        }
      });
    });
  }

  public void AddPlayerEquipmentSlots(PartyMember member) {
    if (!AreEquipmentSlotsOpen()) {
      equipmentSlots.gameObject.SetActive(true);
    }
    if (!IsPlayerInfoOpen()) {
      playerInfo.gameObject.SetActive(true);
    }

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
    RestoreUIForCurrentEquipment();
    playerInfo.Populate(selectedPartyMember);
  }

  public void RemoveItem(int index) {
    PartyMember member = LookUpSelectedPartyMember();
    Item item = member.equipment[index];
    if (item != null) {
      member.equipment[index] = null;
    }
  }

// Save the current items
  public void ParseUIForCurrentEquipment() {
      PartyMember member = LookUpSelectedPartyMember();
      if (member != null) {
        for (int i = 0; i < slots.Length; i++) {
          UIItem uiItem = slots[i].GetComponentInChildren<UIItem>(true);
          member.equipment[i] = null; // Clear the slot
            if (uiItem.item != null) { // The item has been swapped out
              member.equipment[i] = uiItem.item; // Set as item
            }
          }
        }
    }

// Load the previous items
  public void RestoreUIForCurrentEquipment() {
      PartyMember member = LookUpSelectedPartyMember();
      if (member != null) {
      for (int i = 0; i < slots.Length; i++) {
        UIItem uiItem = slots[i].GetComponentInChildren<UIItem>(true);
        if (member.equipment[i] != null) { // The item has been swapped out
          uiItem.item = member.equipment[i]; // Set as item
          uiItem.SetSprite(uiItem.item);
        } else {
          uiItem.DirectlyNullifyItem();
        }
      }
    }
  }
}