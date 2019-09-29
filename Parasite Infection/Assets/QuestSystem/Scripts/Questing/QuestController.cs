using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuestSystem {
  public class QuestController : MonoBehaviour {
    public List<Quest> assignedQuests = new List<Quest>();
    public List <string> completedQuests = new List<string>();

        
    [SerializeField] private QuestUIItem questUIItem;
    // [SerializeField] private GameObject questPanel;
    [SerializeField] private Transform questUIParent;

    private QuestDatabase questDatabase;

    private void Start() {
      if (FindObjectsOfType<QuestController>().Length > 1) {
        Destroy(this.gameObject);
      }

      DontDestroyOnLoad(this.gameObject);
      SceneManager.sceneLoaded += Populate;
      EventController.OnQuestCompleted += RemoveCompletedQuest;
      questDatabase = GetComponent<QuestDatabase>();
      // questPanel = GameObject.FindWithTag("UI/Quest Panel");
    }

    private void Update() {
      if (Input.GetButtonDown("ToggleQuest"))  {
        Transform viewPort = questUIParent.transform.parent;
        Transform scrollView = viewPort.transform.parent;
        GameObject questPanel = scrollView.transform.parent.gameObject;
        questPanel.SetActive(!questPanel.activeSelf);
      }
      
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

    private void Populate(Scene scene, LoadSceneMode sceneMode) {
      questUIParent = GameObject.FindGameObjectWithTag("UI/Quest Item Parent").transform;
      if (assignedQuests.Count > 0) {
        for (int i = 0; i < assignedQuests.Count; i++) {
          AssignQuest(assignedQuests[i].slug); //Reassign from a UI perspective
        }
      }
    }
  }
}