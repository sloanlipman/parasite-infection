using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleSpawnPoint : MonoBehaviour {
    public BattleCharacter Spawn(BattleCharacter character) {
      BattleCharacter characterToSpawn = Instantiate<BattleCharacter>(character, this.transform);
      return characterToSpawn;
    }
  }
}