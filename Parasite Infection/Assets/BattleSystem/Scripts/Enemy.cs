using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class Enemy : BattleCharacter {
    public int enemyId;
    public Enemy() {}
    public int originalExperience;

    public override void SetDefaultValues() {
      base.SetDefaultValues();
      experience = originalExperience;
    }

    public void Act() {
      int dieRoll = Random.Range(0,3);
      BattleCharacter damageTarget = BattleController.Instance.GetRandomPlayer();
      BattleCharacter healTarget = BattleController.Instance.GetWeakestEnemy(); // Ally in this case though because this is the Enemies script
      BattleCharacter actualTarget = damageTarget;

      BattleController.Instance.SetCurrentTarget(actualTarget);

      switch(dieRoll) {
        case 0: {
          Defend();
          break;
        }
        case 1: {
          Ability abilityToCast = GetRandomAbility();
          if (abilityToCast != null) {

            if (abilityToCast.abilityType == Ability.AbilityType.Heal) {
              actualTarget = healTarget;
              BattleController.Instance.SetCurrentTarget(actualTarget);
            }

            if (!UseAbility(abilityToCast, actualTarget)) {
              actualTarget = damageTarget;
              BattleController.Instance.SetCurrentTarget(actualTarget);
              BattleController.Instance.DoAttack(this, actualTarget);
            }

          } else {
            BattleController.Instance.SetCurrentTarget(damageTarget);
            BattleController.Instance.DoAttack(this, damageTarget);
          }
          break;
        }
        case 2: {
          BattleController.Instance.SetCurrentTarget(damageTarget);
          BattleController.Instance.DoAttack(this, damageTarget);
          break;
        }
      }
    }

    Ability GetRandomAbility() {
      if (abilities != null) {
        return abilities[Random.Range(0, abilities.Count)];
      } else {
       return null;
      }
    }

    public override void Die() {
      base.Die();
      EventController.EnemyDied(enemyId);
      BattleController.Instance.characters[1].Remove(this);
    }

    public override void LevelUp() {
      base.LevelUp();
      List<string> stats = new List<string>();
      foreach (var stat in upgradePointsDictionary) {
        stats.Add(stat.Key.ToString());
      }

      stats.ForEach(stat => upgradePointsDictionary[stat]++);
      int maxRoll = upgradePointsDictionary.Count;
      int dieRoll = Random.Range(0, maxRoll);
      if (dieRoll == maxRoll) {
        experience++;
      } else {
    // Give enemies an extra random boost
        string statToIncrease = stats[dieRoll];
        upgradePointsDictionary[statToIncrease]++;
      }

      health = maxHealth;
      energyPoints = maxEnergyPoints;
    }
  }
}