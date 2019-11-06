using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<PartyMember> players { get; set; }
    public List<Enemy> enemies { get; set; }

    private Player player;
    private int random;
    private int numberOfSteps;

    [SerializeField] private DialogPanel dialog;

    private Vector2 worldPosition;
    private int worldSceneIndex;

    private bool battleWasLost;
    private QuestSystem.QuestController questController;
    private CharacterController characterController;

    public void ResetSteps() {
      numberOfSteps = 0;
    }

    private void Update() {
      if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle") && player != null && player.GetRigidbody() != null) {
        if (Time.timeScale != 0 && player.GetRigidbody().velocity != Vector2.zero) {
          random = Random.Range(0, 100);
          numberOfSteps++;
          if (numberOfSteps > 100) {
            ResetSteps();
              if (random > 0) {
              PrepareBattle(player.transform.position);
            }
          }
        }
      }
    }

    private void Awake() {
      SceneManager.sceneLoaded += OnSceneLoaded;
      if (FindObjectsOfType<BattleLauncher>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
      questController = FindObjectOfType<QuestSystem.QuestController>();
      characterController = FindObjectOfType<CharacterController>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle")) {
        player = FindObjectOfType<Player>();
      }
    }

    public void PrepareBattle(Vector2 position) {
      worldPosition = position;
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
      this.players = characterController.GetActiveParty();
      this.enemies = characterController.GetEnemies();

      SceneManager.LoadScene("Battle");
      if (dialog != null) {
        dialog.gameObject.SetActive(false);
      }
    }

    public void Launch() {
      List<Enemy> enemiesToUse = GenerateEnemyParty();
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

      enemies.ForEach(enemy => {
        if (enemy.enemyId <= playerLevel / 3) {
          possibleEnemies.Add(enemy);
        }
      });

      int numberOfEnemies = Random.Range(1,3);

      for (int i = 0; i < numberOfEnemies; i++) {
        int randomIndex = Random.Range(0, possibleEnemies.Count - 1);
        enemyParty.Add(possibleEnemies[randomIndex]);
      }

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
