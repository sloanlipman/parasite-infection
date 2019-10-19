using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleLaunchCharacter : MonoBehaviour {
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private BattleLauncher launcher;
    // private List<PartyMemberEntry> players;
    private List<PartyMember> players;

    void Start() {
      launcher = FindObjectOfType<BattleLauncher>();
      players = CharacterDatabase.Instance.GetPartyMembers();
    }

    public void PrepareBattle(Character character) {
      launcher.PrepareBattle(character.transform.position);
    }
  }
}
