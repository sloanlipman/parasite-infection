using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace BattleSystem {
  public class BattleSummaryPanel : MonoBehaviour {

    [SerializeField] private Text titleText;
    [SerializeField] private Text xpText;
    [SerializeField] private Text itemText;
    [SerializeField] private Button loadLastSave;
    [SerializeField] private Button backToWorld;
    private SceneController sceneController;
    private BattleLauncher battleLauncher;
    private QuestSystem.QuestController questController;
    private InventoryController inventoryController;
    private List<Item> items = new List<Item>();

    private bool battleWasLost;
    private List<BattleCharacter> alivePlayers = new List<BattleCharacter>();
    private int xpToReward;

    private void Awake() {
      alivePlayers.Clear();
      questController = FindObjectOfType<QuestSystem.QuestController>();
      battleLauncher = FindObjectOfType<BattleLauncher>();
      sceneController = FindObjectOfType<SceneController>();
      inventoryController = FindObjectOfType<InventoryController>();
      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ShowVictoryPanel(int xp, List<Item> itemsToGive, List<BattleCharacter> alivePlayers) {
      gameObject.SetActive(true);
      titleText.text = "Victory!";
      xpText.text = "Experience Gained: " + xp;
      battleWasLost = false;
      xpToReward = xp;
      this.alivePlayers = alivePlayers;

      SetItemText(itemsToGive);

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

    public void ShowDefeatPanel() {
      gameObject.SetActive(true);
      xpText.text = "";
      titleText.text = "You're dead!";
      battleWasLost = true;
      backToWorld.gameObject.SetActive(false);
      itemText.gameObject.SetActive(false);
    }

    public void LoadLastSave() {
      SaveService.Instance.Load();
    }

    public void EndBattle() {
      GatewayManager.Instance.SetSpawnPosition(battleLauncher.GetWorldPosition());
      SceneManager.LoadScene(battleLauncher.GetWorldSceneIndex());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (scene != SceneManager.GetSceneByName("Battle")) {
        if (!battleWasLost) {
          questController.CompletePendingQuests(alivePlayers, xpToReward);
        }
        battleWasLost = false;
        SceneManager.sceneLoaded -= OnSceneLoaded;
      }
    }
  }
}