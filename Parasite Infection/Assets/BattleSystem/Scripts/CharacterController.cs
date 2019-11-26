using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class CharacterController : MonoBehaviour {
    private CharacterDatabase characterDatabase;

    public void ResetAllCharacters() {
      characterDatabase.ResetAllCharacters();
    }

    public void Save() {
      ES3.Save<List<PartyMember>>("PartyMembers", GetPartyMembers(), "Party.json");
      ES3.Save<List<string>>("ActiveParty", GetActivePartyList(), "ActiveParty.json");
      ES3.Save<List<Enemy>>("Enemies", GetEnemies(), "Enemies.json");
    }

    public void Load() {
      characterDatabase.Load();
    }

    public int GetExperience(string name) {
      PartyMember p = FindPartyMemberByName(name);
      return p.experience;
    }

    public void SetExperience(int xp, string name) {
      PartyMember p = FindPartyMemberByName(name);
      p.experience = xp;
      LevelUp(p);
    }

    public void AddPlayerToParty(string name) {
      characterDatabase.AddPlayerToParty(name);
    }

    public void RemovePlayerFromParty(string name) {
      characterDatabase.RemovePlayerFromParty(name);
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
      BattleController.Instance.GetDeadPlayersList().ForEach(member => {
        PartyMember p = characterDatabase.FindPartyMemberByName(member.characterName);
          p.health = 1;
          p.energyPoints = member.energyPoints;
      });

  // Survivors only
      partyMembers.ForEach(member => {
        PartyMember p = characterDatabase.FindPartyMemberByName(member.characterName);
        p.health = member.health;
        p.energyPoints = member.energyPoints;
        p.experience += xp;
        Debug.Log(p.characterName + " Got XP: " + xp + ". Current XP is: " + p.experience);
      });
    }

    public void LevelUp(PartyMember member) {
      bool shouldKeepLevelingUp = true;
      int xpToNextLevel = NextLevel(member.level);
      while (shouldKeepLevelingUp) {
        if (member.experience >= xpToNextLevel) {
          member.LevelUp();
          if (member.level % 2 == 0) {
            LevelUpEnemies();
            member.AddUpgradePoint();
          }
        }
        xpToNextLevel = NextLevel(member.level);
          if (member.experience < xpToNextLevel) {
            shouldKeepLevelingUp = false;
        }
      }
      member.SetModSlots();
    }

    public void LevelUpEnemies() {
      GetEnemies().ForEach(enemy => {
      int random = Random.Range(0, 10);
        if (random >= 8) {
          enemy.LevelUp();
        }
      });
    }

    public int NextLevel(int currentLevel) {
      return Mathf.RoundToInt(0.04f * Mathf.Pow(currentLevel, 3) + 0.8f * Mathf.Pow(currentLevel, 2) + 2 * currentLevel);
    }
  }
}
