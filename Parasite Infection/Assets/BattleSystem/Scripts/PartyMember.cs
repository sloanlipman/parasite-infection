using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    [SerializeField] private bool inParty = false;
    int upgradePoints = 0;
    private int modSlots = 1;
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