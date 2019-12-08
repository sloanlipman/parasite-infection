﻿using System.Collections;
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
  }
}
