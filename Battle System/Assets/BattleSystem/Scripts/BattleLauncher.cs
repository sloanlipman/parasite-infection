using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<BattleCharacter> Players { get; set; }
    public List<BattleCharacter> Enemies { get; set; }
    
    private void Awake() {
      DontDestroyOnLoad(this);
    }
    
    public void PrepareBattle(List<BattleCharacter> enemies, List<BattleCharacter> players) {
      Players = players;
      Enemies = enemies;
      UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
    }
    public void Launch() {
      BattleController.Instance.StartBattle(Players, Enemies);
    }
  }
}