using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<BattleCharacter> players { get; set; }
    public List<BattleCharacter> enemies { get; set; }
    private List<BattleCharacter> currentParty;
    private Player player;
    private int random;
    private int numberOfSteps;

    [SerializeField] private DialogPanel dialog;

    private Vector2 worldPosition;
    private int worldSceneIndex;

    private bool battleWasLost;
    private QuestSystem.QuestController questController;


    
    void Start() {
      player = FindObjectOfType<Player>();
    }

    private void Update() {
      currentParty = Party.Instance.GetPartyMembers();
      if (player.GetRigidbody().velocity != Vector2.zero) {
        random = Random.Range(0, 100);
        numberOfSteps++;
        Debug.Log("# of steps is: " + numberOfSteps);
        // Debug.Log("Random # is: " + random);
        if (numberOfSteps > 100) {
          numberOfSteps = 0;
          if (random >= 50) {
            PrepareBattle(null, this.currentParty, player.transform.position);
          }
        }
      }
    }

    private void Awake() {
      if (FindObjectsOfType<BattleLauncher>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
      questController = FindObjectOfType<QuestSystem.QuestController>();
    }

    public void PrepareBattle(List<BattleCharacter> enemies, List<BattleCharacter> players, Vector2 position) {
      worldPosition = position;
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
  }
}
