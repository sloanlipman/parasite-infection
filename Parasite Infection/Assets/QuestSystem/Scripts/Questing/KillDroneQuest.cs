using System.Collections.Generic;
using QuestSystem;

public class KillDroneQuest : Quest {

  void Awake() {
    slug = "KillDroneQuest";    
    questName = "Get a Drone Power Source";
    itemRewards = new List<string>() { "Power Source" };
    goal = new KillGoal(1, 5, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
