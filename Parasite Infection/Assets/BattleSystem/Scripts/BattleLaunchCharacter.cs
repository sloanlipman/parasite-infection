using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLaunchCharacter : MonoBehaviour {
  [SerializeField] private List<BattleSystem.BattleCharacter> players, enemies;
  [SerializeField] private BattleSystem.BattleLauncher launcher;

  void Awake() {
    launcher = FindObjectOfType<BattleSystem.BattleLauncher>();
  }

  public void PrepareBattle(Character character) {
    launcher.PrepareBattle(enemies, players, character.transform.position);
  }
}
