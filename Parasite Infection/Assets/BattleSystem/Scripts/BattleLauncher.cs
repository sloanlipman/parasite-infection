using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<BattleCharacter> players { get; set; }
    public List<BattleCharacter> enemies { get; set; }
    private Player worldPlayer;
    private NPC[] worldNPCs = new NPC[] {};
    private Vector2 worldPosition;
    private int worldSceneIndex;
    private GameObject activatingNPC;
    [SerializeField] private Dialog dialog;
    private bool battleWasLost;

    private void Awake() {

      if (FindObjectsOfType<BattleLauncher>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
      EventController.OnBattleWon += ReturnToWorld;
      EventController.OnBattleLost += LoadLastSave;
      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PrepareBattle(List<BattleCharacter> enemies, List<BattleCharacter> players, Vector2 position, NPC npc) {
      worldPosition = position;
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
      activatingNPC = npc.gameObject;
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

    private void ReturnToWorld() {
      GatewayManager.Instance.SetSpawnPosition(worldPosition);
      SceneManager.LoadScene(worldSceneIndex);
      battleWasLost = true;
    }

    private void LoadLastSave() {
      battleWasLost = true;
      SceneManager.LoadScene(worldSceneIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (battleWasLost) {
        SaveService.Instance.Load();
      }
      battleWasLost = false;

      if (activatingNPC != null) {
        Destroy(activatingNPC);
      }

      Debug.Log("OnSceneLoaded: " + scene.name);
    }
  }
}