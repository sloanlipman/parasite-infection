using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {
  public List<Item> items = new List<Item>();

  void Awake() {
    if (FindObjectsOfType<ItemDatabase>().Length > 1) {
      Destroy(this.gameObject);
    }

    DontDestroyOnLoad(this.gameObject);
    BuildItemDatabase();
  }

  public Item GetItem(int id) {
    return items.Find(item => item.id == id);
  }

  public Item GetItem(string itemName) {
    return items.Find(item => item.itemName == itemName);
  }

  public int GetItemId(Item item) {
    int index = -1;
    for (int i = 0; i < items.Count; i++) {
      if (item.itemName == items[i].itemName) {
        index = items[i].id;
        break;
      }
    }

    return index;
  }

  void BuildItemDatabase() {
    items = new List<Item>() {
      new Item(1, "Heavy Module", "An upgrade to your regular suit. Unlocks Barrage",
      new Dictionary<string, int> {
        { "Ability", 1 }
      }),
      new Item(2, "Fire Module", "A suit upgrade. Unlocks Fireball.",
      new Dictionary<string, int> {
        { "Ability", 2 }
      }),
      new Item(3, "Water Module", "A suit upgrade. Unlocks Hydroblast.",
      new Dictionary<string, int> {
        { "Ability", 3 }
      }),
      new Item(4, "Medic Module", "A suit upgrade. Unlocks Heal.",
      new Dictionary<string, int> {
        { "Ability", 4 }
      }),
      new Item(5, "Heavy Core", "Required component to craft the Heavy Suit.",
      new Dictionary<string, int> {
        { "CoreID", 1}
      }),
      new Item(6, "Fire Core", "Required component to craft the Fire Module.",
      new Dictionary<string, int> {
        { "CoreID", 2}
      }),
      new Item(7, "Water Core", "Required component to craft the Water Module.",
      new Dictionary<string, int> {
        { "CoreID", 3}
      }),
      new Item(8, "Medic Core", "Required component to craft the Medic Module.",
      new Dictionary<string, int> {
        { "CoreID", 4}
      }),
      new Item(9, "Battery", "Required component to craft any module", 
      new Dictionary<string, int> {
        { "Value", 100 }
      }),
      new Item(10, "Integrated Circuit", "Required component to craft any module", 
      new Dictionary<string, int> {
      { "Value", 100 }
      })
    };
  }
}
