using System.Collections.Generic;
using QuestSystem;

public class KillDefenseGolemQuest : Quest {

  void Awake() {
    slug = "KillDefenseGolemQuest";    
    questName = "Get a stabilizer from a Defense Golem";
    itemRewards = new List<string>() { "Stabilizer" };
    goal = new KillGoal(1, 4, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
