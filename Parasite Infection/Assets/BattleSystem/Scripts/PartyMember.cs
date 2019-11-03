using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    int upgradePoints = 0;
    public int modSlots = 1;
    public PartyMember() {}

    public int GetModSlots() {
      return modSlots;
    }

    public void SetModSlots(int num) {
      modSlots = num;
    }

    public override void Die() {
      base.Die();
      BattleController.Instance.GetListOfAlivePlayers().Remove(this);
      BattleController.Instance.ResetUIPlayerInfo();
    }
  }
}