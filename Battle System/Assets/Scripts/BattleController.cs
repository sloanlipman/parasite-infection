using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

  public static BattleController Instance { get; set; }
  [SerializeField] private BattleUIController uiController;

  public Dictionary<int, List<Character>> characters = new Dictionary<int, List<Character>>();
  public int characterTurnIndex;
  public Ability abilityToBeUsed;
  public bool playerIsAttacking;

  [SerializeField] private BattleSpawnPoint[] spawnPoints;
  private int actTurn; // act refers to Action (i.e. who is up on the current side?)
                      // Turn refers to are Enemies going or are players going?

  private bool IsCharacterAPlayer(Character character) {
    return GetPlayerList().Contains(character);
  }

  private bool IsAPlayerAlive() {
    return GetPlayerList().Count > 0;
  }

  private bool IsAnEnemyAlive() {
    return GetEnemyList().Count > 0;
  }

  public List<Character> GetPlayerList() {
    return characters[0];
  }

  public Character GetPlayer(int index) {
    return GetPlayerList()[index];
  }

  public List<Character> GetEnemyList() {
    return characters[1];
  }

  public Character GetSelectedEnemy(int selectedEnemyIndex) {
    return GetEnemyList()[selectedEnemyIndex];
  }

    public Character GetCurrentCharacter() {
    return characters[actTurn][characterTurnIndex];
  }

  private bool HasEveryCharacterGone() {
    return characterTurnIndex < (characters[actTurn].Count) - 1;
  }

  // Start is called before the first frame update
  private void Start() {
    if (Instance != null & Instance != this) {
      Destroy(this.gameObject);
    }
    else {
      Instance = this;
    }

    characters.Add(0, new List<Character>()); // players
    characters.Add(1, new List<Character>()); // enemies
    FindObjectOfType<BattleLauncher>().Launch();
  }

    public void StartBattle(List<Character> players, List<Character> enemies) {
    for (int i = 0; i < players.Count; i++) {
      GetPlayerList().Add(spawnPoints[i+3].Spawn(players[i])); // Add Players to spawn points 3-5
    }
    for (int i = 0; i < enemies.Count; i++) {
     GetEnemyList().Add(spawnPoints[i].Spawn(enemies[i])); // Add Enemies to spawn points 0-2
    }
  }

  public Character GetRandomPlayer() {
    List<Character> playerList = GetPlayerList();
    return playerList[Random.Range(0, playerList.Count - 1)];
  }

  public Character GetWeakestEnemy() {
    List<Character> enemyList = GetEnemyList();
    Character weakestEnemy = enemyList[0];
    foreach (Character enemy in enemyList) {
        if (enemy.health < weakestEnemy.health) {
          weakestEnemy = enemy;
        }
    }
    return weakestEnemy;
  }

  private void NextTurn() {
    actTurn = actTurn == 0? 1 : 0; // Swap between Players & Enemies
  }

  public void NextAct() {
    uiController.ToggleAbilityPanel(false);

    if (IsAPlayerAlive() && IsAnEnemyAlive()) {
      // if (HasEveryCharacterGone()) { // If not every player has gone

      if ( characterTurnIndex < (characters[actTurn].Count) - 1) {
        characterTurnIndex++;
      }
      else {
        NextTurn();
        characterTurnIndex = 0;
        Debug.Log("Turn has changed. Now it is the turn for: " + actTurn);
      }
    Debug.Log("Next turn goes to " + GetCurrentCharacter().characterName);
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
      Debug.Log("Battle is over!");
    }
  }

  IEnumerator PerformAct() {
    yield return new WaitForSeconds(0.75f);
    if (GetCurrentCharacter().health > 0) { // actTurn should always be 1 for enemies here!
      GetCurrentCharacter().GetComponent<Enemy>().Act();
    }
    uiController.UpdateCharacterUI();
    yield return new WaitForSeconds(1f);
    NextAct();
  }

  public void SelectTarget(Character target) {
    if (playerIsAttacking) {
      DoAttack(GetCurrentCharacter(), target);
    }
    else if (abilityToBeUsed != null) {
      if (abilityToBeUsed.abilityType == Ability.AbilityType.Heal) {
        if (!IsCharacterAPlayer(target) || !(target.isCharacterDamaged())) {
          Debug.Log("You can't heal your enemies, or your health is already maxed out!!");
          return;
        }
      }
      Debug.Log("Ability to be used is " + abilityToBeUsed.abilityName);
      Debug.Log("Target is " + target.characterName);
      if (GetCurrentCharacter().UseAbility(abilityToBeUsed, target)) {
        uiController.UpdateCharacterUI();
        NextAct();
      }
      else {
        Debug.LogWarning("Not enough Energy to cast that Ability!");
      }
    }
  }

  public void DoAttack(Character attacker, Character target) {
    Debug.Log(attacker.characterName + " is attacking " + target.characterName);
    target.Hurt(attacker.attackPower);
    NextAct();
  }
}
