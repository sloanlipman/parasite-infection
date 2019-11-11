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
  private MenuController menuController;

  private int currentAct = 0;
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
        SaveService.Instance.StartNewGame();
        characterController.ResetAllCharacters();
        DialogPanel introPanel = GameObject.FindGameObjectWithTag("UI/Intro Panel").GetComponent<DialogPanel>();
        introPanel.StartDialog(gameIntroDialog.dialog);
        EventController.OnDialogPanelClosed += LoadCommandCenter;
        break;
      }

      case "Command Center": {
        if (!questController.HasQuestBeenStarted("KillBlobsQuest")) {
          string[] dialog = new string[] {"Check in with the Android to get your assignment. Walk up to him and click."};
          dialogPanel.StartDialog(dialog);
        } else if (questController.IsQuestCompleted("KillBlobsQuest")) {
            // StartKillBlobsQuestCompletedDialog();
            ActivateGatewayToLeaveCommandCenter();
          }
        break;
      }

      case "Central Core": {
        if (!questController.HasQuestBeenStarted("CraftWaterQuest")) {
          string[] dialog = new string[] {"???: Barry? I'm over here! Follow the green trail!"};
          dialogPanel.StartDialog(dialog);
        } else if (questController.IsQuestCompleted("CraftWaterQuest")) {
          OpenGateToTentacleMonster();
          // Allow access to the next area
        }

        if (questController.IsQuestCompleted("DefeatTentacleMonsterQuest")) {
          ActivateGatewayAfterTentacleMonster();
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
      currentAct = 1;
      Debug.Log("Loaded central core from LoadCentralCore() method");
    } else {

    // TODO design terrain somewhere else in this scene and teleport there to fight some crew members
      // GatewayManager.Instance.SetSpawnPosition(new Vector2());
      // GatewayManager.Instance.MoveInNewScene();
      string[] dialog = new string[] {
        "Thanks for playing the Beta of Parasite: Infection!",
        "For the next release, there will be more content.",
        "There will also be more balance.",
        "I hope you enjoyed it!"
      };
      dialogPanel.StartDialog(dialog);

    }
  }

  private void ActivateGatewayToLeaveCommandCenter() {
    GameObject.FindGameObjectWithTag("Gateways/Command Center").GetComponent<Gateway>().isActive = true;
  }

  private void ActivateGatewayAfterTentacleMonster() {
    GameObject.FindGameObjectWithTag("Gateways/Central Core").GetComponent<Gateway>().isActive = true;
  }

  public void StartKillBlobsQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Android: Let's go help the others.",
      "Head up to the transporter, and we'll help Alan."
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
    BattleSystem.BattleLaunchCharacter tentacleMonster = FindObjectOfType<BattleSystem.BattleLaunchCharacter>();
    Destroy(tentacleMonster.gameObject);
    EventController.OnDialogPanelClosed -= RemoveTentacleMonster;
    ActivateGatewayAfterTentacleMonster();
  }
}
