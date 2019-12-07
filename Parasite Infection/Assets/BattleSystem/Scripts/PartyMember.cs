﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    public int upgradePoints = 1;
    public int modSlots = 1;
    public PartyMember() {}

    public override void SetDefaultValues() {
      if (level == 0 || upgradePointsDictionary.Count == 0) {
        upgradePoints = 0;
        modSlots = 1;
      }
      experience = 0;
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

    public void AddUpgradePoints(int i) {
      upgradePoints += i;
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

    public void SetModSlots() {
      // TODO check the balance of this scheme
      if (level < 10) {
        modSlots = 1;
      } else if (level < 20) {
        modSlots = 2;
      } else if (level < 30) {
        modSlots = 3;
      } else {
        modSlots = 4;
      }
    }

    public override void Die() {
      base.Die();
      BattleController.Instance.RemoveDeadPlayer(this);
      BattleController.Instance.ResetUIPlayerInfo();
    }

    public override void LevelUp() {
      base.LevelUp();

      health = maxHealth;
      energyPoints = maxEnergyPoints;
    }

    public void CopyPartyMember(PartyMember member) {
      upgradePoints = member.upgradePoints;
      modSlots = member.modSlots;
      level = member.level;

      health = member.health;
      maxHealth = member.maxHealth;
      attackPower = member.attackPower;
      defensePower = member.defensePower;
      energyPoints = member.energyPoints;
      maxEnergyPoints = member.maxEnergyPoints;
      experience = member.experience;
      upgradePointsDictionary = member.upgradePointsDictionary;
      equipment = member.equipment;
    }
  }
}
