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
    private InventoryController inventoryController;

    public void ResetAllCharacters() {
      enemyList.ForEach(enemy => {
        enemy.level = 0;
        enemy.SetDefaultValues();
      });

      partyMembers.ForEach(member => {
        member.level = 0;
        member.SetDefaultValues();

        for (int i = 0; i < member.equipment.Length; i++) {
          member.equipment[i] = null;
        }
      });

      activePartyMembers.Clear();
      activePartyMembers.Add("Barry");
    }

    public void AddPlayerToParty(string name) {
      activePartyMembers.Add(name);
    }

    public void RemovePlayerFromParty(string name) {
      if (activePartyMembers.Contains(name)) {
      activePartyMembers.Remove(name);
      }
    }

    public void Load() {
      ES3.LoadInto("PartyMembers", "Party.json", partyMembers);
      ES3.LoadInto("Enemies", "Enemies.json", enemyList);
      activePartyMembers.Clear();
      ES3.Load<List<string>>("ActiveParty", "ActiveParty.json").ForEach(member => activePartyMembers.Add(member));

      partyMembers.ForEach(member => {
        foreach(Item item in member.equipment) {
          if (item != null && item.icon == null) {
            item.LoadSprite();
            inventoryController.AddToListOfCurrentItems(item);
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
      inventoryController = FindObjectOfType<InventoryController>();
    }

    public List<PartyMember> GetActiveParty() {
      List<PartyMember> activePartyMemberList = new List<PartyMember>();
      activePartyMembers.ForEach(memberName => {
        partyMembers.ForEach(member => {
          if (member.characterName == memberName) {
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
      PartyMember[] members = Resources.LoadAll<PartyMember>("Players");
      foreach(PartyMember member in members) {
        if (!partyMembers.Contains(member)) {
          partyMembers.Add(member);
          member.SetDefaultValues();
        }
      }
      LoadPlayerAbilities();
      activePartyMembers.Add("Barry");
    }

    public PartyMember FindPartyMemberByName(string name) {
      return partyMembers.Find(member => member.characterName == name);
    }

    public void LoadPlayerAbilities() {
      partyMembers.ForEach(member => {
        for (int i = 0; i < member.GetAbilities().Count; i++) {
          member.GetAbilities().Clear();
          member.AddAbility(GetAbility(member.abilitiesList[i]));
        }
      });
    }

    public void LoadEnemyAbilities() {
      enemyList.ForEach(enemy => {
        for (int i = 0; i < enemy.GetAbilities().Count; i++) {
          enemy.ClearAbilities();
          enemy.AddAbility(GetAbility(enemy.abilitiesList[i]));
        }
      });
    }

    public Enemy FindEnemyById(int enemyId) {
      return enemyList.Find(enemy => enemy.enemyId == enemyId);
    }

    public Enemy FindEnemyByName(string enemyName) {
      return enemyList.Find(enemy => enemy.characterName == enemyName);
    }

    void BuildEnemyDatabase() {
      Enemy[] enemies = Resources.LoadAll<Enemy>("Enemies");

      foreach(Enemy enemy in enemies) {
        if (!enemyList.Contains(enemy)) {
          enemyList.Add(enemy);
          enemy.SetDefaultValues();
        }
      }
        enemyList.ForEach(enemy => {
          enemy.SetDefaultValues();
          LoadEnemyAbilities();
      });
    }
  }
}