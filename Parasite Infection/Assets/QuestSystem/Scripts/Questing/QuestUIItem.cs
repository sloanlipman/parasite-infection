using UnityEngine;
namespace QuestSystem {
  public class QuestUIItem : MonoBehaviour {
    [SerializeField] private UnityEngine.UI.Text questName, questProgress;
    private Quest quest;

    private void Start() {
      EventController.OnQuestCompleted += QuestCompleted;
      EventController.OnQuestProgressChanged += UpdateProgress;
      if (this.quest != null) {
        if (!this.quest.completed) {
          UpdateProgress(this.quest);
        } else {
          QuestCompleted(this.quest);
        }
      }
    }

    private void OnDestroy() {
      EventController.OnQuestProgressChanged -= UpdateProgress;
      EventController.OnQuestCompleted -= QuestCompleted;
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