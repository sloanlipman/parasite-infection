using System.Collections;
using System.Collections.Generic;
using QuestSystem;

public class SynthesizeTheCureQuest : Quest {

  void Awake() {
    slug = "SynthesizeTheCureQuest";    
    questName = "Synthesize the Cure";
    itemRewards = new List<string>() { "Medkit", "Energy Pack" };
    goal = new CollectionGoal(1, 13, this);
    inventoryController = FindObjectOfType<InventoryController>();
    questController = FindObjectOfType<QuestController>();
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
