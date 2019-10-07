using UnityEngine;
namespace QuestSystem { 
  public class Goal {
    public int countNeeded;
    public int countCurrent;
    public bool completed;
    public Quest quest;

    public Goal() {}

    public void Increment(int amount) {
      countCurrent = Mathf.Min(countCurrent + amount, countNeeded);
      if (countCurrent >= countNeeded && !completed) {
        this.completed = true;
        Debug.Log("Goal completed!");
        quest.Complete();
      }
      EventController.QuestProgressChanged(quest);
    }
  }
}