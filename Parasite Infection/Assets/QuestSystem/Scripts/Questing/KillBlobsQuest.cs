using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

public class KillBlobsQuest : Quest {
  void Awake() {
    slug = "KillBlobsQuest";    
    questName = "Kill Blobs";
    description = "Let's see you kill some blobs";
    itemRewards = new List<string>() { "Fire Module" };
    goal = new KillGoal(2, 0, this);
  }

  public override void Complete() {
    EventController.CompleteKillBlobsQuest("Android");
    base.Complete();
  }
}
