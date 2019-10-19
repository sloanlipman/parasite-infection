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

    private void Update() {
      if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle")) {
        if (player.GetRigidbody().velocity != Vector2.zero) {
          random = Random.Range(0, 100);
          numberOfSteps++;
          Debug.Log("# of steps is: " + numberOfSteps);
          Debug.Log("Random # is: " + random);
          if (numberOfSteps > 100) {
            numberOfSteps = 0;
            // if (random >= 50) {
              if (random > 0) {
              Debug.Log("Position is: " + player.transform.position);
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
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle")) {
      player = FindObjectOfType<Player>();
      }
    }

    public void PrepareBattle(Vector2 position) {
      worldPosition = position;
      Debug.Log("Position is: " + position);
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
      this.players =  CharacterDatabase.Instance.GetPartyMembers();
      this.enemies =  CharacterDatabase.Instance.GetEnemies();

      SceneManager.LoadScene("Battle");
      if (dialog != null) {
        dialog.gameObject.SetActive(false);
      }
    }

    public void Launch() {
      List<Enemy> e = new List<Enemy>(){enemies[0]};
      BattleController.Instance.StartBattle(players, e);
    }

    public int GetWorldSceneIndex() {
      return worldSceneIndex;
    }

    public Vector2 GetWorldPosition() {
      return worldPosition;
    }
  }
}
