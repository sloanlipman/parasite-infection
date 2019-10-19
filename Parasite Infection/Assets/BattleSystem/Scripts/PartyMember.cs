using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    private bool inParty;
    public PartyMember() {}

    public bool IsInParty() {
      return inParty;
    }

    public void ToggleInParty(bool state) {
      inParty = state;
    }

    public override void Die() {
      base.Die();
      BattleController.Instance.characters[0].Remove(this);
    }
  }
}