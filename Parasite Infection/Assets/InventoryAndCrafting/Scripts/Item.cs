﻿using System.Collections.Generic;
using UnityEngine;

public class Item {
  public int id;
  public int index;
  public string itemName;
  public string description;
  public Sprite icon;
  public Dictionary<string, int> stats = new Dictionary<string, int>();

  public Item() {}

  public Item(int id, string itemName, string description, Dictionary<string, int> stats) {
    this.id = id;
    this.itemName = itemName;
    this.description = description;
    this.stats = stats;
    LoadSprite();
  }

  public void LoadSprite() {
    this.icon = Resources.Load<Sprite>("Items/" + itemName);
  }

  public Item(Item item) {
    this.id = item.id;
    this.itemName = item.itemName;
    this.description = item.description;
    this.icon = item.icon;
    this.stats = item.stats;
  }
}
