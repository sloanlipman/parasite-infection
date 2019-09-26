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
  public int maxEnergyPoints;
  // public int maxEnergyPoints; //TODO add max mana, add energy recovery
  public List<Ability> abilities;

  public void Hurt (int amount) {
    int damageAmount;
    if (amount < defensePower) {
      damageAmount = Random.Range(0, amount);
    }
    else {
      damageAmount = amount - defensePower;
    }
    Debug.Log("Base damage " + amount);
    Debug.Log("Defense: " + defensePower);
    Debug.Log("Damage amount: " + damageAmount);
    health = Mathf.Max(health - damageAmount, 0);
    Debug.Log(characterName + "now has " + health + "hp left!");

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

  public bool UseAbility(Ability ability, Character targetCharacter) {
    bool successful = energyPoints >= ability.energyCost;
    Debug.Log("Successful use of ability?" + successful);
    if (successful) {
      Ability abilityToCast = Instantiate<Ability>(ability, transform.position, Quaternion.identity);
      energyPoints -= ability.energyCost;
      abilityToCast.Cast(targetCharacter);
    }
    return successful;
  }

  public bool isCharacterDamaged() {
    return health < maxHealth;
  }

  public virtual void Die() {
    Destroy(this.gameObject);
    Debug.LogFormat("{0} has died!", characterName);
  }
}
