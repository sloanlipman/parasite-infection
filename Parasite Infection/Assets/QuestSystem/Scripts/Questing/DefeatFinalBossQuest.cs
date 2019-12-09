using System.Collections.Generic;
using QuestSystem;

public class DefeatFinalBossQuest : Quest {
  private bool hasFinalBattleBeenTriggered = false;
  void Awake() {
    slug = "DefeatFinalBossQuest";
    sceneController = FindObjectOfType<SceneController>();
    SetUpFinalBattleQuest();
    sceneController.LaunchFinalBattle();
  }


  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }

  private void SetUpFinalBattleQuest() {

    if (sceneController.GetFinalBattleScenario() == 2) {
      goal = new KillGoal(1, 46, this);
      questName = "Annihilate Jake";
    } else {
      goal = new KillGoal(1, 44, this);
      questName = "Defeat the True Parasite Leader";
    }
  }
}
