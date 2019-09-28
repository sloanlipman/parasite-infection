namespace QuestSystem { 
  public class CollectionGoal : Goal {
    public int itemID;

    public CollectionGoal(int amountNeeded, int itemID, Quest quest) {
      countCurrent = 0;
      countNeeded = amountNeeded;
      completed = false;
      this.itemID = itemID;
      this.quest = quest;
      EventController.OnItemCollected += ItemCollected;
    }

    void ItemCollected(int itemID) {
      if (this.itemID == itemID) {
        Increment(1);
        if (this.completed) {
          EventController.OnItemCollected -= ItemCollected;
        }
      }
    }
  }
}