using System.Collections.Generic;
using QuestSystem;

public class TreasureHunterQuest : Quest {
  void Awake() {
    slug = "TreasureHunterQuest";
    questName = "Treasure Hunter";
    description = "hunt some treasure, yo";
    itemRewards = new List<string>() { "Bananas" };
    goal = new CollectionGoal(2, 0, this);
  }

  public override void Complete() {
    base.Complete();
  }
}
