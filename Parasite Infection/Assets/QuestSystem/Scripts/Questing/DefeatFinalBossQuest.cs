using System.Collections.Generic;
using QuestSystem;

public class DefeatFinalBossQuest : Quest {
  private bool hasFinalBattleBeenTriggered = false;
  void Awake() {
    slug = "DefeatFinalBossQuest";    
    questName = "Defeat the Final Challenge";
    // itemRewards = new List<string>() { "Medic Module", "Medkit", "Energy Pack" };
    goal = new KillGoal(1, 44, this);
  }

  private void Update() {
    if (sceneController != null && !hasFinalBattleBeenTriggered) {
      sceneController.TriggerFinalBattle();
      hasFinalBattleBeenTriggered = true;
    }
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    // sceneController.StartDefeatEnhancedParasiteQuestCompletedDialog();
    base.GrantReward();
  }
}
