using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
  public void Act() {
    int dieRoll = Random.Range(0,2);
    Character target = BattleController.Instance.GetRandomPlayer();
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
          BattleController.Instance.DoAttack(this, target);
        }
        break;
      }
      case 2: {
          BattleController.Instance.DoAttack(this, target);
        break;
      }
    }
  }

  Ability GetRandomAbility() {
    return abilities[Random.Range(0, abilities.Count - 1)];
  }

  public override void Die() {
    base.Die();
    BattleController.Instance.characters[1].Remove(this);
  }
}
