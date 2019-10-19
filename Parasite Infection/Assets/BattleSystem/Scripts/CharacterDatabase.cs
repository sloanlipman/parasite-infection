using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class CharacterDatabase : MonoBehaviour {
    public static CharacterDatabase Instance {get; set;}
    private List<Enemy> enemyList = new List<Enemy>();
    private List<PartyMember> partyMembers = new List<PartyMember>();

    public List<Ability> abilityList = new List<Ability>();
    public void AddPartyMember(PartyMember partyMember) {
    }

    public List<PartyMember> GetPartyMembers() {
      return partyMembers;
    }

    public List<Enemy> GetEnemies() {
      return enemyList;
    }

    public void Load() {
      partyMembers = ES3.Load<List<PartyMember>>("PartyMembers", "characters.es3");
      enemyList = ES3.Load<List<Enemy>>("PartyMembers", "characters.es3");
    }

    public void ClearAll() {
      partyMembers.Clear();
      enemyList.Clear();
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
    partyMembers = new List<PartyMember>() {
      Resources.Load<PartyMember>("Players/Barry"),
      Resources.Load<PartyMember>("Players/Android"),
      Resources.Load<PartyMember>("Players/Alan")
    };

    partyMembers[0].ToggleInParty(true);

  }

  void BuildEnemyDatabase() {
    enemyList = new List<Enemy>() {
      Resources.Load<Enemy>("Enemies/0_Blob"),
      Resources.Load<Enemy>("Enemies/Drill"),
      Resources.Load<Enemy>("Enemies/2_Drone"),
      Resources.Load<Enemy>("Enemies/3_Hatchling"),
      Resources.Load<Enemy>("Enemies/4_Infected Android"),
      Resources.Load<Enemy>("Enemies/5_Octopus Monster"),
      Resources.Load<Enemy>("Enemies/6_Tentacle Monster"),
      Resources.Load<Enemy>("Enemies/7_The Eye")
    };
   }
  }
}