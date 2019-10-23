using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleCharacter : MonoBehaviour {
    public string characterName;
    public int health;
    public int maxHealth;
    public int attackPower;
    public int defensePower;
    public int energyPoints;
    public int maxEnergyPoints;
    public int speed;
    public List<Ability> abilities = new List<Ability>();
    public List<string> abilitiesList = new List<string>();
    public int level = 1;
    public int experience;
    public List<Item> equipment = new List<Item>();

    public List<Item> GetEquipment() {
      return equipment;
    }

    public void Hurt (int amount) {
      int damageAmount;
      if (amount < defensePower) {
        damageAmount = Random.Range(0, amount);
      }
      else {
        damageAmount = amount - defensePower;
      }
      Debug.Log("Damage amount: " + damageAmount);
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
      defensePower += (int) Mathf.Round(defensePower * 0.25f);
      Debug.Log("Defense increased to: " + defensePower);
    }

    public void RecoverEnergy(int amount) {
      energyPoints += amount;
      if (energyPoints > maxEnergyPoints) {
        energyPoints = maxEnergyPoints;
      }
    }

    public bool UseAbility(Ability ability, BattleCharacter targetCharacter) {
      bool successful = energyPoints >= ability.energyCost;
      Debug.Log("Successful use of ability?" + successful);
      if (successful) {
        Ability abilityToCast = Instantiate<Ability>(ability, transform.position, Quaternion.identity);
        energyPoints -= ability.energyCost;
        abilityToCast.Cast(targetCharacter);
      }
      return successful;
    }

    public bool IsCharacterDamaged() {
      return health < maxHealth;
    }

    public virtual void Die() {
      Destroy(this.gameObject);
      Debug.LogFormat("{0} has died!", characterName);
    }

     public void AddAbilityFromEquipment(Item item) {
        string abilityToAdd;
          if (item.stats.ContainsKey("Ability")) {
            switch (item.stats["Ability"]) {
              case 1: {
                abilityToAdd = "Barrage";
                break;
              }
              
              case 2: {
                abilityToAdd = "Fireball";
                break;
              }
              case 3: {
                abilityToAdd = "Hydroblast";
                break;
              }
              case 4: {
                abilityToAdd = "Heal";
                break;
              }
              default: {
                abilityToAdd = null;
                break;
              }
            }

            if (abilityToAdd != null && !abilitiesList.Contains(abilityToAdd)) {
              abilitiesList.Add(abilityToAdd);
            }
          }
    }
  }
}