using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character {
  public override void Die() {
    base.Die();
    BattleController.Instance.characters[1].Remove(this);
  }
}
