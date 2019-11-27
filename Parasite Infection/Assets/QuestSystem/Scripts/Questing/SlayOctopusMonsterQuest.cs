using System.Collections.Generic;
using QuestSystem;

public class SlayOctopusMonsterQuest : Quest {

  void Awake() {
    slug = "SlayOctopusMonsterQuest";    
    questName = "Slay the Octopus Monster";
    itemRewards = new List<string>() { "Medkit", "Energy pack", "Medkit", "Energy pack" };
    goal = new KillGoal(2, 0, this);
    expReward = 5;
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartDefeatOctopusMonsterQuestCompletedDialog();

    base.GrantReward();
  }
}
