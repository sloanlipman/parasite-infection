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

    DontDestroyOnLoad(this.gameObject);
    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.sceneUnloaded += OnSceneUnloaded;
  }

  public int GetCurrentAct() {
    return currentAct;
  }

  public void OnSceneUnloaded(Scene scene) {
    if (scene.name.ToString() == "Intro") {
      Debug.Log("Unloaded intro");
      SaveService.Instance.Save();
    }
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
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

      case "Armory": {
        if (!questController.HasQuestBeenStarted("CraftWaterQuest")) {
          string[] dialog = new string[] {"???: Barry? I'm over here! Follow the green trail!"};
          dialogPanel.StartDialog(dialog);
        } else if (questController.IsQuestCompleted("CraftWaterQuest")) {
          // Allow access to the next area
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
    SceneManager.LoadScene("Central Core");
    currentAct = 1;
  }

  private void ActivateGatewayToLeaveCommandCenter() {
    GameObject.FindGameObjectWithTag("Gateways/Command Center").GetComponent<Gateway>().isActive = true;
  }

  public void StartKillBlobsQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Android: Let's go help the others.",
      "Android: Head up to the transporter, and we'll help Alan."
    };
    dialogPanel.StartDialog(dialog);
  }

  public void StartCraftWaterQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Alan: Good stuff. All right, let's get outta here.",
      "Alan: The captain is up ahead.",
      "Alan: I'm right behind you."
    };
    dialogPanel.StartDialog(dialog);
  }
}
