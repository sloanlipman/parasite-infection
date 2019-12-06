using UnityEngine;
using BattleSystem;

public class NPC : Character {
 
  private Vector2[] movementDirections = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
  private Vector2 spawnPosition;
  [SerializeField] private bool wander;

  [SerializeField] private string questName;
  private QuestSystem.Quest quest;
  private QuestSystem.QuestController questController;
  private SceneController sceneController;

  [SerializeField] private DialogData dialogData;
  [SerializeField] private DialogData questCompletedDialogData;

  private Gateway gateway;

  private DialogPanel dialog;
  private DialogCanvas dialogCanvas;

  private Character player;

  public string npcName;

  public void SetDialogData(DialogData dialogData) {
    this.dialogData = dialogData;
  }

  public void SetQuestCompletedDialogData(DialogData questCompletedDialogData) {
    this.questCompletedDialogData = questCompletedDialogData;
  }

  private void Awake() {
    FindDialogPanel();
    sceneController = FindObjectOfType<SceneController>();
    gateway = GetComponent<Gateway>();
  }
  
  private void Start() {
    questController = FindObjectOfType<QuestSystem.QuestController>();
    spawnPosition = transform.position;
    if (wander) {
      Wander();    
    }
  }

  public void Interact(Character player = null) {
    this.player = player;
    if (
      GetComponent<BattleLaunchCharacter>() != null &&
      IsCharacterMalfunctiongAndroidOrIsTheAndroidDefeated()
    ) {
        EventController.OnDialogPanelClosed += StartBattle;
    }
    if (questName != "") { // If NPC gives a quest
      if (quest == null && !IsQuestAssigned() && !IsQuestCompleted()) {
        quest = questController.AssignQuest(questName);
      }
      if (quest == null && questCompletedDialogData != null) {
        dialogData = questCompletedDialogData;
      }
    }
    if (dialogData != null) {
      if (dialog == null) {
        FindDialogPanel();
      }
      if (IsCharacterMalfunctiongAndroidOrIsTheAndroidDefeated()) {
        if (gateway == null) {
          dialog.StartDialog(dialogData.dialog);
        } else if (gateway != null && !gateway.isActive) {
          dialog.StartDialog(dialogData.dialog);
        }
      }

      EventController.OnDialogPanelClosed += UnfreezeTime;
    }
  }

  private void UnfreezeTime() {
    EventController.OnDialogPanelClosed -= UnfreezeTime;
  }

  private void StartBattle() {
    if (this.player != null) {
     GetComponent<BattleLaunchCharacter>().PrepareBattle(this.player);
    }
    EventController.OnDialogPanelClosed -= StartBattle;
  }

  private void FindDialogPanel() {
    dialogCanvas = FindObjectOfType<DialogCanvas>();
    if (dialogCanvas != null) {
      dialog = dialogCanvas.GetComponentInChildren<DialogPanel>(true);
    }
  }

  public void Wander() {
    Vector2 currentPosition = transform.position;
    if (currentPosition == spawnPosition) {
      int roll = Random.Range(0,3);
      Vector2 destination = currentPosition + movementDirections[roll];
      StartCoroutine(MoveTo(destination, Wander, Random.Range(2,5)));
    } else {
      StartCoroutine(MoveTo(spawnPosition, Wander, Random.Range(2,5)));
    }
  }

  public bool IsQuestAssigned() {
    bool isQuestAssigned = false;
    quest = questController.assignedQuests.Find(quest => quest.slug == this.questName);
    if (quest != null) {
      isQuestAssigned = true;
    }

    return isQuestAssigned;
  }

  public bool IsQuestCompleted() {
    bool isQuestCompleted = false;
      if (questController.IsQuestCompleted(questName)) {
      isQuestCompleted = true;
    }
    return isQuestCompleted;
  }

  private bool IsCharacterMalfunctioningAndroid() {
    return npcName == "Malfunctioning Android";
  }

  private bool IsCharacterMalfunctiongAndroidOrIsTheAndroidDefeated() {
    return (
      !IsCharacterMalfunctioningAndroid() || (
        IsCharacterMalfunctioningAndroid() &&
        !sceneController.IsMalfunctioningAndroidDefeated()
      )
    );
  }
}
