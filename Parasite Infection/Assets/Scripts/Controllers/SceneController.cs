using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
  public static SceneController Instance {get; set;}
  [SerializeField] private DialogPanel dialogPanel;
  [SerializeField] private DialogData gameIntroDialog;
  [SerializeField] private QuestSystem.QuestController questController;
  [SerializeField] private BattleSystem.CharacterController characterController;
  [SerializeField] private UIDecisionPanel decisionPanel;
  private MenuController menuController;

  private int currentAct = 0;
  private bool playerRecruitedMegan;

  private bool hasPlayerDoneTutorial;

  public void Save() {
    ES3.Save<int>("CurrentAct", currentAct);
  }

  public void Load() {
    currentAct = ES3.Load<int>("CurrentAct");
  }

  private void Awake() {
    if (FindObjectsOfType<SceneController>().Length > 1) {
        Destroy(this.gameObject);
      }

    menuController = FindObjectOfType<MenuController>();

    DontDestroyOnLoad(this.gameObject);
    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.sceneUnloaded += OnSceneUnloaded;
  }

  public int GetCurrentAct() {
    return currentAct;
  }

  public void FreezeTime() {
    Time.timeScale = 0;
  }

  public void UnfreezeTime() {
    if (!menuController.IsGamePaused()) {
      Time.timeScale = 1;
    }
  }

  private void MakeAct1Decision(string choiceName) {
    playerRecruitedMegan = choiceName == "Megan";
    FinishEndOfAct1Dialog(choiceName);
  }

  public void OnSceneUnloaded(Scene scene) {
    Debug.Log("Unloaded scene:" + scene.name.ToString() );
    if (scene.name.ToString() == "Intro") {
      SaveService.Instance.Save();
    }
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    Debug.Log("Scene Controller OnSceneLoaded: " + scene.name);

    switch(scene.name) {
      case "Intro": {
        currentAct = 0;
        SaveService.Instance.StartNewGame();
        characterController.ResetAllCharacters();
        DialogPanel introPanel = GameObject.FindGameObjectWithTag("UI/Intro Panel").GetComponent<DialogPanel>();
        introPanel.StartDialog(gameIntroDialog.dialog);
        EventController.OnDialogPanelClosed += LoadCommandCenter;
        break;
      }

      case "Command Center": {
        if (!questController.HasQuestBeenStarted("KillBlobsQuest")) {
          string[] dialog = new string[] {
            "Check in with the Android to get your assignment.",
            "If you need help, press ESC to access the tutorial.",
            };
          dialogPanel.StartDialog(dialog);
        } else if (questController.IsQuestCompleted("KillBlobsQuest")) {
            ActivateGatewayToLeaveCommandCenter();
          }
        break;
      }

      case "Central Core": {
        Debug.Log("Case was central core");
        if (!questController.HasQuestBeenStarted("CraftWaterQuest")) {
          string[] dialog = new string[] {"???: Barry? I'm over here! Follow the green trail!"};
          dialogPanel.StartDialog(dialog);
        } else if (questController.IsQuestCompleted("CraftWaterQuest")) {
          OpenGateToTentacleMonster();
        }

        if (questController.IsQuestCompleted("DefeatTentacleMonsterQuest")) {
          RemoveTentacleMonster(); // Internally calls to open the gateway
        }
        break;
      }
    }
  }

  public void LoadSceneFromGateway(string sceneName) {
    SaveService.Instance.Save();
    switch (sceneName) {
      case "Command Center": {
        LoadCommandCenter();
        break;
      }

      case "Central Core": {
        Debug.Log("About to LoadCentralCore");
        LoadCentralCore();
        break;
      }
    }
    GatewayManager.Instance.MoveInNewScene();
  }

  private void LoadCommandCenter() {
    SceneManager.LoadScene("Command Center");
    EventController.OnDialogPanelClosed -= LoadCommandCenter;
  }

  private void LoadCentralCore() {
     if (!questController.IsQuestCompleted("DefeatTentacleMonsterQuest")) {
      SceneManager.LoadScene("Central Core");
      currentAct = 1;
      Debug.Log("Loaded central core from LoadCentralCore() method");
    } else {
      GatewayManager.Instance.SetSpawnPosition(new Vector2(-5, -40));
      GatewayManager.Instance.MoveInNewScene();
    }
  }

  private void ActivateGatewayToLeaveCommandCenter() {
    GameObject.FindGameObjectWithTag("Gateways/Command Center").GetComponent<Gateway>().isActive = true;
  }

  private void ActivateGatewayAfterTentacleMonster() {
    GameObject.FindGameObjectWithTag("Gateways/Central Core").GetComponent<Gateway>().isActive = true;
  }

  private void RemoveBossTrigger() {
    Destroy(GameObject.FindGameObjectWithTag("Boss Trigger"));
  }

  public void StartKillBlobsQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Android: Here's a Fire Module. Don't forget to equip it.",
      "Let's go help the others.",
      "Head down to the transporter.",
      "We should look for Alan."
    };
    dialogPanel.StartDialog(dialog);
  }

  public void StartCraftWaterQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Alan: Good stuff. All right, let's get outta here.",
      "The captain is up ahead.",
      "I'll open the security gate.",
      "I'm right behind you."
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += OpenGateToTentacleMonster;
  }

  public void StartDefeatTentacleMonsterQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Parasite: I...impossible!",
      "How could a human defeat my spawn?",
      "I won't forget this... Barry...",
      "Alan: Let's head back to the teleporter.",
      "We can head down to the Central Core",
      "and regroup with the crew"
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += RemoveTentacleMonster;
  }

  private void OpenGateToTentacleMonster() {
    GameObject barricade = GameObject.FindGameObjectWithTag("Barricade/Tentacle Monster");
    if (barricade != null) {
      Destroy(barricade);
    }
  }

  private void RemoveTentacleMonster() {
    GameObject tentacleMonsterGameObject = GameObject.FindGameObjectWithTag("Tentacle Monster");
    if (tentacleMonsterGameObject != null) {
      BattleSystem.BattleLaunchCharacter tentacleMonster = tentacleMonsterGameObject.GetComponent<BattleSystem.BattleLaunchCharacter>();
      if (tentacleMonster != null) {
        Destroy(tentacleMonster.gameObject);
      }
    }
    EventController.OnDialogPanelClosed -= RemoveTentacleMonster;
    ActivateGatewayAfterTentacleMonster();
  }

  public void StartEndOfAct1Dialog() {
    RemoveBossTrigger();
    string[] dialog = new string[] {
      "Android: Yo. Now I'm only reading one alien.",
      "Alan: One of them must have been mind controlled.",
      "Barry, what do we do?"
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += OpenDecisionPanelForAct1;
  }

  private void OpenDecisionPanelForAct1() {
    decisionPanel.gameObject.SetActive(true);
    decisionPanel.SetTitle("Who is infected?");
    decisionPanel.AddChoice("Megan");
    decisionPanel.AddChoice("Jake");
    EventController.OnDecisionMade += MakeAct1Decision;
    currentAct = 2;
    EventController.OnDialogPanelClosed -= OpenDecisionPanelForAct1;
    // Load a transition scene that has more story or whatever
  }

  private void FinishEndOfAct1Dialog(string choiceName) {
    string[] dialog = new string[] {
      "Android: Alien life detected in the Biosphere.",
      "I'll go on ahead.",
      string.Format("{0}, stick with these guys", choiceName),
      "Take this extra Heavy Module."
    };
    dialogPanel.StartDialog(dialog);

    /** On Dialog close:
      1) Remove Android
      2) Add choiceName
      3) Give player Heavy Module
      4) Adjust choiceName's exp to match Android's
      5) Give everyone exp as a reward
    */
  }
}
