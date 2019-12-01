﻿using System.Collections.Generic;
using UnityEngine;
namespace QuestSystem {
  public class Quest : MonoBehaviour {
    public string slug;
    public string questName;
    public string description;
    public Goal goal;
    public bool completed;
    public List<string> itemRewards;
    public int expReward;
    protected InventoryController inventoryController;
    protected CraftingInventory craftingInventory;
    protected ConsumableInventory consumableInventory;
    protected BattleSystem.CharacterController characterController;
    protected SceneController sceneController;
    protected QuestController questController;

    private void Start() {
      inventoryController = FindObjectOfType<InventoryController>();
      craftingInventory = FindObjectOfType<CraftingInventory>();
      consumableInventory = FindObjectOfType<ConsumableInventory>();
      characterController = FindObjectOfType<BattleSystem.CharacterController>();
      sceneController = FindObjectOfType<SceneController>();
      questController = FindObjectOfType<QuestController>();
    }

    public virtual void Complete() {
      Debug.Log("Quest completed!");
      if (!this.completed) {
        completed = true;
        EventController.QuestSetToPending(this);
      }
    }

    public virtual void GrantReward() {
      characterController.GetActiveParty().ForEach(member => {
        member.experience += expReward;
        characterController.LevelUp(member);
      });

      if (itemRewards != null && itemRewards.Count > 0) {
          inventoryController.GiveItems(itemRewards);
        }
    }
  }
}
