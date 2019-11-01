using UnityEngine;
namespace QuestSystem {
  public class QuestUIItem : MonoBehaviour {
    [SerializeField] private UnityEngine.UI.Text questName, questProgress;
    private Quest quest;

    private void Start() {
      EventController.OnQuestCompleted += QuestCompleted;
      EventController.OnQuestProgressChanged += UpdateProgress;
    }

    private void OnDestroy() {
      EventController.OnQuestProgressChanged -= UpdateProgress;
      EventController.OnQuestCompleted -= QuestCompleted;
    }

    private void OnEnable() {
      if (this.quest != null) {
        if (!this.quest.goal.completed) {
          UpdateProgress(this.quest);
        } else {
          QuestCompleted(this.quest);
        }
      } else {
        Transform parent = GameObject.FindGameObjectWithTag("UI/Quest Item Parent").transform;
        foreach (QuestUIItem quest in parent.GetComponentsInChildren<QuestUIItem>()) {
          Destroy(quest.gameObject);
        }
      }

    }

    public void Setup(Quest questToSetup) {
      quest = questToSetup;
      questName.text = questToSetup.questName;
      questProgress.text = questToSetup.goal.countCurrent + "/" + questToSetup.goal.countNeeded;
    }

    public void UpdateProgress(Quest quest) {
      if (this.quest.questName == quest.questName) {
        questProgress.text = quest.goal.countCurrent + "/" + quest.goal.countNeeded;
      }
    }

    public void QuestCompleted(Quest quest) {
      if (this.quest == quest) {
          Destroy(this.gameObject);
      }
    }
  }
}