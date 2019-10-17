using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class CharacterDatabase : MonoBehaviour {
    public static CharacterDatabase Instance {get; set;}

    public List<PartyMemberEntry> partyMembers = new List<PartyMemberEntry>();
    public List<EnemyEntry> enemiesList = new List<EnemyEntry>();

    public List<Ability> abilityList = new List<Ability>();

    public List<PartyMemberEntry> GetPartyMembers() {
      partyMembers.ForEach(member => Debug.Log(member.characterName));
      return partyMembers;
    }
    
    public List<EnemyEntry> GetEnemies() {
      return enemiesList;
    }

    public void AddPartyMember(PartyMember partyMember) {
      
    }

  
    void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(this.gameObject);
      } else {
        Instance = this;
      }
      DontDestroyOnLoad(this.gameObject);

      BuildAbilityList();
      BuildPartyDatabase();
      BuildEnemyDatabase();
    }

    public Ability GetAbility(string abilityName) {
      return abilityList.Find(ability => ability.abilityName == abilityName);
    }

    void BuildAbilityList() {
      abilityList = new List<Ability>() {
        {Resources.Load<Ability>("Abilities/Fireball")},
        {Resources.Load<Ability>("Abilities/Barrage")},
        {Resources.Load<Ability>("Abilities/Hydroblast")},
        {Resources.Load<Ability>("Abilities/Heal")}
      };
    }



    void BuildPartyDatabase() {
      partyMembers = new List<PartyMemberEntry>() {
        new PartyMemberEntry(0, "Barry",
        new Dictionary<string, int> {
          {"health", 15},
          {"maxHealth", 15},
          {"energyPoints", 15},
          {"maxEnergyPoints", 15},
          {"attackPower", 5},
          {"defensePower", 3},
          {"speed", 5}
        }),
        new PartyMemberEntry(1, "Alan",
        new Dictionary<string, int> {
          {"health", 15},
          {"maxHealth", 15},
          {"energyPoints", 15},
          {"maxEnergyPoints", 15},
          {"attackPower", 5},
          {"defensePower", 3},
          {"speed", 5}
        }),
        new PartyMemberEntry(2, "Android",
        new Dictionary<string, int> {
          {"health", 15},
          {"maxHealth", 15},
          {"energyPoints", 15},
          {"maxEnergyPoints", 15},
          {"attackPower", 5},
          {"defensePower", 3},
          {"speed", 5}
        })
      };
    }

    void BuildEnemyDatabase() {
        enemiesList = new List<EnemyEntry>() {
        new EnemyEntry(0, "Blob",
        new Dictionary<string, int> {
          {"health", 10},
          {"maxHealth", 10},
          {"energyPoints", 15},
          {"maxEnergyPoints", 15},
          {"attackPower", 5},
          {"defensePower", 1},
          {"speed", 7}
        },
        new List<Ability>{
          GetAbility("Fireball")
        }),
       new EnemyEntry(1, "Hatchling",
        new Dictionary<string, int> {
          {"health", 10},
          {"maxHealth", 10},
          {"energyPoints", 10},
          {"maxEnergyPoints", 10},
          {"attackPower", 5},
          {"defensePower", 20},
          {"speed", 2}
        },
        new List<Ability>{
          GetAbility("Heal")
        }),
        new EnemyEntry(2, "InfectedAndroid",
        new Dictionary<string, int> {
          {"health", 25},
          {"maxHealth", 25},
          {"energyPoints", 15},
          {"maxEnergyPoints", 15},
          {"attackPower", 7},
          {"defensePower", 3},
          {"speed", 10}
        },
        new List<Ability>{
          GetAbility("Barrage")
        }),
      };
    }
  }
}