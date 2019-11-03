using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    public int upgradePoints = 0;
    public int modSlots = 1;
    public PartyMember() {}

    public override void SetDefaultValues() {
      if (level == 0 || upgradePointsDictionary.Count == 0) {
        upgradePoints = 1;
      }
      base.SetDefaultValues();
    }

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
      BattleController.Instance.RemoveDeadPlayer(this);
      BattleController.Instance.ResetUIPlayerInfo();
    }
  }
}