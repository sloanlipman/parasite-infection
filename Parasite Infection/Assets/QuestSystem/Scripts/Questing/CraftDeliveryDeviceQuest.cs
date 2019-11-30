using System.Collections;
using System.Collections.Generic;
using QuestSystem;

public class CraftDeliveryDeviceQuest : Quest {

  void Awake() {
    slug = "CraftDeliveryDeviceQuest";    
    questName = "Craft the Delivery Device";
    itemRewards = new List<string>() { "Medkit", "Energy Pack" };
    goal = new CollectionGoal(1, 14, this);
    inventoryController = FindObjectOfType<InventoryController>();
    questController = FindObjectOfType<QuestController>();
    questController.AssignQuest("KillDrillQuest");
    questController.AssignQuest("KillDefenseGolemQuest");
    questController.AssignQuest("KillDroneQuest");
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    base.GrantReward();
  }
}
