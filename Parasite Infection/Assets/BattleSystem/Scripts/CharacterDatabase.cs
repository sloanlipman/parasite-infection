using UnityEngine;
using UnityEditor;
using System;

 using System.Collections.Generic;
namespace BattleSystem {
  public class CharacterDatabase : MonoBehaviour {
    public static CharacterDatabase Instance {get; set;}
    public List<Enemy> enemyList = new List<Enemy>();
    public List<PartyMember> partyMembers = new List<PartyMember>();
    public List<string> activePartyMembers = new List<string>();
    public Dictionary<string, Ability> abilityList = new Dictionary<string, Ability>();


    private void AddAndroidToParty() {
      activePartyMembers.Add("Android"); // TODO generalize this method
    }

    private void RemoveAndroidFromParty() {
      activePartyMembers.Remove("Android");
    }

    public void Load() {
      ES3.LoadInto("PartyMembers", "Party.ES3", partyMembers);
      ES3.LoadInto("Enemies", "Enemies.ES3", enemyList);
      activePartyMembers = ES3.Load<List<string>>("ActiveParty", "ActiveParty.ES3");

      partyMembers.ForEach(member => {
        foreach(Item item in member.equipment) {
          if (item != null && item.icon == null) {
            item.LoadSprite();
          }
        }
      });
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
      EventController.OnKillBlobsQuestCompleted += AddAndroidToParty;
    }

    public List<PartyMember> GetActiveParty() {
      List<PartyMember> activePartyMemberList = new List<PartyMember>();
      activePartyMembers.ForEach(name => {
        partyMembers.ForEach(member => {
          if (member.characterName == name) {
            activePartyMemberList.Add(member);
          }
        });
      });
        return activePartyMemberList;
      }

    public List<PartyMember> GetPartyMembers() {
      return partyMembers;
    }

    public List<string> GetActivePartyList() {
      return activePartyMembers;
    }

    public List<Enemy> GetEnemies() {
      return enemyList;
    }

    public Ability GetAbility(string abilityName) {
      Ability ability = abilityList.ContainsKey(abilityName) ? abilityList[abilityName] : null;
      return ability;
    }

    void BuildAbilityList() {
      abilityList = new Dictionary<string, Ability>() {
        {"Fireball", Resources.Load<Ability>("Abilities/Fireball")},
        {"Barrage", Resources.Load<Ability>("Abilities/Barrage")},
        {"Hydroblast", Resources.Load<Ability>("Abilities/Hydroblast")},
        {"Heal", Resources.Load<Ability>("Abilities/Heal")}
      };
    }

    public Dictionary<string, Ability> GetAbilityList() {
      return abilityList;
    }

    void BuildPartyDatabase() {
      partyMembers = new List<PartyMember>() {
        Resources.Load<PartyMember>("Players/Barry"),
        Resources.Load<PartyMember>("Players/Android"),
        Resources.Load<PartyMember>("Players/Alan")
      };

      LoadPlayerAbilities();
      activePartyMembers.Add("Barry");
    }

    public PartyMember FindPartyMemberByName(string name) {
      return partyMembers.Find(member => member.characterName == name);
    }

    public void LoadPlayerAbilities() {
      partyMembers.ForEach(member => {
        for (int i = 0; i < member.abilities.Count; i++) {
          member.abilities.Clear();
          member.abilities.Add(GetAbility(member.abilitiesList[i]));
        }
      });
    }

    public void LoadEnemyAbilities() {
      enemyList.ForEach(enemy => {
        for (int i = 0; i < enemy.abilities.Count; i++) {
          enemy.abilities.Clear();
          enemy.abilities.Add(GetAbility(enemy.abilitiesList[i]));
          
        }
      });
    }

    public Enemy FindEnemyById(int enemyId) {
      return enemyList.Find(enemy => enemy.enemyId == enemyId);
    }

    void BuildEnemyDatabase() {
      enemyList = new List<Enemy>() {
        Resources.Load<Enemy>("Enemies/0_Blob"),
        Resources.Load<Enemy>("Enemies/1_Drill"),
        Resources.Load<Enemy>("Enemies/2_Drone"),
        Resources.Load<Enemy>("Enemies/3_Hatchling"),
        Resources.Load<Enemy>("Enemies/4_Infected Android"),
        Resources.Load<Enemy>("Enemies/5_Octopus Monster"),
        Resources.Load<Enemy>("Enemies/6_Tentacle Monster"),
        Resources.Load<Enemy>("Enemies/7_The Eye")
      };
      LoadEnemyAbilities();
    }
  }
}