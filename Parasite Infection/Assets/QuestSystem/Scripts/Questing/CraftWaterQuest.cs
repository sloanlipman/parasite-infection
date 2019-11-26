using System.Collections;
using System.Collections.Generic;
using QuestSystem;

public class CraftWaterQuest : Quest {

  void Awake() {
    slug = "CraftWaterQuest";    
    questName = "Craft Water Module";
    description = "Alan needs a Water Module!";
    itemRewards = new List<string>() { "Medkit", "Energy pack" };
    goal = new CollectionGoal(1, 3, this);
    inventoryController = FindObjectOfType<InventoryController>();

    inventoryController.GiveItem(9);
    inventoryController.GiveItem(10);
    inventoryController.GiveItem(7);

    expReward = 10;
  }

  public override void Complete() {
    characterController.AddPlayerToParty("Alan");
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartCraftWaterQuestCompletedDialog();
    base.GrantReward();
  }
}
