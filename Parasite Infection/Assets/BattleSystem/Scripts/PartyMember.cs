using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    public override void Die() {
      base.Die();
      BattleController.Instance.characters[1].Remove(this);
    }
  }
}