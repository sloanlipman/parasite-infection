using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class Enemy : BattleCharacter {

    public bool Act() {
      bool didEnemyUseAbility = false;
      int dieRoll = Random.Range(0,2);
      BattleCharacter target = BattleController.Instance.GetRandomPlayer();
      switch(dieRoll) {
        case 0: {
          Defend();
          break;
        }
        case 1: {
          Ability abilityToCast = GetRandomAbility();
          if (abilityToCast.abilityType == Ability.AbilityType.Heal) {
            target = BattleController.Instance.GetWeakestEnemy();
          }
          if (!UseAbility(abilityToCast, target)) {
            didEnemyUseAbility = true;
            BattleController.Instance.DoAttack(this, target);
          }
          break;
        }
        case 2: {
            BattleController.Instance.DoAttack(this, target);
          break;
        }
      }
      return didEnemyUseAbility;
    }

    Ability GetRandomAbility() {
      return abilities[Random.Range(0, abilities.Count - 1)];
    }

    public override void Die() {
      base.Die();
      BattleController.Instance.characters[1].Remove(this);
    }
  }
}