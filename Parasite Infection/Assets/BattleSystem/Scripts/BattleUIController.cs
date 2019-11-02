using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem {
  public class BattleUIController : MonoBehaviour {

    [SerializeField] private GameObject abilityPanel;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private Button[] actionButtons;
    [SerializeField] private Button button;
    [SerializeField] private List<Text> characterInfo;
    [SerializeField] private Transform characterInfoParent;
    [SerializeField] private Text characterInfoText;


    void Start() {
      abilityPanel.SetActive(false);
      itemPanel.SetActive(false);
      GenerateCharacterUI();
      SetColor(0, Color.red);
      UpdateCharacterUI();
    }

    void Update () {
      if (Input.GetMouseButtonDown(0)) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
        if (hitInfo.collider != null && hitInfo.collider.CompareTag("Character")) {
          BattleController.Instance.SelectTarget(hitInfo.collider.GetComponent<BattleCharacter>());
        }
      }
    }

    public void ResetCharacterInfo() {
      foreach(Text text in characterInfoParent.GetComponentsInChildren<Text>()) {
        Destroy(text.gameObject);
      }
      characterInfo.Clear();
      GenerateCharacterUI();
    }

    public void ToggleActionState(bool state) {
      foreach(Button button in actionButtons) {
        button.interactable = state;
      }
    }

    public void ToggleAbilityPanel(bool state) {
      abilityPanel.SetActive(state);
      if (state) {
        BuildAbilityList(BattleController.Instance.GetCurrentCharacter().abilities);
      }
    }

    public void ToggleItemPanel(bool state) {
      itemPanel.SetActive(state);
      if (state) {
        BuildItemList();
      }
    }

    public void GenerateCharacterUI() {
      List<BattleCharacter> players = BattleController.Instance.GetListOfAlivePlayers();
      for (int i = 0; i < players.Count; i++) {
        Text infoToAdd = Instantiate(characterInfoText, characterInfoParent);
        characterInfo.Add(infoToAdd);
        SetColor(i, Color.white);
      }
    }

    public void UpdateCharacterUI() {
      int currentPlayerIndex = BattleController.Instance.characterTurnIndex;
      for (int i = 0; i < BattleController.Instance.GetListOfAlivePlayers().Count; i++) {
        BattleCharacter character = BattleController.Instance.GetPlayer(i);
        characterInfo[i].text = string.Format("{0} hp: {1}/{2}, ep: {3}/{4}", character.characterName, character.health, character.maxHealth, character.energyPoints, character.maxEnergyPoints);
      }
    }

    private void ClearAbilityPanel() {
      if (abilityPanel.transform.childCount > 0) {
        foreach(Button button in abilityPanel.transform.GetComponentsInChildren<Button>()) {
          Destroy(button.gameObject);
        }
      }
    }

    private void ClearItemPanel() {
      if (abilityPanel.transform.childCount > 0) {
        foreach(Button button in itemPanel.transform.GetComponentsInChildren<Button>()) {
          Destroy(button.gameObject);
        }
      }
    }

    public void BuildItemList() {
      ClearItemPanel();
      ConsumableInventory items = BattleController.Instance.items;
      if (items.playerItems.Count > 0) {
        foreach (Item item in items.playerItems) {
          Button itemButton = Instantiate<Button>(button, itemPanel.transform);
          itemButton.GetComponentInChildren<Text>().text = item.itemName;
          itemButton.onClick.AddListener(() => SelectItem(item));
        }
      }
    }

    public void BuildAbilityList(List<Ability> abilities) {
      ClearAbilityPanel();

    if (abilities != null) {
      foreach (Ability ability in abilities) {
        Button abilityButton = Instantiate<Button>(button, abilityPanel.transform);
        abilityButton.GetComponentInChildren<Text>().text = ability.abilityName;
        abilityButton.onClick.AddListener(() => SelectAbility(ability));
       }
      }
    }

    void SelectItem(Item item) {
      Debug.Log("Selected " + item.itemName);
      BattleController.Instance.itemToBeUsed = item;
      BattleController.Instance.abilityToBeUsed = null;
      BattleController.Instance.playerIsAttacking = false;
    }

    void SelectAbility(Ability ability) {
      BattleController.Instance.abilityToBeUsed = ability;
      BattleController.Instance.itemToBeUsed = null;
      BattleController.Instance.playerIsAttacking = false;
    }

    public void SelectAttack() {
      BattleController.Instance.abilityToBeUsed = null;
      BattleController.Instance.itemToBeUsed = null;
      BattleController.Instance.playerIsAttacking = true;
    }

    public void Defend() {
      BattleController.Instance.GetCurrentCharacter().Defend();
      BattleController.Instance.NextAct();
      Deselect();
    }

    public void Deselect() {
      GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public void SetColor(int index, Color color) {
      characterInfo[index].color = color;
    }
  }
}