using System.Collections.Generic;
using QuestSystem;

public class KillBlobsQuest : Quest {

  void Awake() {
    slug = "KillBlobsQuest";    
    questName = "Kill Blobs";
    itemRewards = new List<string>() { "Fire Module", "Medkit", "Energy Pack" };
    goal = new KillGoal(1, 0, this);
    expReward = 10;
    sceneController = FindObjectOfType<SceneController>();
  }

  void Start() {
    sceneController.ActivateBlob();
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartKillBlobsQuestCompletedDialog();
    base.GrantReward();
  }
}
