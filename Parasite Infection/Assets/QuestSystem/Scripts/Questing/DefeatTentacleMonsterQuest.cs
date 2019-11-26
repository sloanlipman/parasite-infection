using System.Collections.Generic;
using QuestSystem;

public class DefeatTentacleMonsterQuest : Quest {
  void Awake() {
    slug = "DefeatTentacleMonsterQuest";    
    questName = "Defeat the Tentacle Monster";
    itemRewards = new List<string>() { "Medic Module", "Medkit", "Energy pack" };
    goal = new KillGoal(1, 31, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartDefeatTentacleMonsterQuestCompletedDialog();
    base.GrantReward();
  }
}
