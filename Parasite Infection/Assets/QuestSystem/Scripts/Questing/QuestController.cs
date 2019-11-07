using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
namespace QuestSystem {
  public class QuestController : MonoBehaviour {
    public List<Quest> assignedQuests = new List<Quest>();
    public List <string> completedQuests = new List<string>();
    [SerializeField] private QuestUIItem questUIItem;
    [SerializeField] private Transform questUIParent;
    private QuestDatabase questDatabase;
    [SerializeField] private QuestPanel questPanel;

    private void Awake() {
      questDatabase = GetComponent<QuestDatabase>();

    }

    private void Start() {
      if (FindObjectsOfType<QuestController>().Length > 1) {
        Destroy(this.gameObject);
      }

      DontDestroyOnLoad(this.gameObject);
      EventController.OnQuestCompleted += RemoveCompletedQuest;

      if (questPanel != null) {
        questPanel.gameObject.SetActive(false);
      }
    }

    public void Save() {
      ES3.Save<Dictionary<string, int[]>>("QuestDatabase", questDatabase.quests, "QuestDatabase.json");
    }

    public void ClearQuests() {
      ClearActiveQuests();
      ClearQuestUI();
      if (assignedQuests.Count > 0) {
        assignedQuests.Clear();
      }
      if (completedQuests.Count > 0) {
        completedQuests.Clear();
      }
      questDatabase.ClearAll();
    }

    public void ClearQuestUI() {
      foreach (QuestUIItem quest in questUIParent.GetComponentsInChildren<QuestUIItem>()) {
        Destroy(quest.gameObject);
      }
    }

    public void ClearActiveQuests() {
      assignedQuests.ForEach(quest => Destroy(quest));
    }

    public void Load() {
      EventController.GameReloaded();
    // Load the DB
      questDatabase.quests = ES3.Load<Dictionary<string, int[]>>("QuestDatabase", "QuestDatabase.json");
      List<string> questNamesToAdd = new List<string>();
    // Get a list of quest slugs that were loaded
      foreach(var quest in questDatabase.quests) {
        questNamesToAdd.Add(quest.Key);
      }
      // Assign IP quests, mark completed quests as completed
        foreach (string questName in questNamesToAdd) {
          if (questDatabase.Completed(questName)) {
            completedQuests.Add(questName);
          } else {
            AssignQuest(questName, true);
          }
        }
    }

    public bool IsQuestCompleted(string questName) {
      return questDatabase.Completed(questName);
    }

    public Quest AssignQuest(string questSlug, bool loadingFromSave = false) {
      bool isNewQuest = false;
      Quest questToAdd = null;

      if (!IsQuestCompleted(questSlug) && FindActiveQuest(questSlug) == null || loadingFromSave) {
        // Add a new quest
        questToAdd = (Quest) gameObject.AddComponent(System.Type.GetType(questSlug));
        assignedQuests.Add(questToAdd);
        try {
          isNewQuest = questDatabase.AddQuest(questToAdd);
        } catch {
          questToAdd.goal.countCurrent = questDatabase.GetCurrentQuestCount(questSlug);
        }

      } else {
        // Quest already exists - Find instance of it
        questToAdd = (Quest) gameObject.GetComponent(System.Type.GetType(questSlug));
      }

      if (isNewQuest || !questDatabase.Completed(questToAdd.slug)) {
        QuestUIItem questUI = Instantiate(questUIItem, questUIParent);

        questUI.Setup(questToAdd);
        return questToAdd;
      } else {
        return null;
      }
    }

    private void RemoveCompletedQuest (Quest quest) {
      assignedQuests.Remove(quest);
      completedQuests.Add(quest.questName);
      Destroy(quest);
    }

    public Quest FindActiveQuest(string questSlug) {
      return GetComponent(System.Type.GetType(questSlug)) as Quest;
    }

    public void CompletePendingQuests() {
      questDatabase.CompletePendingQuests();
      questDatabase.ClearPendingQuests();
    }
  }
}
