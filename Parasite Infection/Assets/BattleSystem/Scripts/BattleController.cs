using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleController : MonoBehaviour {

    public static BattleController Instance { get; set; }
    public ConsumableInventory items;

    private CharacterController characterController;
    [SerializeField] private BattleUIController uiController;
    [SerializeField] private BattleSpawnPoint[] spawnPoints;
    [SerializeField] private BattleActionLabel labelText;
    private BattleSummaryPanel battleSummaryPanel;
    private UIInventory[] inventoryPanels;
    private QuestSystem.QuestPanel questPanel;
    private InventoryController inventoryController;

    private BattleCharacter currentTarget;

    public Dictionary<int, List<BattleCharacter>> characters = new Dictionary<int, List<BattleCharacter>>();
    private List<PartyMember> deadPlayers = new List<PartyMember>();
    private List<Enemy> deadEnemies = new List<Enemy>();
    public int characterTurnIndex;
    public Ability abilityToBeUsed;
    public Item itemToBeUsed;
    public bool playerIsAttacking;
    private bool didEnemyUseAbility;

    private string labelDamage;
    private string labelDefenseIncrease;
    private string labelHeal;
    private string labelEPRecovery;
    private string labelDefenseDecrease;
    private string labelEPDecrease;

    private int actTurn; // act refers to Action (i.e. who is up on the current side?)
                        // Turn refers to are Enemies going or are players going?

    private int xpToReward;
    private List<Item> itemsToGive = new List<Item>();

    public void SetCurrentTarget(BattleCharacter target) {
      currentTarget = target;
    }

    public void SetDamageLabel(int amount) {
      labelDamage = amount.ToString();
    }

    public void SetDefenseIncrease (int amount) {
      labelDefenseIncrease = amount.ToString();
    }

    public void SetEnergyRecoveryAmount (int amount) {
      labelEPRecovery = amount.ToString();
    }

    public void SetHealAmount (int amount) {
      labelHeal = amount.ToString();
    }

    public void SetCureAllAmount (int hp, int ep) {
      SetHealAmount(hp);
      SetEnergyRecoveryAmount(ep);
    }

    public void SetDefenseDecreaseAmount (int amount) {
      labelDefenseDecrease = amount.ToString();
    }

    public void SetEPDecreaseAmount(int amount) {
      labelEPDecrease = amount.ToString();
    }

    public void ShowLabel(string label) {
      switch (label) {
        case "defense": {
          labelText.ShowDefend(labelDefenseIncrease, labelEPRecovery, currentTarget);
          break;
        }

        case "heal": {
          labelText.ShowHeal(labelHeal, currentTarget);
          break;
        }

        case "cureAll": {
          labelText.ShowCureAll(labelHeal, labelEPRecovery, currentTarget);
          break;
        }

        case "hurt": {
          labelText.ShowDamage(labelDamage, currentTarget);
          break;
        }

        case "EP": {
          labelText.ShowEnergyRecovered(labelEPRecovery, currentTarget);
          break;
        }

        case "Hydroblast": {
          labelText.ShowHydroblastEffects(labelDamage, labelDefenseDecrease, currentTarget);
          break;
        }

        case "Fireball": {
          labelText.ShowFireballEffects(labelDamage, labelEPDecrease, currentTarget);
          break;
        }
      }
    }

    public void RemoveDeadPlayer(PartyMember member) {
      GetListOfAlivePlayers().Remove(member);
      deadPlayers.Add(member);
    }

    public List<PartyMember> GetDeadPlayersList() {
      return deadPlayers;
    }

    public List<Enemy> GetDeadEnemiesList() {
      return deadEnemies;
    }

    public bool IsValidHealTarget(BattleCharacter target) {
      return IsCharacterAPlayer(target) && target.IsCharacterDamaged();
    }

    public bool IsValidEnergyHealTarget(BattleCharacter target) {
      return IsCharacterAPlayer(target) && target.IsCharacterMissingEnergy();
    }

    public bool IsCurrentTurnPlayerTurn() {
      return actTurn == 0;
    }

    public void ResetUIPlayerInfo() {
      uiController.ResetCharacterInfo();
    }

    public bool IsCharacterAPlayer(BattleCharacter character) {
      return GetListOfAlivePlayers().Contains(character);
    }

    private bool AreAnyPlayersAlive() {
      return GetListOfAlivePlayers().Count > 0;
    }

    private bool AreAnyEnemiesAlive() {
      return GetEnemyList().Count > 0;
    }

    public bool IsBattleActive() {
      return AreAnyPlayersAlive() && AreAnyEnemiesAlive();
    }

    public List<BattleCharacter> GetListOfAlivePlayers() {
      return characters[0];
    }

    public BattleCharacter GetPlayer(int index) {
      return GetListOfAlivePlayers()[index];
    }

    public List<BattleCharacter> GetEnemyList() {
      return characters[1];
    }

    public BattleCharacter GetSelectedEnemy(int selectedEnemyIndex) {
      return GetEnemyList()[selectedEnemyIndex];
    }

    public BattleCharacter GetCurrentCharacter() {
      BattleCharacter character = characters[actTurn][characterTurnIndex];
      return character;
    }

    private bool HasACharacterNotGone() {
      return characterTurnIndex < characters[actTurn].Count - 1;
    }

    private void HideMenus() {
      questPanel = FindObjectOfType<QuestSystem.QuestPanel>();
      inventoryPanels = FindObjectsOfType<UIInventory>();
      battleSummaryPanel = FindObjectOfType<BattleSummaryPanel>();
      if (questPanel != null) {
        questPanel.gameObject.SetActive(false);
      }

      if (inventoryPanels != null) {
        foreach(UIInventory panel in inventoryPanels) {
          panel.gameObject.SetActive(false);
        }
      }

      if (battleSummaryPanel != null) {
        battleSummaryPanel.gameObject.SetActive(false);
      }
    }

    private void Awake() {
      characterController = FindObjectOfType<CharacterController>();
      inventoryController = FindObjectOfType<InventoryController>();
      items = FindObjectOfType<ConsumableInventory>();
      EventController.OnEnemyDied += AddEnemyExperience;
      EventController.OnEnemyDied += AddRandomItem;
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
        GetListOfAlivePlayers().Add(spawnPoints[i+3].Spawn(players[i])); // Add Players to spawn points 3-5
        GetListOfAlivePlayers()[i].upgradePointsDictionary = players[i].upgradePointsDictionary;
        for (int j = 0; j < players[i].equipment.Length; j++) {
          if (players[i].equipment[j] != null) {
            string abilityToAdd = GetListOfAlivePlayers()[i].AddAbilityFromEquipment(players[i].equipment[j]);
            if (abilityToAdd != "") {
              if (!DoesPlayerAlreadyHaveAbility(GetListOfAlivePlayers()[i], abilityToAdd))
              GetListOfAlivePlayers()[i].AddAbility(characterController.GetAbility(abilityToAdd));
            }
          }
        }
      }

      for (int i = 0; i < enemies.Count; i++) {
        GetEnemyList().Add(spawnPoints[i].Spawn(enemies[i])); // Add Enemies to spawn points 0-2
        GetEnemyList()[i].upgradePointsDictionary = enemies[i].upgradePointsDictionary;
        GetEnemyList()[i].ClearAbilities();
        for (int j = 0; j < GetEnemyList()[i].abilitiesList.Count; j++) {
          GetEnemyList()[i].AddAbility(characterController.GetAbility(GetEnemyList()[i].abilitiesList[j]));
        }
      }
    }

    public bool DoesPlayerAlreadyHaveAbility(BattleCharacter player, string abilityName) {
     return player.GetAbilities().Contains(characterController.GetAbility(abilityName));
    }

    public BattleCharacter GetRandomPlayer() {
      List<BattleCharacter> playerList = GetListOfAlivePlayers();
      return playerList[Random.Range(0, playerList.Count)];
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
      if (IsCurrentTurnPlayerTurn()) {
        uiController.SetColor(0, Color.red);
      } else {
        List<BattleCharacter> playerList = GetListOfAlivePlayers();
        for (int i = 0; i < playerList.Count; i++) {
          uiController.SetColor(i, Color.white);
        }
      }
    }

    public void NextAct() {
      if (IsCurrentTurnPlayerTurn()) {
        uiController.SetColor(characterTurnIndex, Color.white);
      }
      uiController.UpdateCharacterUI();
      uiController.ToggleAbilityPanel(false);
      uiController.ToggleItemPanel(false);

      if (AreAnyPlayersAlive() && AreAnyEnemiesAlive()) {
          abilityToBeUsed = null;
          playerIsAttacking = false;
          currentTarget = null;
        if (HasACharacterNotGone()) {
          characterTurnIndex++;
          if (IsCurrentTurnPlayerTurn()) {
            uiController.SetColor(characterTurnIndex, Color.red);
          }
        } else { // Every character has gone
          NextTurn(); // Flips actTurn to the other side
          characterTurnIndex = 0;
        }

// Check if player turn or enemy turn
        switch(actTurn) {
          case 0: {
            uiController.ToggleActionState(true);
            uiController.BuildAbilityList(GetCurrentCharacter().GetAbilities());
            uiController.BuildItemList();
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
        uiController.ToggleActionState(false);
        StartCoroutine(EndBattle());
      }
    }

    IEnumerator EndBattle() {
      yield return new WaitForSeconds(1f);
        if (AreAnyPlayersAlive()) {
        battleSummaryPanel.ShowVictoryPanel(xpToReward, itemsToGive);
        characterController.UpdatePlayers(GetListOfAlivePlayers(), xpToReward);
      } else {
        battleSummaryPanel.ShowDefeatPanel();
      }
    }

    IEnumerator PerformAct() {
      yield return new WaitForSeconds(1f);
      if (GetCurrentCharacter().health > 0) { // actTurn should always be 1 for enemies here!
        Enemy enemy = GetCurrentCharacter() as Enemy;
        enemy.Act();
        uiController.UpdateCharacterUI();
        yield return new WaitForSeconds(1f);
        NextAct();
      }
    }

    public void SelectTarget(BattleCharacter target) {
      SetCurrentTarget(target);
      if (playerIsAttacking) {
        if (IsCharacterAPlayer(target)) {
          Debug.LogWarning("Don't target your own team!");
        } else {
        DoAttack(GetCurrentCharacter(), target);
        NextAct();
        }
      }
      else if (abilityToBeUsed != null) {
        if (abilityToBeUsed.abilityType == Ability.AbilityType.Heal) { // TODO Could add support spells here too
          if (!IsValidHealTarget(target)) {
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
          SetCurrentTarget(target);
          uiController.UpdateCharacterUI();
          NextAct();
        }
        else {
          Debug.LogWarning("Not enough Energy to cast that Ability!");
        }
      } else if (itemToBeUsed != null) {
        if (items.UseItem(target, itemToBeUsed)) {

          uiController.UpdateCharacterUI();
          NextAct();
        };
      }
    }

    public void ResetBattlePanel() {
      playerIsAttacking = false;
      abilityToBeUsed = null;
      itemToBeUsed = null;
    }

    public void DoAttack(BattleCharacter attacker, BattleCharacter target) {
      Debug.Log(attacker.characterName + " is attacking " + target.characterName);
      target.Hurt(attacker.attackPower, "attack");
    }

    private void AddEnemyExperience(int enemyId) {
      Enemy killedEnemy = characterController.FindEnemyById(enemyId);
      deadEnemies.Add(killedEnemy);
      xpToReward += killedEnemy.experience;
   }

    private void AddRandomItem(int enemyId) {
      Item itemToGive = null;
      int roll = Random.Range(0, 10);
      if (roll < 3) {
        int randomItem = Random.Range(1, 13);
        if (randomItem < 13) {
          itemToGive = inventoryController.GetItem(randomItem);
        } else {
          itemToGive = inventoryController.GetItem(20); // Cure-all comes out of sequence, so its ID gets hardcoded
        }
      }
      if (itemToGive != null) {
        itemsToGive.Add(itemToGive);
      }
    }
  }
}