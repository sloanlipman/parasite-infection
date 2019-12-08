using System.Collections.Generic;
using QuestSystem;

public class DefeatEnhancedParasiteQuest : Quest {
  void Awake() {
    slug = "DefeatEnhancedParasiteQuest";    
    questName = "Defeat your former ally who is beyond saving";
    goal = new KillGoal(1, 42, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
