using System.Collections;
using System.Collections.Generic;
using QuestSystem;

public class CraftWaterQuest : Quest {

  void Awake() {
    slug = "CraftWaterQuest";    
    questName = "Craft Water Module";
    itemRewards = new List<string>() { "Medkit", "Energy Pack" };
    goal = new CollectionGoal(1, 3, this);
    inventoryController = FindObjectOfType<InventoryController>();
    List<string> waterModuleItems = new List<string>(){
      "Water Core",
      "Battery",
      "Integrated Circuit"
    };
    inventoryController.GiveItems(waterModuleItems);
    expReward = 50;
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
