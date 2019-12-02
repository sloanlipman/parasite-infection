using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemDatabase : MonoBehaviour {
  private List<Item> itemDatabaseList = new List<Item>();
  private HashSet<Item> currentItems = new HashSet<Item>();

  void Awake() {
    BuildItemDatabase();
  }

  public List<Item> GetItemDatabaseList() {
    return itemDatabaseList;
  }

  public Item GetItem(int id) {
    return itemDatabaseList.Find(item => item.id == id);
  }

  public Item GetItem(string itemName) {
    return itemDatabaseList.Find(item => item.itemName == itemName);
  }

  public int GetItemId(Item item) {
    int id = -1;
    for (int i = 0; i < itemDatabaseList.Count; i++) {
      if (item.itemName == itemDatabaseList[i].itemName) {
        id = itemDatabaseList[i].id;
        break;
      }
    }

    return id;
  }

  public void ClearCurrentItemList() {
    currentItems.Clear();
  }

  public HashSet<Item> GetCurrentItemList() {
    return currentItems;
  }

  void BuildItemDatabase() {
    itemDatabaseList = new List<Item>() {
      new Item(1, "Heavy Module", "An upgrade to your regular suit. Unlocks Barrage",
      new Dictionary<string, int> {
        {"Ability", 1 },
        {"Crafting", 1},
        {"Equippable", 1}
      }),
      new Item(2, "Fire Module", "A suit upgrade. Unlocks Fireball.",
      new Dictionary<string, int> {
        {"Ability", 2 },
        {"Crafting", 1},
        {"Equippable", 1}
      }),
      new Item(3, "Water Module", "A suit upgrade. Unlocks Hydroblast.",
      new Dictionary<string, int> {
        {"Ability", 3 },
        {"Crafting", 1},
        {"Equippable", 1}
      }),
      new Item(4, "Medic Module", "A suit upgrade. Unlocks Heal.",
      new Dictionary<string, int> {
        {"Ability", 4 },
        {"Crafting", 1},
        {"Equippable", 1}
      }),
      new Item(5, "Heavy Core", "Required component to craft the Heavy Suit.",
      new Dictionary<string, int> {
        {"CoreID", 1},
        {"Crafting", 1},
        {"Equippable", 0}
      }),
      new Item(6, "Fire Core", "Required component to craft the Fire Module.",
      new Dictionary<string, int> {
        {"CoreID", 2},
        {"Crafting", 1},
        {"Equippable", 0}
      }),
      new Item(7, "Water Core", "Required component to craft the Water Module.",
      new Dictionary<string, int> {
        {"CoreID", 3},
        {"Crafting", 1},
        {"Equippable", 0}
      }),
      new Item(8, "Medic Core", "Required component to craft the Medic Module.",
      new Dictionary<string, int> {
        {"CoreID", 4},
        {"Crafting", 1},
        {"Equippable", 0}
      }),
      new Item(9, "Battery", "Required component to craft any module", 
      new Dictionary<string, int> {
        {"Crafting", 1},
        {"Equippable", 0}
      }),
      new Item(10, "Integrated Circuit", "Required component to craft any module", 
      new Dictionary<string, int> {
      {"Crafting", 1},
      {"Equippable", 0}
      }),
      new Item(11, "Medkit", "Restores 10 HP", 
      new Dictionary<string, int> {
      {"Health", 10},
      {"Equippable", 0},
      {"Crafting", 0}
      }),
      new Item(12, "Energy Pack", "Restores 10 EP",
      new Dictionary<string, int> {
      {"Energy", 10},
      {"Equippable", 0},
      {"Crafting", 0}
      }),
      new Item(13, "Cure", "Needs a Delivery Device to be viable",
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 1}
      }),
      new Item(14, "Delivery Device", "Combine this with the Cure",
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 1}      
      }),
      new Item(15, "Alien DNA", "Kelly says we need 3 of these to make the cure",
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 1}      
      }),
      new Item(16, "Power Source", "This will power the Cure delivery device",
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 1}      
      }),
      new Item(17, "Injector", "This will be tough enough to pierce the Parasites' skin",
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 1}      
      }),
      new Item(18, "Stabilizer", "This will stabilize the Cure so it'll work",
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 1}      
      }),
      new Item(19, "Completed Cure", "This will cure the crew",
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 1}      
      }),
      new Item(20, "Cure-all", "Some leftover healing material from synthesizing the cure. Recover 20 HP and 20 EP"),
      new Dictionary<string, int> {
        {"Equippable", 0},
        {"Crafting", 0},
        {"Health", 20},
        {"Energy", 20}
      }
    };
  }
}
