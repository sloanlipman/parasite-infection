using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour {

  [SerializeField] private GameObject abilityPanel;
  [SerializeField] private Button[] actionButtons;
  [SerializeField] private Button button;
  [SerializeField] private Text[] characterInfo;

  void Start() {
    abilityPanel.SetActive(false);
    UpdateCharacterUI();
  }

  void Update () {
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
      if (hitInfo.collider != null && hitInfo.collider.CompareTag("Character")) {
        BattleController.Instance.SelectTarget(hitInfo.collider.GetComponent<Character>());
      }
    }
	}

  public void ToggleActionState(bool state) {
    // ToggleAbilityPanel(state);
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

  public void UpdateCharacterUI() {
    for (int i = 0; i < BattleController.Instance.GetPlayerList().Count; i++) {
      Character character = BattleController.Instance.GetPlayer(i);
      characterInfo[i].text = string.Format("{0} hp: {1}/{2}, ep: {3}/{4}", character.characterName, character.health, character.maxHealth, character.energyPoints, character.maxEnergyPoints);
    }
  }

  public void BuildAbilityList(List<Ability> abilities) {
    if (abilityPanel.transform.childCount > 0) {
      foreach(Button button in abilityPanel.transform.GetComponentsInChildren<Button>()) {
        Destroy(button.gameObject);
      }
    }

    foreach (Ability ability in abilities) {
      Button abilityButton = Instantiate<Button>(button, abilityPanel.transform);
      abilityButton.GetComponentInChildren<Text>().text = ability.abilityName;
      abilityButton.onClick.AddListener(() => SelectAbility(ability));
    }
  }

  void SelectAbility(Ability ability) {
    BattleController.Instance.abilityToBeUsed = ability;
    BattleController.Instance.playerIsAttacking = false;
  }

  public void SelectAttack() {
    Debug.Log("Attack!!!!");
    BattleController.Instance.abilityToBeUsed = null;
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
}
