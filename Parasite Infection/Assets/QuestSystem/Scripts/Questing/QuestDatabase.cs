using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuestSystem {
  public class QuestDatabase : MonoBehaviour {
    public Dictionary<string, int[]> quests = new Dictionary<string, int[]>();
    public List<Quest> pendingQuests = new List<Quest>();
  
    private void Start() {
      EventController.OnQuestProgressChanged += UpdateQuestData;
      EventController.OnQuestSetToPending += MarkQuestAsPending;
    }

    private void OnDestroy() {
      EventController.OnQuestProgressChanged -= UpdateQuestData;
      EventController.OnQuestSetToPending -= MarkQuestAsPending;
    }

    public bool AddQuest(Quest quest) {
      bool wasQuestAdded = false;
      if (!Completed(quest.slug)) {
        quests.Add(quest.slug, new int[] { 0, 0 }); // First is completion status (T or F), second one is the count
        wasQuestAdded = true;
      }
      return wasQuestAdded;
    }

    public void UpdateQuestData(Quest quest) {
      if (quest != null) {
        quests[quest.slug] = new int[] { System.Convert.ToInt32(quest.completed), quest.goal.countCurrent};
      }
    }

    public int GetCurrentQuestCount(string slug) {
      if (quests.ContainsKey(slug)) {
        return quests[slug][1];
      } else {
        return -1;
      }
    }

    public void CompletePendingQuests() {
      if (pendingQuests.Count > 0) {
        pendingQuests.ForEach(quest => {
          if (quest != null) {
            Debug.Log("Pending quest: " + quest.questName);
            quest.GrantReward();
            EventController.QuestCompleted(quest);
          }
        });
      }
    }

    public bool IsQuestPending(string slug) {
      Quest pendingQuest = null;
      if (pendingQuests.Count > 0) {
        pendingQuest = pendingQuests.Find(quest => quest.slug == slug);
      }
      return pendingQuest != null;
    }

    public void ClearPendingQuests() {
      if (pendingQuests.Count > 0) {
        pendingQuests.Clear();
      }
    }

    public void ClearAll() {
      ClearPendingQuests();
      if (quests.Count > 0 ) {
        quests.Clear();
      }
    }

    public void MarkQuestAsPending(Quest quest) {
      if (quests.ContainsKey(quest.slug)) {
        quests[quest.slug][0] = 1;
        pendingQuests.Add(quest);
      }

      if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Battle")) {
        CompletePendingQuests();
      }
    }

    public bool Completed(string slug) {
      Debug.Log("Quests contains key " + slug + "?" + quests.ContainsKey(slug));
      if (quests.ContainsKey(slug)) {
        Debug.Log("Is it completed" + System.Convert.ToBoolean(quests[slug][0]));
        return System.Convert.ToBoolean(quests[slug][0]);
      }
      return false;
    }
  }
}