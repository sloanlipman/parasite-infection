using System.Collections.Generic;
using UnityEngine;
namespace QuestSystem {
  public class QuestDatabase : MonoBehaviour {
    public Dictionary<string, int[]> quests = new Dictionary<string, int[]>();

    private void Awake() {
      EventController.OnQuestProgressChanged += UpdateQuestData;
      EventController.OnQuestCompleted += CompleteQuest;
    }

    public bool AddQuest(Quest quest) {
      bool wasQuestAdded = false;
      if (!Completed(quest.questName)) {
        quests.Add(quest.questName, new int[] { 0, 0 }); // First is completion status (T or F), second one is the count
        wasQuestAdded = true;
      }
      return wasQuestAdded;
    }

    public void UpdateQuestData(Quest quest) {
      quests[quest.questName] = new int[] { System.Convert.ToInt32(quest.completed), quest.goal.countCurrent};
      Debug.Log("Data updated for: " + quest.questName);
    }

    public void CompleteQuest(Quest quest) {
      if (quests.ContainsKey(quest.questName)) {
        quests[quest.questName][0] = 1;
      }
    }

    public bool Completed(string questName) {
      if (quests.ContainsKey(questName)) {
        return System.Convert.ToBoolean(quests[questName][0]);
      }
      return false;
    }
  }
}