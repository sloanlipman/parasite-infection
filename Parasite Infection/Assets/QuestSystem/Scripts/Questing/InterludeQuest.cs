using System.Collections.Generic;
using QuestSystem;

public class InterludeQuest : Quest {

  void Awake() {
    slug = "InterludeQuest";    
    questName = "Defeat the Parasite Leader";
    goal = new KillGoal(1, 43, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.CompleteInterlude();
    base.GrantReward();
  }
}
