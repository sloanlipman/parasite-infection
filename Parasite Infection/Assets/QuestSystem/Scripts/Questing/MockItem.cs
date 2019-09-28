using UnityEngine;

namespace QuestSystem { 
  public class MockItem : MonoBehaviour {
    private int itemID;
    void Start() {
      itemID = 0;
    }

    public void Collect() {
      EventController.ItemCollected(itemID);
    }
  }
}