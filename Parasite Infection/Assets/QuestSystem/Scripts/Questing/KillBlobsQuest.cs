using System.Collections.Generic;
using QuestSystem;

public class KillBlobsQuest : Quest {
  void Awake() {
    slug = "KillBlobsQuest";    
    questName = "Kill Blobs";
    description = "These blobs are everywhere!!! Help!!!!";
    itemRewards = new List<string>() { "Med Pack" };
    goal = new KillGoal(1, 0, this);
  }

    public override void Complete() {
        base.Complete();
    }
}
