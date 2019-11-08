using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

  [SerializeField] private DialogPanel dialogPanel;
  [SerializeField] private DialogData gameIntroDialog;
  [SerializeField] private QuestSystem.QuestController questController;
  [SerializeField] private BattleSystem.CharacterController characterController;
  private int currentAct = 1;
  private bool hasPlayerDoneTutorial;

  private void Awake() {
    if (FindObjectsOfType<SceneController>().Length > 1) {
        Destroy(this.gameObject);
      }

    DontDestroyOnLoad(this.gameObject);
    SceneManager.sceneLoaded += OnSceneLoaded;

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
        }
        break;
      }
    }
  }

  void LoadCommandCenter() {
    SceneManager.LoadScene("Command Center");
    EventController.OnDialogPanelClosed -= LoadCommandCenter;
  }
}
