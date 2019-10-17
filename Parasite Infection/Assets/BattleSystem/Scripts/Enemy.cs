using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class Enemy : BattleCharacter {
    public int enemyId;

    public Enemy() {}

    public Enemy SetupEnemy (EnemyEntry enemy) {
      this.characterName = enemy.characterName;
      this.health = enemy.stats["health"];
      this.maxHealth = enemy.stats["maxHealth"];
      this.attackPower = enemy.stats["attackPower"];
      this.defensePower = enemy.stats["defensePower"];
      this.energyPoints = enemy.stats["energyPoints"];
      this.maxEnergyPoints = enemy.stats["maxEnergyPoints"];
      this.speed = enemy.stats["speed"];
      this.abilities = enemy.abilities;
      if (enemy.sprite != null) {
        this.sprite.sprite = enemy.sprite;
      }
      this.equipment = enemy.equipment;
      this.level = enemy.level;
      this.experience = enemy.level;
      return this;
    }

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
          if (abilityToCast != null) {
            if (abilityToCast.abilityType == Ability.AbilityType.Heal) {
              target = BattleController.Instance.GetWeakestEnemy(); // Ally in this case though because this is the Enemies script
            }
            if (!UseAbility(abilityToCast, target)) {
              BattleController.Instance.DoAttack(this, target);
            } else {
              didEnemyUseAbility = true;
            }
          } else {
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
      if (abilities != null) {
        return abilities[Random.Range(0, abilities.Count - 1)];
      } else {
       return null;
      }
    }

    public override void Die() {
      base.Die();
      EventController.EnemyDied(enemyId);
      BattleController.Instance.characters[1].Remove(this);
    }
  }
}