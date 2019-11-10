using System.Collections.Generic;
using QuestSystem;

public class KillBlobsQuest : Quest {

  void Awake() {
    slug = "KillBlobsQuest";    
    questName = "Kill Blobs";
    description = "Let's see you kill some blobs";
    itemRewards = new List<string>() { "Fire Module", "Medkit", "Energy pack" };
    goal = new KillGoal(2, 0, this);
    expReward = 5;
  }

  public override void Complete() {
    characterController.AddPlayerToParty("Android");
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartKillBlobsQuestCompletedDialog();
    base.GrantReward();
  }
}
