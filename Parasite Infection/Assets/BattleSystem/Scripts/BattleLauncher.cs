using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLauncher : MonoBehaviour {
    public List<PartyMemberEntry> players { get; set; }
    public List<EnemyEntry> enemies { get; set; }
    private List<PartyMemberEntry> currentParty;
    [SerializeField] private Player player;
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
      if (FindObjectsOfType<BattleLauncher>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
      questController = FindObjectOfType<QuestSystem.QuestController>();
    }

    // public PartyMember GetPartyMember(string name) {
    //   PartyMemberEntry p = CharacterDatabase.Instance.GetPartyMembers().Find(partyMember => partyMember.characterName == name);
    //   return new PartyMember(p);
    // }

    // public Enemy GetEnemy(string name) {
    //   EnemyEntry enemy = CharacterDatabase.Instance.GetEnemies().Find(e => e.characterName == name);
    //   Enemy newEnemy = gameObject.AddComponent<Enemy>();
    //   return newEnemy;
    // }

    public void PrepareBattle(Vector2 position) {
      // currentParty = CharacterDatabase.Instance.GetPartyMembers();
      worldPosition = position;
      Debug.Log("Position is: " + position);
      worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
      // currentParty.ForEach(partyMember => this.players.Add(CharacterDatabase.Instance.GetPartyMember(partyMember.characterName)));
      // this.players = players;
      // this.enemies = enemies;
  // TODO grab real values to be used
      this.players = CharacterDatabase.Instance.GetPartyMembers();
      this.enemies =  CharacterDatabase.Instance.GetEnemies();

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
