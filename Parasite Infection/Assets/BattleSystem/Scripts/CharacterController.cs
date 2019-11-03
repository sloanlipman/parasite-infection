﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class CharacterController : MonoBehaviour {
    private CharacterDatabase characterDatabase;

    public void Save() {
      ES3.Save<List<PartyMember>>("PartyMembers", GetPartyMembers(), "Party.json");
      ES3.Save<List<string>>("ActiveParty", GetActivePartyList(), "ActiveParty.json");
      ES3.Save<List<Enemy>>("Enemies", GetEnemies(), "Enemies.json");
    }

    public void Load() {
      characterDatabase.Load();
    }

    private List<PartyMember> GetPartyMembers() {
      return characterDatabase.GetPartyMembers();
    }

    public List<PartyMember> GetActiveParty() {
      return characterDatabase.GetActiveParty();
    }

    private List<string> GetActivePartyList() {
      return characterDatabase.GetActivePartyList();
    }
    
    public List<Enemy> GetEnemies() {
      return characterDatabase.GetEnemies(); 
    }

    public Enemy FindEnemyById(int enemyId) {
      return characterDatabase.FindEnemyById(enemyId);
    }

    private void Awake() {
      characterDatabase = GetComponent<CharacterDatabase>();
    }

    private void Start() {
      if (FindObjectsOfType<CharacterController>().Length > 1) {
        Destroy(this.gameObject);
      }

      DontDestroyOnLoad(this.gameObject);
    }

    public Ability GetAbility(string abilityName) {
      Ability ability = characterDatabase.GetAbility(abilityName);
      return ability;
    }

    public Dictionary<string, Ability> GetAbilityList() {
      return characterDatabase.GetAbilityList();
    }

    public PartyMember FindPartyMemberByName(string characterName) {
      return characterDatabase.FindPartyMemberByName(characterName);
    }

    public void UpdatePlayers(List<BattleCharacter> partyMembers, int xp) {

  // Set all to a default value
      GetActiveParty().ForEach(member => {
        member.health = 1;
        member.energyPoints = member.maxEnergyPoints / 2; // Recover a bit if they died. Should capture the last value from before they died though.
      });
  // Survivors only
      partyMembers.ForEach(member => {
        PartyMember p = characterDatabase.FindPartyMemberByName(member.characterName);
        p.health = member.health;
        p.energyPoints = member.energyPoints;
        p.experience += xp;
        LevelUp(p);
      });
    }

    public void LevelUp(PartyMember member) {
      int xpToNextLevel = NextLevel(member.level);
      if (member.experience >= xpToNextLevel) {
        member.level++;
      }
    }

    public int NextLevel(int currentLevel) {
      return Mathf.RoundToInt(0.04f * (currentLevel^3) + 0.8f * (currentLevel^2) + 2 * currentLevel);
    }
  }
}