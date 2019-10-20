using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    [SerializeField] private bool inParty = false;
    public PartyMember() {}

    public override void Die() {
      base.Die();
      BattleController.Instance.characters[0].Remove(this);
    }
  }
}