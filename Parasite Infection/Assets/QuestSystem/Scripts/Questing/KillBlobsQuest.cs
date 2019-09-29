using System.Collections.Generic;
using QuestSystem;

public class KillBlobsQuest : Quest {
  void Awake() {
    slug = "KillBlobsQuest";    
    questName = "Kill Blobs";
    description = "Let's see you kill some blobs";
    itemRewards = new List<string>() { "Med Pack" };
    goal = new KillGoal(2, 0, this);
  }

    public override void Complete() {
        base.Complete();
    }
}
