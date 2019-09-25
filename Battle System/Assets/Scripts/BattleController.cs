using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

  public static BattleController Instance { get; set; }
  [SerializeField] private BattleUIController uiController;

  public Dictionary<int, List<Character>> characters = new Dictionary<int, List<Character>>();
  public int characterTurnIndex;
  public Ability playerSelectedAbility;
  public bool playerIsAttacking;

  [SerializeField] private BattleSpawnPoint[] spawnPoints;
  private int actTurn; // act refers to Action (i.e. who is up on the current side?)
                      // Turn refers to are Enemies going or are players going?

  private bool IsAPlayerAlive() {
    return GetPlayerList().Count > 0;
  }

  private bool IsAnEnemyAlive() {
    return characters[1].Count > 0;
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

  public Character GetEnemy(int index) {
    return GetEnemyList()[index];
  }

  private bool HasEveryCharacterGone() {
    return characterTurnIndex > characters[actTurn].Count - 1;
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
  }

  public Character GetRandomPlayer() {
    return GetPlayerList()[Random.Range(0, GetPlayerList().Count - 1)];
  }

  public Character GetWeakestEnemy() {
    Character weakestEnemy = GetEnemyList()[0];
    foreach (Character enemy in GetEnemyList()) {
        if (enemy.health < weakestEnemy.health) {
          weakestEnemy = enemy;
        }
    }
    return weakestEnemy;
  }

  public void StartBattle(List<Character> players, List<Character> enemies) {
    Debug.Log("Battle is being set up!");
    for (int i = 0; i < players.Count; i++) {
      GetPlayerList().Add(spawnPoints[i+3].Spawn(players[i])); // Add Players to spawn points 3-5
    }
    for (int i = 0; i < enemies.Count; i++) {
      GetEnemyList().Add(spawnPoints[i].Spawn(enemies[i])); // Add Enemies to spawn points 0-2
    }
  }

  private void NextTurn() {
    actTurn = actTurn == 0? 1 : 0; // Swap between Players & Enemies
  }

  private void NextAct() {
    if (IsAPlayerAlive() && IsAnEnemyAlive()) {
      if (!HasEveryCharacterGone()) { // If not every player has gone
        characterTurnIndex++;
      }
      else {
        NextTurn();
        characterTurnIndex = 0;
        Debug.Log("Turn has changed. Now it is the turn for: " + actTurn);
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
      Debug.Log("Battle is over!");
    }
  }

  IEnumerator PerformAct() {
    yield return new WaitForSeconds(0.75f);
    if (GetCurrentCharacter().health > 0) { // actTurn should always be 1 for enemies here!
      GetCurrentCharacter().GetComponent<Enemy>().Act();
    }
    yield return new WaitForSeconds(1f);
    NextAct();
  }

  public void SelectCharacter(Character character) {
    if (playerIsAttacking) {
      DoAttack(GetCurrentCharacter(), character);
    }
    else if (playerSelectedAbility != null) {
      if (GetCurrentCharacter().CastAbility(playerSelectedAbility, character)) {
        NextAct();
      }
      else {
        Debug.LogWarning("Not enough Energy to cast that Ability!");
      }
    }
  }

  public Character GetCurrentCharacter() {
    return characters[actTurn][characterTurnIndex];
  }

  public void DoAttack(Character attacker, Character target) {
    target.Hurt(attacker.attackPower);
  }
}
