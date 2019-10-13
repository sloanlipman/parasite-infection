using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<BattleCharacter> players { get; set; }
    public List<BattleCharacter> enemies { get; set; }

    [SerializeField] private Dialog dialog;

    private Vector2 worldPosition;
    private int worldSceneIndex;

    private GameObject activatingNPC;
    private bool battleWasLost;
    private QuestSystem.QuestController questController;

    private void Awake() {
      if (FindObjectsOfType<BattleLauncher>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
      questController = FindObjectOfType<QuestSystem.QuestController>();
    }

    public void PrepareBattle(List<BattleCharacter> enemies, List<BattleCharacter> players, Vector2 position, NPC npc) {
      worldPosition = position;
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
      activatingNPC =  npc.gameObject; // TODO gets set to null after battle loads  
      this.players = players;
      this.enemies = enemies;
      SceneManager.LoadScene("Battle");
      if (dialog != null) {
        dialog.gameObject.SetActive(false);
      }
    }

    public void Launch() {
      BattleController.Instance.StartBattle(players, enemies);
    }

    public int GetWorldSceneIndex() {
      return worldSceneIndex;
    }

    public Vector2 GetWorldPosition() {
      return worldPosition;
    }

    public GameObject GetActivatingNPC() {
      return activatingNPC;
    }
  }
}
