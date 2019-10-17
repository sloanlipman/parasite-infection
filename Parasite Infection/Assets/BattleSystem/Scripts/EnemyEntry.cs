using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class EnemyEntry : BattleCharacterEntry {
    public int enemyId;

    public EnemyEntry(
      int id,
      string characterName,
      Dictionary<string, int> stats,
      List<Ability> abilities
    ) {
      this.enemyId = id;
      this.characterName = characterName;
      this.stats = stats;
      this.LoadSprite();
    }
  }
}