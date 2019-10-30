using System.Collections.Generic;
using UnityEngine;
namespace QuestSystem {
  public class Quest : MonoBehaviour {
    public string slug;
    public string questName;
    public string description;
    public Goal goal;
    public bool completed;
    public List<string> itemRewards;
    private InventoryController inventoryController;
    private CraftingInventory craftingInventory;
    private ConsumableInventory consumableInventory;

    private void Start() {
      inventoryController = FindObjectOfType<InventoryController>();
      craftingInventory = FindObjectOfType<CraftingInventory>();
      consumableInventory = FindObjectOfType<ConsumableInventory>();
    }

    public virtual void Complete() {
      Debug.Log("Quest completed!");
      if (!this.completed) {
        completed = true;
        EventController.QuestSetToPending(this);
      }
    }

    public void GrantReward() {
      
      Debug.Log("Turning in quest... granting reward");
      foreach(string item in itemRewards) {
        UIInventory inventory = inventoryController.SelectCorrectInventoryUI(item);
        Debug.Log("Rewarded with: " + item);
        craftingInventory.GiveItem(item);
      }
    }
  }
}