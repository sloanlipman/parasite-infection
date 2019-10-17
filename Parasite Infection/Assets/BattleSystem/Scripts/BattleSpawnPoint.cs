using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleSpawnPoint : MonoBehaviour {
    public BattleCharacter SpawnPartyMember(PartyMemberEntry character) {
      BattleCharacter characterToSpawn = ConvertPartyMemberEntry(character);
      return characterToSpawn;
    }
      private PartyMember ConvertPartyMemberEntry(PartyMemberEntry p) {
        GameObject character = Instantiate(Resources.Load("Characters/PartyMemberTemplate") as GameObject, this.transform);
        PartyMember member = character.GetComponent<PartyMember>().SetupPartyMember(p);
        return member;
    }

    public BattleCharacter SpawnEnemy(EnemyEntry character) {
      BattleCharacter characterToSpawn = ConvertEnemyEntry(character);
      return characterToSpawn;
    }

      private Enemy ConvertEnemyEntry(EnemyEntry e) {
        GameObject character = Instantiate(Resources.Load("Characters/EnemyTemplate") as GameObject, this.transform);
        Enemy enemy = character.GetComponent<Enemy>().SetupEnemy(e);
        return enemy;
    }
  }
}
