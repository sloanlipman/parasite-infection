using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe {
  public HashSet<int> requiredItems;
  public int itemToCraft;

  public CraftRecipe(int itemToCraft, HashSet<int> requiredItems) {
    this.requiredItems = requiredItems;
    this.itemToCraft = itemToCraft;
  }
}
