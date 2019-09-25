using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
  public string characterName;
  public int health;
  public int maxHealth;
  public int attackPower;
  public int defensePower;
  public int energyPoints;
  public List<Ability> abilities;

  public void Hurt (int amount) {
    int damageAmount = Random.Range(0,1) * (amount - defensePower);
    health = Mathf.Max(health - damageAmount, 0);
    if (health == 0) {
      Die();
    }
  }

  public void Heal (int amount) {
    int healAmount = amount;
    health = Mathf.Min(health + healAmount, maxHealth);
  }

  public void Defend() {
    defensePower += (int) (defensePower * 0.33);
    Debug.Log("Defense increased to: " + defensePower);
  }

  public bool CastAbility(Ability ability, Character targetCharacter) {
    bool successful = energyPoints >= ability.energyCost;
    if (successful) {
      Ability abilityToCast = Instantiate<Ability>(ability, transform.position, Quaternion.identity);
      energyPoints -= ability.energyCost;
      abilityToCast.Cast(targetCharacter);
    }
    return successful;
  }

  public virtual void Die() {
    Destroy(this.gameObject);
    Debug.LogFormat("{0} has died!", characterName);
  }
}
