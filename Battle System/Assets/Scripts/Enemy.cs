using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
  public void Act() {
    int dieRoll = Random.Range(0,2);
    // Character target = BattleController.Instance.GetRandomPlayer();
    switch(dieRoll) {
      case 0: {
        Defend();
        break;
      }
      case 1: {
        Ability abilityToCast = GetRandomAbility();
        if (abilityToCast.abilityType == Ability.AbilityType.Heal) {
          // get friendly weak target
        }
        if (!CastAbility(abilityToCast, null)) {
          // attack
        }
        break;
      }
      case 2: {
        // attack
        break;
      }
    }
  }

  Ability GetRandomAbility() {
    return abilities[Random.Range(0, abilities.Count - 1)];
  }

  public override void Die() {
    base.Die();
  }
}
