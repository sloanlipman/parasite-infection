using System.Collections.Generic;
using QuestSystem;

public class DefeatEvolvedBlobQuest : Quest {

  void Awake() {
    slug = "DefeatEvolvedBlobQuest";    
    questName = "Extract Evolved Blob DNA";
    itemRewards = new List<string>() { "Alien DNA" };
    goal = new KillGoal(1, 41, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.RemoveEvolvedBlob();
    base.GrantReward();
  }
}
