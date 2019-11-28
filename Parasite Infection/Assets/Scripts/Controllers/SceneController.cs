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
  private bool hasPlayerDoneTutorial;

// Central Core flags and strings
  private bool shouldShowAlanInitialDialog = true;
  private string deadCrewMember;
  private string crewMemberWhoJoinedParty;
  private bool hasJakeOrMeganBeenRemoved = false;
  private bool hasPlayerMadeAct1Decision = false;

// Biosphere
  private bool hasPigAlienBeenRemoved = false;
  private bool isMalfunctioningAndroidDefeated = false;

// Shed
  private string playerRemovedFromPartyForOctopusFight;

  public void Save() {
    ES3.Save<int>("currentAct", currentAct, "SceneController.json");
    ES3.Save<bool>("hasPlayerDoneTutorial", hasPlayerDoneTutorial, "SceneController.json");
    ES3.Save<bool>("shouldShowAlanInitialDialog", shouldShowAlanInitialDialog, "SceneController.json");
    ES3.Save<string>("deadCrewMember", deadCrewMember, "SceneController.json");
    ES3.Save<string>("crewMemberWhoJoinedParty", crewMemberWhoJoinedParty, "SceneController.json");
    ES3.Save<bool>("hasJakeOrMeganBeenRemoved", hasJakeOrMeganBeenRemoved, "SceneController.json");
    ES3.Save<bool>("hasPlayerMadeAct1Decision", hasPlayerMadeAct1Decision, "SceneController.json");
    ES3.Save<bool>("hasPigAlienBeenRemoved", hasPigAlienBeenRemoved, "SceneController.json");
    ES3.Save<bool>("isMalfunctioningAndroidDefeated", isMalfunctioningAndroidDefeated, "SceneController.json");
    ES3.Save<string>("playerRemovedFromPartyForOctopusFight", playerRemovedFromPartyForOctopusFight, "SceneController.json");
  }

  public void Load() {
    currentAct = ES3.Load<int>("currentAct", "SceneController.json");
    hasPlayerDoneTutorial = ES3.Load<bool>("hasPlayerDoneTutorial", "SceneController.json");
    shouldShowAlanInitialDialog = ES3.Load<bool>("shouldShowAlanInitialDialog", "SceneController.json");
    deadCrewMember = ES3.Load<string>("deadCrewMember", "SceneController.json");
    crewMemberWhoJoinedParty = ES3.Load<string>("crewMemberWhoJoinedParty", "SceneController.json");
    hasJakeOrMeganBeenRemoved = ES3.Load<bool>("hasJakeOrMeganBeenRemoved", "SceneController.json");
    hasPlayerMadeAct1Decision = ES3.Load<bool>("hasPlayerMadeAct1Decision", "SceneController.json");
    hasPigAlienBeenRemoved = ES3.Load<bool>("hasPigAlienBeenRemoved", "SceneController.json");
    isMalfunctioningAndroidDefeated = ES3.Load<bool>("isMalfunctioningAndroidDefeated", "SceneController.json");
    playerRemovedFromPartyForOctopusFight = ES3.Load<string>("playerRemovedFromPartyForOctopusFight", "SceneController.json");
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
    deadCrewMember = choiceName;
    crewMemberWhoJoinedParty = choiceName == "Megan" ? "Jake" : "Megan";
    hasPlayerMadeAct1Decision = true;
    FinishEndOfAct1Dialog();
  }

  private void MakeAct2Decision(string choiceName) {
    playerRemovedFromPartyForOctopusFight = choiceName;
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
        if(questController.IsQuestCompleted("KillPigAlienQuest")) {
          RemovePigAlien();
        }
        break;
      }

      case "Shed": {
        if (questController.IsQuestCompleted("DefeatMalfunctioningAndroidQuest")) {
          ActivateOctopusMonster();
        }

        if (questController.IsQuestCompleted("SlayOctopusMonsterQuest")) {
          RemoveOctopusMonster();
        }
        break;
      }

      case "Labs": {
        currentAct = 3;
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

  private void UnlockShedEntrance() {
    GameObject.FindGameObjectWithTag("Gateways/Shed Entrance").GetComponent<Gateway>().isActive = true;
  }

  private void UnlockShedExit() {
    GameObject.FindGameObjectWithTag("Gateways/Shed Exit").GetComponent<Gateway>().isActive = true;
  }

  private void RemoveBossTrigger() {
    GameObject bossTrigger = GameObject.FindGameObjectWithTag("Boss Trigger");
    if (bossTrigger != null) {
      Destroy(bossTrigger);
    }
  }

  public void DefeatMalfunctioningAndroid() {
    isMalfunctioningAndroidDefeated = true;
  }

  public bool IsMalfunctioningAndroidDefeated() {
    return isMalfunctioningAndroidDefeated;
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
      "Tentacle Monster: IMPOSSIBLE!!!",
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
    // RemoveBossTrigger();
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
    string[] dialog = new string[] {
      string.Format("{0}: I've dismantled him."),
      "He had some data about a potential cure.",
      "Let's head down to the labs and check it out."
    };
    dialogPanel.StartDialog(dialog);
    RemoveOctopusMonster();
    currentAct = 3;
    characterController.AddPlayerToParty(playerRemovedFromPartyForOctopusFight);
    UnlockShedExit(); // TODO make this load act 3
  }

  private void ActivateOctopusMonster() {
    GameObject octopusMonsterParent = GameObject.FindGameObjectWithTag("Act 2 Boss");
    if (octopusMonsterParent != null) {
      NPC octopusMonster = octopusMonsterParent.GetComponentInChildren<NPC>(true);
      if (octopusMonster != null) {
        octopusMonster.gameObject.SetActive(true);
      }
    }
  }

  private void RemoveOctopusMonster() {
    GameObject ocotpusMonsterParent = GameObject.FindGameObjectWithTag("Act 2 Boss");
    if (ocotpusMonsterParent != null) {
      Destroy(ocotpusMonsterParent);
    }
  }

  private void OpenDecisionPanelForAct1() {
    decisionPanel.gameObject.SetActive(true);
    decisionPanel.SetTitle("Who is infected?");
    decisionPanel.AddChoice("Megan");
    decisionPanel.AddChoice("Jake");
    EventController.OnDecisionMade += MakeAct1Decision;
    currentAct = 2;
    EventController.OnDialogPanelClosed -= OpenDecisionPanelForAct1;
  }

  private void OpenDecisionPanelForAct2() {
    decisionPanel.gameObject.SetActive(true);
    decisionPanel.SetTitle("Who will stay back?");
    decisionPanel.AddChoice("Alan");
    decisionPanel.AddChoice(crewMemberWhoJoinedParty);
    EventController.OnDecisionMade += MakeAct2Decision;
    EventController.OnDialogPanelClosed -= OpenDecisionPanelForAct2;
  }

  private void FinishEndOfAct1Dialog() {
    string[] dialog = new string[] {
      string.Format("*You shoot {0}*", deadCrewMember),
      "Android: Alien life detected in the Biosphere.",
      "I'll go on ahead.",
      string.Format("{0}, stick with these guys", crewMemberWhoJoinedParty),
      "Take this extra Heavy Module."
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += RemoveJakeOrMegan;
  }

  private void RemoveJakeOrMegan() {
    if (!hasJakeOrMeganBeenRemoved && hasPlayerMadeAct1Decision) {
      NPC[] npcs = GameObject.FindObjectsOfType<NPC>();
      foreach(NPC n in npcs) {
        if (n.npcName == deadCrewMember) {
          Destroy(n.gameObject);
        }
      }
      characterController.AddPlayerToParty(crewMemberWhoJoinedParty);
      characterController.RemovePlayerFromParty("Android");
      int androidExperience = characterController.GetExperience("Android");
      characterController.SetExperience(androidExperience, crewMemberWhoJoinedParty);
      inventoryController.GiveItem("Heavy Module");
      hasJakeOrMeganBeenRemoved = true;
    }
    EventController.OnDialogPanelClosed -= RemoveJakeOrMegan;
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
    EventController.OnDialogPanelClosed -= RemovePigAlien;

    UnlockShedEntrance();
  }
}
