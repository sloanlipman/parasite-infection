using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleLaunchCharacter : MonoBehaviour {
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private BattleLauncher launcher;
    private List<PartyMember> players;
    private CharacterController characterController;

    void Start() {
      launcher = FindObjectOfType<BattleLauncher>();
      characterController = FindObjectOfType<CharacterController>();
    }

    public void PrepareBattle(Character character) {
      players = characterController.GetActiveParty();
      launcher.PrepareBattle(character.transform.position, enemies);
    }
  }
}
