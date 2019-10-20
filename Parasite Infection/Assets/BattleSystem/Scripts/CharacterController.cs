using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class CharacterController : MonoBehaviour {
    private CharacterDatabase characterDatabase;

    public void Save() {
      ES3.Save<List<PartyMember>>("PartyMembers", GetPartyMembers(), "Party.ES3");
      ES3.Save<List<string>>("ActiveParty", GetActivePartyList(), "ActiveParty.ES3");
      ES3.Save<List<Enemy>>("Enemies", GetEnemies(), "Enemies.ES3");
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
      return characterDatabase.GetAbility(abilityName);
    }
   }
}