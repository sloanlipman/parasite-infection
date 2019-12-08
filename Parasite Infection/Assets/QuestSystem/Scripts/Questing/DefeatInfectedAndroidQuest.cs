using System.Collections.Generic;
using QuestSystem;

public class DefeatInfectedAndroidQuest : Quest {
  void Awake() {
    slug = "DefeatInfectedAndroidQuest";    
    questName = "Defeat the Infected Android";
    itemRewards = new List<string>() { "Battery", "Integrated Circuit", "Medkit", "Energy Pack" };
    goal = new KillGoal(1, 34, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartDefeatInfectedAndroidQuestCompletedDialog();
    base.GrantReward();
  }
}
