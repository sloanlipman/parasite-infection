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
  [SerializeField] private InventoryController inventoryController;
  [SerializeField] private BattleSystem.BattleLauncher battleLauncher;
  private MenuController menuController;

  private int currentAct = 0;
  private bool shouldShowAlanInitialDialog = true;
  private bool playerRecruitedMegan;
  private bool hasJakeOrMeganBeenRemoved = false;
  private bool hasPigAlienBeenRemoved = false;
  private string playerRemovedFromParty;

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

  private void MakeAct2Decision(string choiceName) {
    playerRemovedFromParty = choiceName;
    characterController.RemovePlayerFromParty(choiceName);
    ActivateOctopusMonster();
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
        currentAct = 1;
        if (!questController.HasQuestBeenStarted("CraftWaterQuest")) {
          if (shouldShowAlanInitialDialog) {
            string[] dialog = new string[] {"Alan: Barry? I'm over here! Follow the green trail!"};
            dialogPanel.StartDialog(dialog);
            shouldShowAlanInitialDialog = false;
          }
        } else if (questController.IsQuestCompleted("CraftWaterQuest")) {
          OpenGateToTentacleMonster();
        }

        if (questController.IsQuestCompleted("DefeatTentacleMonsterQuest")) {
          RemoveTentacleMonster(); // Internally calls to open the gateway
        }

        if (questController.IsQuestCompleted("Act1FinalBossQuest")) {
          RemoveBossTrigger();
          RemoveJakeOrMegan(); // Internally opens gateway to biosphere
        }
        break;
      }

      case "Biosphere": {
        currentAct = 2;
        // TODO add more stuff here
        if(questController.IsQuestCompleted("KillPigAlienQuest")) {
          RemovePigAlien();
        }
        break;
      }

      case "Shed": {
        if (questController.IsQuestCompleted("DefeatMalfunctioningAndroidQuest")) {
          ActivateOctopusMonster();
        }

        if (questController.IsQuestCompleted("SlayOctopusMonster")) {
          RemoveOctopusMonster();
        }
        break;
      }

      default: {
        break;
      }
    }
  }

  public void LoadSceneFromGateway(string sceneName) {
    if (SceneManager.GetSceneByName(sceneName) != SceneManager.GetActiveScene()) {
      SceneManager.LoadScene(sceneName);
    }
    GatewayManager.Instance.MoveInNewScene();
  }

  private void LoadCommandCenter() {
    SceneManager.LoadScene("Command Center");
    EventController.OnDialogPanelClosed -= LoadCommandCenter;
  }

  private void ActivateGatewayToLeaveCommandCenter() {
    GameObject.FindGameObjectWithTag("Gateways/Command Center").GetComponent<Gateway>().isActive = true;
  }

  private void ActivateGatewayAfterTentacleMonster() {
    GameObject.FindGameObjectWithTag("Gateways/Central Core").GetComponent<Gateway>().isActive = true;
  }

  private void ActivateGatewayAtEndOfAct1() {
    GameObject.FindGameObjectWithTag("Gateways/Biosphere").GetComponent<Gateway>().isActive = true;
  }

  private void UnlockShed() {
    GameObject.FindGameObjectWithTag("Gateways/Shed").GetComponent<Gateway>().isActive = true;
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
      "Pig Farmer: Thanks for the help!",
      "How could a human defeat my spawn?",
      "I won't forget this... Barry...",
      "Alan: Let's head back to the teleporter.",
      "We can head down to the Central Core",
      "and regroup with the crew"
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += RemoveTentacleMonster;
  }

  public void StartKillPigAlienQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Pig Farmer: Thanks for the help!",
      "I think I saw the Android go into the shed.",
      "I'll unlock for door for ya."
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += RemovePigAlien;
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

  public void StartDefeatMalfunctioningAndroidQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Alan: Bar, there's a nasty parasite incoming,",
      "but one of us needs to dismantle the Android.",
      "Who's it going to be?"
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += OpenDecisionPanelForAct2;
  }

  public void StartDefeatOctopusMonsterQuestCompletedDialog() {
    // TODO FILL THIS IN
    currentAct = 3;
    UnlockShed(); // TODO make this load act 3
  }

  private void ActivateOctopusMonster() {
    GameObject.FindGameObjectWithTag("Act 2 Boss").GetComponentInChildren<GameObject>(true).SetActive(true);
  }

  private void RemoveOctopusMonster() {
    GameObject ocotpusMonsterParent = GameObject.FindGameObjectWithTag("Act 2 Boss");
    if (ocotpusMonsterParent != null) {
      Destroy(ocotpusMonsterParent);
    }
  }

  private void OpenDecisionPanelForAct1() {
    decisionPanel.gameObject.SetActive(true);
    decisionPanel.SetTitle("Whom should we save?");
    decisionPanel.AddChoice("Megan");
    decisionPanel.AddChoice("Jake");
    EventController.OnDecisionMade += MakeAct1Decision;
    currentAct = 2;
    EventController.OnDialogPanelClosed -= OpenDecisionPanelForAct1;
    // Load a transition scene that has more story or whatever
  }

  private void OpenDecisionPanelForAct2() {
    decisionPanel.gameObject.SetActive(true);
    decisionPanel.SetTitle("Who will stay back?");
    decisionPanel.AddChoice("Alan");
    decisionPanel.AddChoice(playerRecruitedMegan ? "Megan" : "Jake");
    EventController.OnDecisionMade += MakeAct2Decision;
    EventController.OnDialogPanelClosed -= OpenDecisionPanelForAct2;

  }

  public void SplitPartyForAndroidBattle(Vector3 position, List<BattleSystem.Enemy> enemies, bool splitParty) {

    battleLauncher.PrepareBattle(position, enemies, splitParty);
  }

  private void FinishEndOfAct1Dialog(string choiceName) {
    string target = choiceName == "Megan" ? "Jake" : "Megan";
    string[] dialog = new string[] {
      string.Format("*You shoot {0}*", target),
      "Android: Alien life detected in the Biosphere.",
      "I'll go on ahead.",
      string.Format("{0}, stick with these guys", choiceName),
      "Take this extra Heavy Module."
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += RemoveJakeOrMegan;
  }

  private void RemoveJakeOrMegan() {
    if (!hasJakeOrMeganBeenRemoved) {
      NPC[] npcs = GameObject.FindObjectsOfType<NPC>();
      string nameToFind = playerRecruitedMegan ? "Jake" : "Megan";
      foreach(NPC n in npcs) {
        if (n.npcName == nameToFind) {
          Destroy(n.gameObject);
        }
      }
      string playerToAdd = playerRecruitedMegan ? "Megan" : "Jake";
      characterController.AddPlayerToParty(playerToAdd);
      characterController.RemovePlayerFromParty("Android");
      int androidExperience = characterController.GetExperience("Android");
      characterController.SetExperience(androidExperience, playerToAdd);
      inventoryController.GiveItem("Heavy Module");
      hasJakeOrMeganBeenRemoved = true;
    }
    ActivateGatewayAtEndOfAct1();
  }

  private void RemovePigAlien() {
    if (!hasPigAlienBeenRemoved) {
      NPC[] npcs = GameObject.FindObjectsOfType<NPC>();
      foreach(NPC n in npcs) {
        if (n.npcName == "Pig Alien") {
          Destroy(n.gameObject);
        }
      }
      hasPigAlienBeenRemoved = true;
    }

    UnlockShed();
  }
}
