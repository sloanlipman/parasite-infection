using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<PartyMember> players { get; set; }
    public List<Enemy> enemies { get; set; }

    public List<Enemy> presetEnemyParty = new List<Enemy>();

    private Player player;
    private int random;
    private int numberOfSteps;

    [SerializeField] private DialogPanel dialog;
    private SceneController sceneController;

    private Vector2 worldPosition;
    private int worldSceneIndex;

    private bool battleWasLost;
    private QuestSystem.QuestController questController;
    private CharacterController characterController;

    public void ResetSteps() {
      numberOfSteps = 0;
    }

    private void Update() {
      if (CanLaunchBattle()) {
        numberOfSteps = numberOfSteps + Mathf.Abs(Mathf.RoundToInt(player.GetRigidbody().velocity.x));
        if (numberOfSteps > 5000) {
          ResetSteps();
          random = Random.Range(0, 10);
          if (random < 1) {
        // if (IsPlayerNotNullAndMoving()) // Uncomment this line (and comment other condition checks) for debugging battle-related tasks. Otherwise leave this line commented.
            PrepareBattle(player.transform.position);
          }
        }
      }
    }

    private bool CanLaunchBattle() {
      return
        questController.HasQuestBeenStarted("KillBlobsQuest") &&
        SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle") &&
        IsPlayerNotNullAndMoving() &&
        !IsGamePaused();
    }

    private bool IsGamePaused() {
      return Time.timeScale == 0;
    }

    private bool IsPlayerMoving() {
       return player.GetRigidbody().velocity != Vector2.zero;
    }

    private bool IsPlayerNotNullAndMoving() {
      return player != null && player.GetRigidbody() != null && IsPlayerMoving();
    }

    private void Awake() {
      SceneManager.sceneLoaded += OnSceneLoaded;
      if (FindObjectsOfType<BattleLauncher>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
      questController = FindObjectOfType<QuestSystem.QuestController>();
      characterController = FindObjectOfType<CharacterController>();
      sceneController = FindObjectOfType<SceneController>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle")) {
        player = FindObjectOfType<Player>();
      }
    }

    public void PrepareBattle(Vector2 position, List<Enemy> enemiesToUse = null, bool splitParty = false) {
      worldPosition = position;
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
      this.players = characterController.GetActiveParty();
      this.enemies = characterController.GetEnemies();

      this.presetEnemyParty = enemiesToUse;

      SceneManager.LoadScene("Battle");
      if (dialog != null) {
        dialog.gameObject.SetActive(false);
      }
    }

    public void Launch() {
      List<Enemy> enemiesToUse = new List<Enemy>();
      if (this.presetEnemyParty != null) {
        enemiesToUse = GenerateEnemyPartyFromPresets();
      } else {
        enemiesToUse = GenerateEnemyParty();
      }

      players.ForEach(player => {
        player.abilities.Clear();
        player.abilitiesList.Clear();
      });
      BattleController.Instance.StartBattle(players, enemiesToUse);
    }

    private List<Enemy> GenerateEnemyParty() {
      List<Enemy> possibleEnemies = new List<Enemy>();
      List<Enemy> enemyParty = new List<Enemy>();
      int playerLevel = 0;
      players.ForEach(p => {
        if (p.level > playerLevel) {
          playerLevel = p.level;
        }
      });

      int currentAct = sceneController.GetCurrentAct();
      int maxEnemyId = 0;
      if (currentAct > 0) {
        switch (currentAct) {
          case 1: {
            maxEnemyId = questController.IsQuestCompleted("DefeatTentacleMonsterQuest") ? 3 : 1;
            break;
          }
          case 2: {
            maxEnemyId = 5;
            break;
          }
        }
      }

      enemies.ForEach(enemy => {
        if (enemy.enemyId <= maxEnemyId) {
          possibleEnemies.Add(enemy);
        }
      });

      int numberOfEnemies = Random.Range(0, 3);
      if (currentAct == 0) {
        numberOfEnemies = 0;
      }

      for (int i = 0; i < numberOfEnemies + 1; i++) {
        int randomIndex = Random.Range(0, possibleEnemies.Count);
        enemyParty.Add(possibleEnemies[randomIndex]);
      }

      return enemyParty;
    }

    private List<Enemy> GenerateEnemyPartyFromPresets() {
      List<Enemy> enemyParty = new List<Enemy>();

      this.presetEnemyParty.ForEach(enemy => {
        enemyParty.Add(characterController.FindEnemyById(enemy.enemyId)); // Get most up to date version of the enemy
      });
      return enemyParty;
    }

    public int GetWorldSceneIndex() {
      return worldSceneIndex;
    }

    public Vector2 GetWorldPosition() {
      return worldPosition;
    }
  }
}
