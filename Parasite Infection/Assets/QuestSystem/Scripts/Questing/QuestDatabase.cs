using System.Collections.Generic;
using UnityEngine;
namespace QuestSystem {
  public class QuestDatabase : MonoBehaviour {
    public Dictionary<string, int[]> quests = new Dictionary<string, int[]>();

    public void Save() {
      ES3.Save<Dictionary<string, int[]>>("QuestDatabase", quests);
    }

    private void Awake() {
      EventController.OnQuestProgressChanged += UpdateQuestData;
      EventController.OnQuestCompleted += CompleteQuest;
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
      quests[quest.slug] = new int[] { System.Convert.ToInt32(quest.completed), quest.goal.countCurrent};
      Debug.Log("Data updated for: " + quest.slug);
    }

    public int GetCurrentQuestCount(string slug) {
      if (quests.ContainsKey(slug)) {
        return quests[slug][1];
      } else {
        return -1;
      }
    }

    public void CompleteQuest(Quest quest) {
      if (quests.ContainsKey(quest.slug)) {
        quests[quest.slug][0] = 1;
      }
    }

    public bool Completed(string slug) {
      if (quests.ContainsKey(slug)) {
        return System.Convert.ToBoolean(quests[slug][0]);
      }
      return false;
    }
  }
}