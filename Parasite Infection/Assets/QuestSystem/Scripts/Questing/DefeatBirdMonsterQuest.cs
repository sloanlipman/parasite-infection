using System.Collections.Generic;
using QuestSystem;

public class DefeatBirdMonsterQuest : Quest {

  void Awake() {
    slug = "DefeatBirdMonsterQuest";    
    questName = "Extract Bird Monster DNA";
    itemRewards = new List<string>() { "Alien DNA" };
    goal = new KillGoal(1, 40, this);
    sceneController = FindObjectOfType<SceneController>();
  }

  void Start() {
    sceneController.ActivateBirdMonster();
  }

  public override void Complete() {
    base.Complete();
  }

  public override void GrantReward() {
    sceneController.RemoveBirdMonster();
    base.GrantReward();
  }
}
