using UnityEngine;

public class EventController : MonoBehaviour {
  public static event System.Action<int> OnEnemyDied = delegate {};
  public static event System.Action<int> OnItemCollected = delegate { };
  public static event System.Action<QuestSystem.Quest> OnQuestProgressChanged = delegate {};
  public static event System.Action<QuestSystem.Quest> OnQuestCompleted = delegate {};
  public static event System.Action OnBattleCompleted = delegate {};

  public static void EnemyDied(int enemyId) {
    OnEnemyDied(enemyId);
  }

  public static void ItemCollected(int itemID) {
    OnItemCollected(itemID);
  }

  public static void QuestProgressChanged(QuestSystem.Quest quest) {
    OnQuestProgressChanged(quest);
  }

  public static void QuestCompleted(QuestSystem.Quest quest) {
    OnQuestCompleted(quest);
  }

  public static void BattleCompleted() {
    OnBattleCompleted();
  }
}