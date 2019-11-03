using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    private int upgradePoints = 0;
    public int modSlots = 1;
    public PartyMember() {}

    public int GetUpgradePoints() {
      return upgradePoints;
    }

    public void SetUpgradePoints(int i) {
      upgradePoints = i;
    }

    public void AddUpgradePoint() {
      upgradePoints++;
    }

    public void SpendUpgradePoint() {
      upgradePoints--;
    }

    public bool HasUpgradePointsToSpend() {
      return upgradePoints > 0;
    }

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