using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleLaunchCharacter : MonoBehaviour {
    [SerializeField] private List<BattleCharacter> enemies;
    [SerializeField] private BattleLauncher launcher;
    private List<BattleCharacter> players;

    void Start() {
      launcher = FindObjectOfType<BattleLauncher>();
      players = Party.Instance.GetPartyMembers();
    }

    public void PrepareBattle(Character character, NPC npc) {
      launcher.PrepareBattle(enemies, players, character.transform.position, npc);
    }
  }
}
