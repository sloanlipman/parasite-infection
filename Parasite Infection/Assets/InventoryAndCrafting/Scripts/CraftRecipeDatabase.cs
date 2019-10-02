using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipeDatabase : MonoBehaviour {

  public List<CraftRecipe> recipes = new List<CraftRecipe>();
  private ItemDatabase itemDatabase;

  void Awake() {
    itemDatabase = GetComponent<ItemDatabase>();
    BuildCraftRecipeDatabase();
  }

  public Item CheckRecipe(HashSet<int> recipe) {
    foreach(CraftRecipe craftRecipe in recipes) {
      if (craftRecipe.requiredItems.SetEquals(recipe)) {
        return itemDatabase.GetItem(craftRecipe.itemToCraft);
      }
    }
    return null;
  }

  void BuildCraftRecipeDatabase() {
    recipes = new List<CraftRecipe>() {
      new CraftRecipe(1, new HashSet<int> {5, 9, 10}), // Heavy Suit
      new CraftRecipe(2, new HashSet<int> {6, 9, 10}), // Fire Module,
      new CraftRecipe(3, new HashSet<int> {7, 9, 10}), // Water Module
      new CraftRecipe(4, new HashSet<int> {8, 9, 10}) // Water Module
    };
  }
}
