using System.Collections.Generic;
using QuestSystem;

public class KillPigAlienQuest : Quest {

  void Awake() {
    slug = "KillPigAlienQuest";    
    questName = "Kill Pig Alien";
    itemRewards = new List<string>() { "Medkit", "Energy Pack" };
    goal = new KillGoal(3, 37, this);
    expReward = 100;
    sceneController = FindObjectOfType<SceneController>();
  }

  void Start() {
    sceneController.ActivatePigAlien();
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.StartKillPigAlienQuestCompletedDialog();
    base.GrantReward();
  }
}
