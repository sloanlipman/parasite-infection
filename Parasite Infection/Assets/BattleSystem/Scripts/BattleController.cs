using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleController : MonoBehaviour {

    public static BattleController Instance { get; set; }
    private CharacterController characterController;
    [SerializeField] private BattleUIController uiController;
    [SerializeField] private BattleSpawnPoint[] spawnPoints;
    private BattleSummaryPanel battleSummaryPanel;
    private UIInventory inventoryPanel;
    private QuestSystem.QuestPanel questPanel;

    public Dictionary<int, List<BattleCharacter>> characters = new Dictionary<int, List<BattleCharacter>>();
    public int characterTurnIndex;
    public Ability abilityToBeUsed;
    public bool playerIsAttacking;
    private bool didEnemyUseAbility;

    private int actTurn; // act refers to Action (i.e. who is up on the current side?)
                        // Turn refers to are Enemies going or are players going?

    private int xpToReward;

    public bool IsItPlayerTurn() {
      return actTurn == 0;
    }

    private bool IsCharacterAPlayer(BattleCharacter character) {
      return GetPlayerList().Contains(character);
    }

    private bool IsAPlayerAlive() {
      return GetPlayerList().Count > 0;
    }

    private bool IsAnEnemyAlive() {
      return GetEnemyList().Count > 0;
    }

    public bool IsBattleActive() {
      return IsAnEnemyAlive() && IsAnEnemyAlive();
    }

    public List<BattleCharacter> GetPlayerList() {
      return characters[0];
    }

    public BattleCharacter GetPlayer(int index) {
      return GetPlayerList()[index];
    }

    public List<BattleCharacter> GetEnemyList() {
      return characters[1];
    }

    public BattleCharacter GetSelectedEnemy(int selectedEnemyIndex) {
      return GetEnemyList()[selectedEnemyIndex];
    }

    public BattleCharacter GetCurrentCharacter() {
      return characters[actTurn][characterTurnIndex];
    }

    private bool HasACharacterNotGone() {
      return characterTurnIndex < characters[actTurn].Count - 1;
    }

    private void HideMenus() {
      questPanel = FindObjectOfType<QuestSystem.QuestPanel>();
      inventoryPanel = FindObjectOfType<UIInventory>();
      battleSummaryPanel = FindObjectOfType<BattleSummaryPanel>();
      if (questPanel != null) {
        questPanel.gameObject.SetActive(false);
      }

      if (inventoryPanel != null) {
        inventoryPanel.gameObject.SetActive(false);
      }

      if (battleSummaryPanel != null) {
        battleSummaryPanel.gameObject.SetActive(false);
      }
    }

    private void Awake() {
      characterController = FindObjectOfType<CharacterController>();
      EventController.OnEnemyDied += AddEnemyExperience;
      HideMenus();
    }

    // Start is called before the first frame update
    private void Start() {
      if (Instance != null & Instance != this) {
        Destroy(this.gameObject);
      }
      else {
        Instance = this;
      }

      characters.Add(0, new List<BattleCharacter>()); // players
      characters.Add(1, new List<BattleCharacter>()); // enemies

      FindObjectOfType<BattleLauncher>().Launch();
    }

    public void StartBattle(List<PartyMember> players, List<Enemy> enemies) {
      for (int i = 0; i < players.Count; i++) {
        GetPlayerList().Add(spawnPoints[i+3].Spawn(players[i])); // Add Players to spawn points 3-5
        GetPlayerList()[i].abilities.Clear();
        for (int j = 0; j < GetPlayerList()[i].abilitiesList.Count; j++) {
          GetPlayerList()[i].abilities.Add(characterController.GetAbility(GetPlayerList()[i].abilitiesList[j]));
        }
      }


      for (int i = 0; i < enemies.Count; i++) {
        GetEnemyList().Add(spawnPoints[i].Spawn(enemies[i])); // Add Enemies to spawn points 0-2
        GetEnemyList()[i].abilities.Clear();
        for (int j = 0; j < GetEnemyList()[i].abilitiesList.Count; j++) {
          GetEnemyList()[i].abilities.Add(characterController.GetAbility(GetEnemyList()[i].abilitiesList[j]));
        }
      }
    }

    public BattleCharacter GetRandomPlayer() {
      List<BattleCharacter> playerList = GetPlayerList();
      return playerList[Random.Range(0, playerList.Count - 1)];
    }

    public BattleCharacter GetWeakestEnemy() {
      List<BattleCharacter> enemyList = GetEnemyList();
      BattleCharacter weakestEnemy = enemyList[0];
      foreach (BattleCharacter enemy in enemyList) {
          if (enemy.health < weakestEnemy.health) {
            weakestEnemy = enemy;
          }
      }
      return weakestEnemy;
    }

    private void NextTurn() {
      actTurn = actTurn == 0? 1 : 0; // Swap between Players & Enemies
      if (IsItPlayerTurn()) {
        uiController.SetColor(0, Color.red);
      } else {
        List<BattleCharacter> playerList = GetPlayerList();
        for (int i = 0; i < playerList.Count; i++) {
          uiController.SetColor(i, Color.white);
        }
      }
    }

    public void NextAct() {
      if (IsItPlayerTurn()) {
        uiController.SetColor(characterTurnIndex, Color.white);
        }
      BattleCharacter currentCharacter = GetCurrentCharacter();
      if (IsCharacterAPlayer(currentCharacter) && abilityToBeUsed == null) {
        int energyToRecover = (int)Mathf.Round(0.1f * currentCharacter.maxEnergyPoints);
        currentCharacter.RecoverEnergy(energyToRecover);
      }
      uiController.UpdateCharacterUI();
      uiController.ToggleAbilityPanel(false);

      if (IsAPlayerAlive() && IsAnEnemyAlive()) {
          abilityToBeUsed = null;
          playerIsAttacking = false;
        if (HasACharacterNotGone()) {
          characterTurnIndex++;
          if (IsItPlayerTurn()) {
            uiController.SetColor(characterTurnIndex, Color.red);
          }
        }
        else {
          NextTurn();
          characterTurnIndex = 0;
        }
        switch(actTurn) {
          case 0: {
            uiController.ToggleActionState(true);
            uiController.BuildAbilityList(GetCurrentCharacter().abilities);
            break;
          }

          case 1: {
            StartCoroutine(PerformAct()); // AI takes over for enemies
            uiController.ToggleActionState(false);
            break;
          }
        }
      }
      else {
        Debug.LogWarning("Battle is over!");
        Debug.Log("Experience gained: " + xpToReward);
        if (IsAPlayerAlive()) {
          battleSummaryPanel.ShowVictoryPanel(xpToReward);
          characterController.UpdatePlayers(GetPlayerList(), xpToReward);
        } else {
          battleSummaryPanel.ShowDefeatPanel();
        }
      }
    }

    IEnumerator PerformAct() {
      yield return new WaitForSeconds(0.75f);
      if (GetCurrentCharacter().health > 0) { // actTurn should always be 1 for enemies here!
      Enemy enemy = GetCurrentCharacter() as Enemy;
      bool didEnemyUseAbility = enemy.Act();
      if (!didEnemyUseAbility) {
        int energyToRecover = (int)Mathf.Round(0.1f * GetCurrentCharacter().maxEnergyPoints);
        GetCurrentCharacter().RecoverEnergy(energyToRecover);
        }
      }
      uiController.UpdateCharacterUI();
      yield return new WaitForSeconds(1f);
      NextAct();
    }

    public void SelectTarget(BattleCharacter target) {
      if (playerIsAttacking) {
        if (IsCharacterAPlayer(target)) {
          Debug.LogWarning("Don't target your own team!");
        } else {
        DoAttack(GetCurrentCharacter(), target);
        }
      }
      else if (abilityToBeUsed != null) {
        if (abilityToBeUsed.abilityType == Ability.AbilityType.Heal) { // TODO Could add support spells here too
          if (!IsCharacterAPlayer(target) || !(target.IsCharacterDamaged())) {
            Debug.LogWarning("You can't heal your enemies, or your health is already maxed out!!");
            return;
          }
        } else if (abilityToBeUsed.abilityType == Ability.AbilityType.Attack) { // TODO Could add debuff spells here too
            if (IsCharacterAPlayer(target)) {
              Debug.LogWarning("Don't target your own team!");
              return;
            }
        }
        if (GetCurrentCharacter().UseAbility(abilityToBeUsed, target)) {
          uiController.UpdateCharacterUI();
          NextAct();
        }
        else {
          Debug.LogWarning("Not enough Energy to cast that Ability!");
        }
      }
    }

    public void DoAttack(BattleCharacter attacker, BattleCharacter target) {
      Debug.Log(attacker.characterName + " is attacking " + target.characterName);
      target.Hurt(attacker.attackPower);
      NextAct();
    }
    private void AddEnemyExperience(int enemyId) {
      Enemy killedEnemy = characterController.FindEnemyById(enemyId);
      xpToReward += killedEnemy.experience;
   }
  }
}