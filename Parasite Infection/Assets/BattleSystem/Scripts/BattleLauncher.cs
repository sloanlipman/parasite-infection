using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<BattleCharacter> Players { get; set; }
    public List<BattleCharacter> Enemies { get; set; }

    private Vector2 worldPosition;
    private int worldSceneIndex;
    private GameObject activatingNPC;
    
    private void Awake() {

      if (FindObjectsOfType<BattleLauncher>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
      EventController.OnBattleCompleted += ReturnToWorld;
    }
    
    public void PrepareBattle(List<BattleCharacter> enemies, List<BattleCharacter> players, Vector2 position, NPC npc) {
      worldPosition = position;
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
      activatingNPC = npc.gameObject;
      Players = players;
      Enemies = enemies;
      SceneManager.LoadScene("Battle");
    }

    public void Launch() {
      BattleController.Instance.StartBattle(Players, Enemies);
    }

    private void ReturnToWorld() {
      GatewayManager.Instance.SetSpawnPosition(worldPosition);
      SceneManager.LoadScene(worldSceneIndex);
      if (activatingNPC != null) {
        Destroy(activatingNPC);
      }

    }
  }
}