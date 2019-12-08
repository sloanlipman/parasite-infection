using System.Collections.Generic;
using QuestSystem;

public class DefeatFinalBossQuest : Quest {
  private bool hasFinalBattleBeenTriggered = false;
  void Awake() {
    slug = "DefeatFinalBossQuest";
    goal = new KillGoal(1, 44, this);
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

    if (sceneController.GetFinalBattleScenario() == 1) {
      goal = new KillGoal(1, 46, this);
      questName = "Annihilate Jake and his Mechs";
    } else {
      goal = new KillGoal(1, 44, this);
      questName = "Defeat the True Parasite Leader";
    }
  }
}
