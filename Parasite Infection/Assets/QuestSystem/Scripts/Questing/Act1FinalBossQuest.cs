using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class Act1FinalBossQuest : Quest {
  void Awake() {
    slug = "Act1FinalBossQuest";    
    questName = "Defeat the Sketchy Crewmembers";
    itemRewards = new List<string>() { "Battery", "Integrated Circuit", "Medkit", "Energy Pack" };
    goal = new KillGoal(1, 32, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartEndOfAct1Dialog();
    base.GrantReward();
  }
}