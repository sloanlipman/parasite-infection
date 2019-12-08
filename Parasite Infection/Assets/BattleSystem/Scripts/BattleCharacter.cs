using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleCharacter : MonoBehaviour {
    public string characterName;
    public int health;
    public int maxHealth;
    public int attackPower;
    public int defensePower;
    public int energyPoints;
    public int maxEnergyPoints;
    protected List<Ability> abilities = new List<Ability>();
    public List<string> abilitiesList = new List<string>();
    public int level = 1;
    public int experience;
    public Item[] equipment = new Item[4];

    public int originalMaxHealth;
    public int originalMaxEP;
    public int originalAttackPower;
    public int originalDefensePower;
    public string originalCharacterName;

    public Dictionary<string, int> upgradePointsDictionary = new Dictionary<string, int>();

    public virtual void SetDefaultValues() {
      if (level == 0 || upgradePointsDictionary.Count == 0) {
        level = 1;
        maxHealth = originalMaxHealth;
        maxEnergyPoints = originalMaxEP;
        health = maxHealth;
        energyPoints = maxEnergyPoints;
        attackPower = originalAttackPower;
        defensePower = originalDefensePower;
        characterName = originalCharacterName;
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

    public List<Ability> GetAbilities() {
      return abilities;
    }

    public void AddAbility(Ability ability) {
      abilities.Add(ability);
    }

    public void ClearAbilities() {
      abilities.Clear();
    }

    public void Hurt (int amount, string damageSource, int rawPower = 0) {
      int damageAmount;
      if (amount < defensePower) {
        damageAmount = Random.Range(0, amount / 2);
      }
      else {
        damageAmount = amount - defensePower;
      }
      health = Mathf.Max(health - damageAmount, 0);
      if (health == 0) {
        Die();
      }

      BattleController.Instance.SetDamageLabel(damageAmount);

      switch(damageSource) {
        case "Hydroblast": {
          LowerDefense(rawPower / 5);
          break;
        }

        case "Fireball": {
          ReduceEnergy(damageAmount / 5);
          break;
        }

        default: {
          BattleController.Instance.ShowLabel("hurt");
          break;
        }
      }

    }

    public void LowerDefense(int amount) {
      int amountToDecrease = Mathf.Max(defensePower - amount, 0);
      defensePower -= amountToDecrease;
      BattleController.Instance.SetDefenseDecreaseAmount(amountToDecrease);
      BattleController.Instance.ShowLabel("Hydroblast");
    }

    public void ReduceEnergy(int amount) {
      int amountToDecrease = Mathf.Max(energyPoints - amount, 0);
      energyPoints -= amountToDecrease;
      BattleController.Instance.SetEPDecreaseAmount(amountToDecrease);
      BattleController.Instance.ShowLabel("Fireball");
    }

    public void Heal (int amount) {
      int healAmount = amount;
      health = Mathf.Min(health + healAmount, maxHealth);
      if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Battle")) {
        BattleController.Instance.SetHealAmount(healAmount);
        BattleController.Instance.ShowLabel("heal");
      }
    }

    public void Defend() {
      BattleController.Instance.SetCurrentTarget(BattleController.Instance.GetCurrentCharacter());
      int defenseToIncrease = Mathf.Max((int) Mathf.Round(defensePower * 0.25f), 1);
      defensePower += defenseToIncrease;
      BattleController.Instance.SetDefenseIncrease(defenseToIncrease);
      int energyToRecover = (int)Mathf.Round(0.1f * maxEnergyPoints);
      RecoverEnergy(energyToRecover, false);
      BattleController.Instance.ShowLabel("defense");
      BattleController.Instance.GetTooltip().GenerateAutoDismissTooltip(string.Format("{0} defended.", characterName));
    }

    public void RecoverEnergy(int amount, bool showLabel = true) {
      energyPoints += amount;
      if (energyPoints > maxEnergyPoints) {
        energyPoints = maxEnergyPoints;
      }
      if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Battle")) {
        BattleController.Instance.SetEnergyRecoveryAmount(amount);
        BattleController.Instance.ShowLabel("EP");
      }
    }

    public bool UseAbility(Ability ability, BattleCharacter targetCharacter) {
      bool successful = energyPoints >= ability.energyCost;
      BattleController.Instance.GetTooltip().GenerateAutoDismissTooltip(string.Format("{0} used {1} on {2}!", characterName, ability.abilityName, targetCharacter.characterName));
      if (successful) {
        Ability abilityToCast = Instantiate<Ability>(ability, transform.position, Quaternion.identity);
        energyPoints -= ability.energyCost;
        int powerToAdd = 0;
        if (upgradePointsDictionary.ContainsKey(abilityToCast.abilityName)) {
          powerToAdd =  upgradePointsDictionary[abilityToCast.abilityName];
        }
        int power = ability.power + powerToAdd;
        abilityToCast.Cast(this, targetCharacter, power);
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
      BattleController.Instance.GetTooltip().GenerateAutoDismissTooltip(string.Format("{0} has died!", characterName));
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

    public virtual void LevelUp() {
      level++;
      maxHealth++;
      maxEnergyPoints++;
      attackPower++;
      defensePower++;
    }

    public void ReduceFinalBossStats() {
      health = ReduceStat(health);
      maxHealth = ReduceStat(maxHealth);
      attackPower = ReduceStat(attackPower);
      defensePower = ReduceStat(defensePower);
      energyPoints = ReduceStat(energyPoints);
      maxEnergyPoints = ReduceStat(maxEnergyPoints);
    }

    public void IncreaseFinalBossStats() {
      health = IncreaseStat(health);
      maxHealth = IncreaseStat(maxHealth);
      attackPower = IncreaseStat(attackPower);
      defensePower = IncreaseStat(defensePower);
      energyPoints = IncreaseStat(energyPoints);
      maxEnergyPoints = IncreaseStat(maxEnergyPoints);
    }

    private int  ReduceStat(int stat) {
      return stat * 3 /4;
    }

    private int IncreaseStat(int stat) {
      return stat + stat / 4;
    }

    public void SetSprite(Sprite sprite) {
      SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
      if (spriteRender != null) {
        spriteRender.sprite = sprite;
      }
    }
  }
}
