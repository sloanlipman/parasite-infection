using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleSystem {
  public class BattleLaunchCharacter : MonoBehaviour {
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private BattleLauncher launcher;
    private List<PartyMember> players;
    private CharacterController characterController;
    private SceneController sceneController;
    private bool battleIsCompleted = false;
    private bool battleWasLaunched = false;

    public bool isBoss;
    public bool isMalfunctioningAndroid;
    public bool isInfectedAndroid;
    public bool isParasiteLeader;
    public bool isFinalBoss;
    public bool isInfectedCrewMember;

    void Start() {
      launcher = FindObjectOfType<BattleLauncher>();
      characterController = FindObjectOfType<CharacterController>();
      sceneController = FindObjectOfType<SceneController>();
      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PrepareBattle(Character character) {
      if (!battleIsCompleted) {
        players = characterController.GetActiveParty();
        battleWasLaunched = true;
        bool showDialog = false;
        SetUpBossMusic();
        launcher.PrepareBattle(character.transform.position, showDialog, enemies);
      }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (battleWasLaunched &&
        SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle")) {
        battleIsCompleted = true;
      }
    }

    public void AddEnemy(Enemy enemy) {
      if (enemies.Count < 3) {
        enemies.Add(enemy);
      }
    }

    private void SetUpBossMusic() {
      if (isBoss) {
        sceneController.setGeneralBossFight();
      } else if (isMalfunctioningAndroid) {
        sceneController.SetMalfunctioningAndroidFight();
      } else if (isInfectedAndroid) {
        sceneController.SetInfectedAndroidFight();
      } else if (isInfectedCrewMember) {
        sceneController.SetInfectedCrewMemberFight();
      } else if (isParasiteLeader) {
        sceneController.SetParasiteLeaderFight();
      } else if (isFinalBoss) {
        sceneController.SetFinalBossFight();
      } else {
        sceneController.ResetBossFightBoolean();
      }
    }
  }
}
