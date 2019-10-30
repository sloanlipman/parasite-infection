using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlots : MonoBehaviour {
  public CraftRecipeDatabase recipeDatabase;
  private List<UIItem> uIItems = new List<UIItem>();
  public UIItem craftResultSlot;
  private CraftingInventory craftingInventory;
  private ItemDatabase itemDatabase;

  private void Start() {
    craftingInventory = FindObjectOfType<CraftingInventory>();
    itemDatabase = FindObjectOfType<ItemDatabase>();
    uIItems = GetComponent<SlotPanel>().uiItems;
    uIItems.ForEach(item => item.isCraftingSlot = true);
  }

  public void ClearCraftingSlots() {
    int i = 0;
    uIItems.ForEach(item => {
      craftingInventory.RemoveItem(item.item);
      item = null;
      i++;
    });
    craftingInventory.UpdateIndices();
  }

  public void UpdateRecipe() {
    int[] itemTable = new int[uIItems.Count];
    for (int i = 0; i < uIItems.Count; i++) {
      if (uIItems[i].item != null) {
       itemTable[i] = uIItems[i].item.id;
      }
    }
    HashSet<int> craftTable = new HashSet<int>(itemTable);
    Item itemToCraft = recipeDatabase.CheckRecipe(craftTable);
    UpdateCraftingResultSlot(itemToCraft);
  }

  void UpdateCraftingResultSlot(Item itemToCraft) {
    craftResultSlot.UpdateItem(itemToCraft);
  }
}
