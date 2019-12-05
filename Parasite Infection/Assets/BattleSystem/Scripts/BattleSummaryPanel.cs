using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace BattleSystem {
  public class BattleSummaryPanel : MonoBehaviour {

    [SerializeField] private Text titleText;
    [SerializeField] private Text xpText;
    [SerializeField] private Text itemText;
    [SerializeField] private Text questText;
    [SerializeField] private Button loadLastSave;
    [SerializeField] private Button backToWorld;
    private SceneController sceneController;
    private BattleLauncher battleLauncher;
    private QuestSystem.QuestController questController;
    private InventoryController inventoryController;
    private List<Item> items = new List<Item>();

    private bool battleWasLost;

    private void Awake() {
      questController = FindObjectOfType<QuestSystem.QuestController>();
      battleLauncher = FindObjectOfType<BattleLauncher>();
      sceneController = FindObjectOfType<SceneController>();
      inventoryController = FindObjectOfType<InventoryController>();
      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ShowVictoryPanel(int xp, List<Item> itemsToGive) {
      gameObject.SetActive(true);
      titleText.text = "Victory!";
      xpText.text = "Experience Gained: " + xp;
      battleWasLost = false;

      SetItemText(itemsToGive);
      SetQuestText(questController.GetPendingQuests());

      loadLastSave.gameObject.SetActive(false);
      if (questController.IsQuestPending("DefeatMalfunctioningAndroidQuest")) {
        sceneController.DefeatMalfunctioningAndroid();
      }
    }

    private void SetItemText(List<Item> itemsToGive) {
      if (itemsToGive.Count > 0) {
        itemText.gameObject.SetActive(true);
        itemText.text = "Items Found: ";
        itemsToGive.ForEach(item => {
          itemText.text += item.itemName + ", ";
          inventoryController.GiveItem(item);
        });

        itemText.text = itemText.text.Substring(0, itemText.text.Length - 2);
      } else {
        itemText.gameObject.SetActive(false);
      }
    }

    private void SetQuestText(List<QuestSystem.Quest> pendingQuests) {
      if (pendingQuests.Count > 0) {
        questText.gameObject.SetActive(true);
        questText.text = "Quests Completed: ";
        pendingQuests.ForEach(quest => questText.text += quest.questName + ", ");
        questText.text = questText.text.Substring(0, questText.text.Length - 2);

      } else {
        questText.gameObject.SetActive(false);
      }
    }

    public void ShowDefeatPanel() {
      gameObject.SetActive(true);
      xpText.text = "";
      titleText.text = "You're dead!";
      battleWasLost = true;
      backToWorld.gameObject.SetActive(false);
    }

    public void LoadLastSave() {
      SaveService.Instance.Load();
    }

    public void EndBattle() {
      GatewayManager.Instance.SetSpawnPosition(battleLauncher.GetWorldPosition());
      SceneManager.LoadScene(battleLauncher.GetWorldSceneIndex());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (!battleWasLost) {
        questController.CompletePendingQuests();
      }
      battleWasLost = false;
      SceneManager.sceneLoaded -= OnSceneLoaded;
    }
  }
}