using UnityEngine;

namespace QuestSystem {
  public class KillGoal : Goal {
    public int enemyId;

    public KillGoal() {}

    public KillGoal(int amountNeeded, int enemyId, Quest quest) {
      countCurrent = 0;
      countNeeded = amountNeeded;
      completed = false;
      this.enemyId = enemyId;
      this.quest = quest;
      EventController.OnEnemyDied += EnemyKilled;
      EventController.OnGameReloaded += DestroyKillGoal;
    }

    public void DestroyKillGoal() {
      EventController.OnEnemyDied -= EnemyKilled;
    }

    void EnemyKilled(int enemyId) {
      if (this.enemyId == enemyId) {
        Increment(1);
        if (this.completed) {
          EventController.OnEnemyDied -= EnemyKilled;
        }
      }
    }  
  }
}