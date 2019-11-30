﻿using System.Collections.Generic;
using QuestSystem;

public class DefeatDinosaurMonsterQuest : Quest {

  void Awake() {
    slug = "DefeatDinosaurMonsterQuest";    
    questName = "Extract Dinosaur Monster DNA";
    itemRewards = new List<string>() { "Alien DNA" };
    goal = new KillGoal(1, 39, this);
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
