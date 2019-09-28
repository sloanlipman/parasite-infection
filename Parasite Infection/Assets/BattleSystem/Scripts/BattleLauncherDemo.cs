using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleLauncherDemo : MonoBehaviour {
    [SerializeField] private List<BattleCharacter> players, enemies;
    [SerializeField] private BattleLauncher launcher;

    public void Launch() {
      Debug.Log("Launching from demo!!");
      launcher.PrepareBattle(enemies, players, Vector2.zero);
    }
  }
}