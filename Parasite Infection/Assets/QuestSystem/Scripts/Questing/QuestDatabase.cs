﻿using System.Collections.Generic;
using UnityEngine;
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
          quest.GrantReward();
          EventController.QuestCompleted(quest);
        });
      }
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
    }

    public bool Completed(string slug) {
      if (quests.ContainsKey(slug)) {
        return System.Convert.ToBoolean(quests[slug][0]);
      }
      return false;
    }
  }
}