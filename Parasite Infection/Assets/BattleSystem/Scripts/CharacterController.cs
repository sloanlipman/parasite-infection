using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class CharacterController : MonoBehaviour {
    // public static CharacterController Instance {get; set;}
    // public List<Enemy> enemiesList = new List<Enemy>();
    // public List<PartyMember> partyMembers = new List<PartyMember>();
    private CharacterDatabase characterDatabase;

    public void Save() {
      ES3.Save<List<PartyMember>>("PartyMembers", characterDatabase.GetPartyMembers(), "characters.ES3");
      ES3.Save<List<Enemy>>("Enemies", characterDatabase.GetEnemies(), "characters.ES3");
    }

    public void Load() {
      characterDatabase.Load();
    }

    public void ClearCharacters() {
      characterDatabase.ClearAll();
    }

    public List<PartyMember> GetPartyMembers() {
      List<PartyMember> currentParty = new List<PartyMember>();
      characterDatabase.GetPartyMembers().ForEach(member => {
        if (member.IsInParty()) {
          currentParty.Add(member);
        }
      });
      return currentParty;
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

      // if (questPanel != null) {
      //   questPanel.gameObject.SetActive(false);
      // }
    }
   }
}