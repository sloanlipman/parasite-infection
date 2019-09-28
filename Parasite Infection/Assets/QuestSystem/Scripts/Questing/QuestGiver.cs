using UnityEngine;
namespace QuestSystem {
  public class QuestGiver : MonoBehaviour {

    [SerializeField] private string questName;
    private QuestController questController;
    private Quest quest;

    void Start() {
      questController = FindObjectOfType<QuestController>();
    }

    public void GiveQuest() {
      quest = questController.AssignQuest(questName);
      GetComponent<UnityEngine.UI.Button>().image.color = Color.red;
      GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
  }
}