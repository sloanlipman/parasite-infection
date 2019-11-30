using System.Collections.Generic;
using QuestSystem;

public class SaveCattleQuest : Quest {
  void Awake() {
    slug = "SaveCattleQuest";    
    questName = "Defeat 10 Defense Drones";
    itemRewards = new List<string>() {
      "Medkit",
      "Medkit",
      "Medkit", 
      "Energy Pack",
      "Energy Pack",
      "Battery",
      "Integrated Circuit",
      "Fire Core",
      "Water Core"
      };
    goal = new KillGoal(10, 5, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
