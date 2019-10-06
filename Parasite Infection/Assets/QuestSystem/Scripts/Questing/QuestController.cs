using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
  public class QuestController : MonoBehaviour {
    public List<Quest> assignedQuests = new List<Quest>();
    public List <string> completedQuests = new List<string>();
    [SerializeField] private QuestUIItem questUIItem;
    [SerializeField] private Transform questUIParent;
    private QuestDatabase questDatabase;
    [SerializeField] private QuestPanel questPanel;

    private void Start() {
      if (FindObjectsOfType<QuestController>().Length > 1) {
        Destroy(this.gameObject);
      }

      DontDestroyOnLoad(this.gameObject);
      EventController.OnQuestCompleted += RemoveCompletedQuest;
      questDatabase = GetComponent<QuestDatabase>();

      if (questPanel != null) {
        questPanel.gameObject.SetActive(false);
      }
    }

    public void Save() {
      ES3.Save<List<Quest>>("AssignedQuests", assignedQuests);
      ES3.Save<List<string>>("CompletedQuests", completedQuests);
    }

    public void Load() {
      assignedQuests = ES3.Load<List<QuestSystem.Quest>>("AssignedQuests");
      completedQuests = ES3.Load<List<string>>("CompletedQuests");
    }

    public bool IsQuestCompleted(string questName) {
      return questDatabase.Completed(questName);
    }

    public Quest AssignQuest(string questSlug) {
      bool wasQuestAdded = false;
      Quest questToAdd = null;

      if (FindActiveQuest(questSlug) == null) {
        questToAdd = (Quest) gameObject.AddComponent(System.Type.GetType(questSlug));
        assignedQuests.Add(questToAdd);
        wasQuestAdded = questDatabase.AddQuest(questToAdd);

      } else {
        questToAdd = (Quest) gameObject.GetComponent(System.Type.GetType(questSlug));
      }

      if (wasQuestAdded || !questDatabase.Completed(questToAdd.questName)) {
        QuestUIItem questUI = Instantiate(questUIItem, questUIParent);

        questUI.Setup(questToAdd);
        return questToAdd;
      } else {
        return null;
      }
    }

    private void RemoveCompletedQuest (Quest quest) {
      assignedQuests.Remove(quest);
      Destroy(quest);
    }

    public Quest FindActiveQuest(string questSlug) {
      return GetComponent(System.Type.GetType(questSlug)) as Quest;
    }
  }
}