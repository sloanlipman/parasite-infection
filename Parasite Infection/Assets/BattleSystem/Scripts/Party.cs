using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class Party : MonoBehaviour {
    public static Party Instance {get; set;}
    [SerializeField] private List<BattleCharacter> currentParty;
      // Start is called before the first frame update

    public List<BattleCharacter> GetPartyMembers() {
      return currentParty;
    }
  
    void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(this.gameObject);
      } else {
        Instance = this;
      }

    DontDestroyOnLoad(this.gameObject);
  }

    // Update is called once per frame
    void Update() {

    }
  }
}