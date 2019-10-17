using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMemberEntry : BattleCharacterEntry {
    bool inParty;

    public PartyMemberEntry(
      int id,
      string characterName,
      Dictionary<string, int> stats
    ) {
      this.id = id;
      this.characterName = characterName;
      this.stats = stats;

      this.level = 1;
      this.experience = 0;
      this.inParty = false;
      this.LoadSprite();
    }
  }
}