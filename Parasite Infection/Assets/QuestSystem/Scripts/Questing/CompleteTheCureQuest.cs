using System.Collections;
using System.Collections.Generic;
using QuestSystem;

public class CompleteTheCureQuest : Quest {

  void Awake() {
    slug = "CompleteTheCureQuest";    
    questName = "Complete the Cure";
    itemRewards = new List<string>() { "Medkit", "Energy Pack" };
    goal = new CollectionGoal(1, 19, this);
    inventoryController = FindObjectOfType<InventoryController>();
    questController = FindObjectOfType<QuestController>();
    questController.AssignQuest("CraftDeliveryDeviceQuest");
    questController.AssignQuest("SynthesizeTheCureQuest");

    expReward = 300;
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
