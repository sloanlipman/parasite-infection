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
    public Item[] equipment = new Item[4];

    public int originalMaxHealth;
    public int originalMaxEP;
    public int originalAttackPower;
    public int originalDefensePower;

    public Dictionary<string, int> upgradePointsDictionary = new Dictionary<string, int>();

    public virtual void SetDefaultValues() {

      if (level == 0 || upgradePointsDictionary.Count == 0) {
        level = 1;
        experience = 0;
        maxHealth = originalMaxHealth;
        maxEnergyPoints = originalMaxEP;
        health = maxHealth;
        energyPoints = maxEnergyPoints;
        attackPower = originalAttackPower;
        defensePower = originalDefensePower;
        upgradePointsDictionary = new Dictionary<string, int> {
            {"HP", 0},
            {"EP", 0},
            {"Attack", 0},
            {"Defense", 0},
            {"Barrage", 0},
            {"Fireball", 0},
            {"Hydroblast", 0},
            {"Heal", 0},
        };
      }
    }

    public Item[] GetEquipment() {
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
      int energyToRecover = (int)Mathf.Round(0.1f * maxEnergyPoints);
      RecoverEnergy(energyToRecover);
      Debug.Log("Defense increased to: " + defensePower);
      Debug.Log("Recovered energy points: " + energyToRecover);
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
        int powerToAdd = 0;
        foreach(var up in upgradePointsDictionary) {
          Debug.Log(up.Key.ToString() + up.Value.ToString());
        }
        if (upgradePointsDictionary.ContainsKey(abilityToCast.abilityName)) {
          powerToAdd =  upgradePointsDictionary[abilityToCast.abilityName];
        }
        int power = ability.power + powerToAdd;
        abilityToCast.Cast(targetCharacter, power);
      }
      return successful;
    }

    public bool IsCharacterDamaged() {
      return health < maxHealth;
    }

    public bool IsCharacterMissingEnergy() {
      return energyPoints < maxEnergyPoints;
    }

    public virtual void Die() {
      Destroy(this.gameObject);
      Debug.LogFormat("{0} has died!", characterName);
    }

    public string AddAbilityFromEquipment(Item item) {
      string abilityToAdd = "";
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
              abilityToAdd = "";
              break;
            }
          }

        if (abilityToAdd != null && !abilitiesList.Contains(abilityToAdd)) {
          abilitiesList.Add(abilityToAdd);
        }
      }
      return abilityToAdd;
    }

    public void LevelUp() {
      level++;
      maxHealth++;
      maxEnergyPoints++;
      attackPower++;
      defensePower++;
      health = maxHealth;
      energyPoints = maxEnergyPoints;
    }
  }
}