using System.Collections.Generic;
using QuestSystem;

public class DefeatMalfunctioningAndroidQuest : Quest {

  void Awake() {
    slug = "DefeatMalfunctioningAndroidQuest";    
    questName = "Defeat Malfunctioning Android";
    itemRewards = new List<string>() { "Medkit", "Energy Pack", "Medkit", "Energy Pack" };
    goal = new KillGoal(1, 38, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartDefeatMalfunctioningAndroidQuestCompletedDialog();
    base.GrantReward();
  }
}
