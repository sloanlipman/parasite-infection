using System.Collections.Generic;
using QuestSystem;

public class KillDrillQuest : Quest {

  void Awake() {
    slug = "KillDrillQuest";    
    questName = "Get an injector from a Drone";
    itemRewards = new List<string>() { "Injector" };
    goal = new KillGoal(1, 4, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
