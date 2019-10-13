using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace BattleSystem {
  public class BattleSummaryPanel : MonoBehaviour {

    [SerializeField] private Text titleText;
    [SerializeField] private Button loadLastSave;
    [SerializeField] private Button backToWorld;
    private BattleLauncher battleLauncher;
    private QuestSystem.QuestController questController;

    private bool battleWasLost;

    private void Awake() {
      questController = FindObjectOfType<QuestSystem.QuestController>();
      battleLauncher = FindObjectOfType<BattleLauncher>();
      SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void ShowVictoryPanel() {
      gameObject.SetActive(true);
      titleText.text = "Victory!";
      battleWasLost = false;
      loadLastSave.gameObject.SetActive(false);
    }

    public void ShowDefeatPanel() {
      gameObject.SetActive(true);
      titleText.text = "You're dead!";
      battleWasLost = true;
      backToWorld.gameObject.SetActive(false);
    }

    public void LoadLastSave() {
      SceneManager.LoadScene(battleLauncher.GetWorldSceneIndex());
    }

    public void EndBattle() {
      GatewayManager.Instance.SetSpawnPosition(battleLauncher.GetWorldPosition());
      SceneManager.LoadScene(battleLauncher.GetWorldSceneIndex());

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (battleWasLost) {
        SaveService.Instance.Load();
      } else {
        questController.CompletePendingQuests();
      }
      battleWasLost = false;
      SceneManager.sceneLoaded -= OnSceneLoaded;

      if (battleLauncher.GetActivatingNPC() != null) {
        Debug.Log("Found NPC");
        Destroy(battleLauncher.GetActivatingNPC());
      }

    }
  }
}