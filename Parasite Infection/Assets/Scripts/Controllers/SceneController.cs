using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BattleSystem;

public class SceneController : MonoBehaviour {
  public static SceneController Instance {get; set;}
  [SerializeField] private DialogPanel dialogPanel;
  [SerializeField] private DialogData gameIntroDialog;
  [SerializeField] private QuestSystem.QuestController questController;
  [SerializeField] private BattleSystem.CharacterController characterController;
  [SerializeField] private UIDecisionPanel decisionPanel;
  [SerializeField] private InventoryController inventoryController;
  [SerializeField] private BattleLauncher battleLauncher;
  private MenuController menuController;
  private List<Enemy> finalBattleEnemyParty = new List<Enemy>();

  private int currentAct = 0;
  private bool hasPlayerDoneTutorial;
  private int finalBattleScenario = 0;

  public int GetFinalBattleScenario() {
    return finalBattleScenario;
  }

// Central Core flags and strings
  private bool shouldAlanCallOutToBarry = true;
  private string deadCrewMember;
  private string crewMemberWhoJoinedParty;
  private bool hasJakeOrMeganBeenRemoved = false;
  private bool hasPlayerMadeAct1Decision = false;

// Biosphere
  private bool isMalfunctioningAndroidDefeated = false;

// Shed
  private string characterRemovedFromPartyForOctopusFight;

// Labs
  private string characterKilledDuringInterlude = "";
  private bool shouldTellToGoToBridge = true;

// Final boss
  private bool finalBossShouldBeAlien = true;
  private bool hasBossBeenRevealed = false;

  private bool IsQuestCompleted(string questName) {
    return questController.IsQuestCompleted(questName);
  }

  private bool HasQuestBeenStarted(string questName) {
    return questController.HasQuestBeenStarted(questName);
  }

  private bool IsQuestInProgress (string questName) {
    return HasQuestBeenStarted(questName) && !IsQuestCompleted(questName);
  }

  public void Save() {
    ES3.Save<int>("currentAct", currentAct, "SceneController.json");
    ES3.Save<bool>("hasPlayerDoneTutorial", hasPlayerDoneTutorial, "SceneController.json");
    ES3.Save<bool>("shouldAlanCallOutToBarry", shouldAlanCallOutToBarry, "SceneController.json");
    ES3.Save<string>("deadCrewMember", deadCrewMember, "SceneController.json");
    ES3.Save<string>("crewMemberWhoJoinedParty", crewMemberWhoJoinedParty, "SceneController.json");
    ES3.Save<bool>("hasJakeOrMeganBeenRemoved", hasJakeOrMeganBeenRemoved, "SceneController.json");
    ES3.Save<bool>("hasPlayerMadeAct1Decision", hasPlayerMadeAct1Decision, "SceneController.json");
    ES3.Save<bool>("isMalfunctioningAndroidDefeated", isMalfunctioningAndroidDefeated, "SceneController.json");
    ES3.Save<string>("characterRemovedFromPartyForOctopusFight", characterRemovedFromPartyForOctopusFight, "SceneController.json");
    ES3.Save<string>("characterKilledDuringInterlude", characterKilledDuringInterlude, "SceneController.json");
    ES3.Save<bool>("shouldTellToGoToBridge", shouldTellToGoToBridge, "SceneController.json");
    ES3.Save<bool>("hasBossBeenRevealed", hasBossBeenRevealed, "SceneController.json");
  }

  public void Load() {
    currentAct = ES3.Load<int>("currentAct", "SceneController.json", 1);
    hasPlayerDoneTutorial = ES3.Load<bool>("hasPlayerDoneTutorial", "SceneController.json", false);
    shouldAlanCallOutToBarry = ES3.Load<bool>("shouldAlanCallOutToBarry", "SceneController.json", true);
    deadCrewMember = ES3.Load<string>("deadCrewMember", "SceneController.json", "");
    crewMemberWhoJoinedParty = ES3.Load<string>("crewMemberWhoJoinedParty", "SceneController.json", "");
    hasJakeOrMeganBeenRemoved = ES3.Load<bool>("hasJakeOrMeganBeenRemoved", "SceneController.json", false);
    hasPlayerMadeAct1Decision = ES3.Load<bool>("hasPlayerMadeAct1Decision", "SceneController.json", false);
    isMalfunctioningAndroidDefeated = ES3.Load<bool>("isMalfunctioningAndroidDefeated", "SceneController.json", false);
    characterRemovedFromPartyForOctopusFight = ES3.Load<string>("characterRemovedFromPartyForOctopusFight", "SceneController.json", "");
    characterKilledDuringInterlude = ES3.Load<string>("characterKilledDuringInterlude", "SceneController.json", "");
    shouldTellToGoToBridge = ES3.Load<bool>("shouldTellToGoToBridge", "SceneController.json", true);
    hasBossBeenRevealed = ES3.Load<bool>("hasBossBeenRevealed", "SceneController.json", false);
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

  private string GetSubjectivePronounForDeadCrewMember() {
    return deadCrewMember == "Jake" ? "he" : "she";
  }

  private string GetObjectivePronounForDeadCrewMember() {
    return deadCrewMember == "Jake" ? "him" : "her";
  }

  private string GetObjectivePronounForAliveCrewMember() {
    return deadCrewMember == "Jake" ? "her" : "him";
  }

  private string GetPossessivePronounForAliveCrewMember() {
    return deadCrewMember == "Jake" ? "her" : "his";
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
    decisionPanel.ClearPanel();
    FinishEndOfAct1Dialog();
  }

  private void MakeAct2Decision(string choiceName) {
    characterRemovedFromPartyForOctopusFight = choiceName;
    characterController.RemovePlayerFromParty(choiceName);
    decisionPanel.ClearPanel();

    ActivateOctopusMonster();
  }

  private void MakeAct3Decision(string choiceName) {
    characterKilledDuringInterlude = choiceName;
    characterController.RemovePlayerFromParty(choiceName);
    decisionPanel.ClearPanel();

    FinishEndOfAct3Dialog();
  }

  public void OnSceneUnloaded(Scene scene) {
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
        if (!HasQuestBeenStarted("KillBlobsQuest")) {
          string[] dialog = new string[] {
            "Check in with the Android to get your assignment.",
            "If you need help, press ESC to access the tutorial.",
            };
          dialogPanel.StartDialog(dialog);
        } else if (IsQuestCompleted("KillBlobsQuest")) {
            ActivateGatewayToLeaveCommandCenter();
          }
        break;
      }

// Note: Don't refactor the Central Core case. It isn't worth the effort and functions as it is meant to.
      case "Central Core": {
        currentAct = 1;
        if (shouldAlanCallOutToBarry) {
            string[] dialog = new string[] {"Alan: Barry? I'm over here! Follow the green trail!"};
            dialogPanel.StartDialog(dialog);
            shouldAlanCallOutToBarry = false;
          }
          OpenGateToTentacleMonster();
          RemoveTentacleMonster(); // Internally calls to open the gateway
        if (IsQuestCompleted("Act1FinalBossQuest")) {
          RemoveBossTrigger();
          RemoveJakeOrMegan(); // Internally opens gateway to biosphere
        }
      break;

      }

      case "Biosphere": {
        currentAct = 2;
        ActivatePigAlien();
        RemovePigAlien();
        break;
      }

      case "Shed": {
        currentAct = 2;
        ActivateOctopusMonster();
        RemoveOctopusMonster();
        break;
      }

      case "Labs": {
        currentAct = 3;
        StartLabsDialog();
        SetInfectedAndroidParty();

        ActivateBirdMonster();
        ActivateDinosaurMonster();
        ActivateEvolvedBlob();

        RemoveBirdMonster();
        RemoveDinosaurMonster();
        RemoveEvolvedBlob();

        RemoveInfectedAndroid();

        ActivateEnhancedParasite();
        RemoveEnhancedParasite();
        ActivateGatewayToLowerLabs();
        StartDefeatInfectedAndroidQuestCompletedDialog();
        StartDefeatEnhancedParasiteQuestCompletedDialog();
        CompleteInterlude();
        break;
      }

      case "Bridge": {
        currentAct = 4;
        ActivateFinalBoss();
        RemoveFinalBossTrigger();
        RemoveFinalBoss();
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
    if (IsQuestCompleted("KillPigAlienQuest")) {
      OpenGateway("Gateways/Shed Entrance");
    }
  }

  private void UnlockShedExit() {
    if(IsQuestCompleted("SlayOctopusMonsterQuest")) {
      OpenGateway("Gateways/Shed Exit");
    }
  }

  private void ActivateGatewayToLowerLabs() {
    if (IsQuestCompleted("CompleteTheCureQuest")) {
      OpenGateway("Gateways/Lower Labs");
    }
  }

  private void ActivateGatewayToBridge() {
    if (IsQuestCompleted("InterludeQuest")) {
      OpenGateway("Gateways/Bridge");
    }
  }

  private void OpenGateway(string tag) {
    GameObject.FindGameObjectWithTag(tag).GetComponent<Gateway>().isActive = true;
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
    if (IsQuestCompleted("CraftWaterQuest")) {
    GameObject barricade = GameObject.FindGameObjectWithTag("Barricade/Tentacle Monster");
    if (barricade != null) {
      Destroy(barricade);
    }
    }
  }

  private void RemoveTentacleMonster() {
    if (IsQuestCompleted("DefeatTentacleMonsterQuest")) {
      GameObject tentacleMonsterGameObject = GameObject.FindGameObjectWithTag("Tentacle Monster");
      if (tentacleMonsterGameObject != null) {
        BattleLaunchCharacter tentacleMonster = tentacleMonsterGameObject.GetComponent<BattleLaunchCharacter>();
        if (tentacleMonster != null) {
          Destroy(tentacleMonster.gameObject);
        }
      }
      EventController.OnDialogPanelClosed -= RemoveTentacleMonster;
      ActivateGatewayAfterTentacleMonster();
    }
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

    public void ActivatePigAlien() {
    if (IsQuestInProgress("KillPigAlienQuest")) {
      GameObject pigAlienParent = GameObject.FindGameObjectWithTag("Pig Alien");
      if (pigAlienParent != null) {
        NPC pigAlien = pigAlienParent.GetComponentInChildren<NPC>(true);
        if (pigAlien != null) {
          pigAlien.gameObject.SetActive(true);
        }
      }
    }
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
      string.Format("{0}: I've dismantled him.", characterRemovedFromPartyForOctopusFight),
      "He had some data about a potential cure.",
      "Let's head down to the labs and check it out."
    };
    dialogPanel.StartDialog(dialog);
    RemoveOctopusMonster();
    currentAct = 3;
    characterController.AddPlayerToParty(characterRemovedFromPartyForOctopusFight);
  }

  private void ActivateOctopusMonster() {
    if (IsQuestCompleted("DefeatMalfunctioningAndroidQuest")) {
      GameObject octopusMonsterParent = GameObject.FindGameObjectWithTag("Act 2 Boss");
      if (octopusMonsterParent != null) {
        NPC octopusMonster = octopusMonsterParent.GetComponentInChildren<NPC>(true);
        if (octopusMonster != null) {
          octopusMonster.gameObject.SetActive(true);
        }
      }
    }
  }

  private void RemoveOctopusMonster() {
    if (IsQuestCompleted("SlayOctopusMonsterQuest")) {
      GameObject ocotpusMonsterParent = GameObject.FindGameObjectWithTag("Act 2 Boss");
      if (ocotpusMonsterParent != null) {
        Destroy(ocotpusMonsterParent);
      }
      UnlockShedExit();
    }
  }

  private void RemoveInfectedAndroid() {
    if (IsQuestCompleted("DefeatInfectedAndroidQuest")) {
      GameObject infectedAndroidGameObject = GameObject.FindGameObjectWithTag("Infected Android");
      if (infectedAndroidGameObject != null) {
        Destroy(infectedAndroidGameObject);
      }
    }
    EventController.OnDialogPanelClosed -= RemoveInfectedAndroid;
  }

  private void ActivateEnhancedParasite() {
    if (IsQuestCompleted("DefeatInfectedAndroidQuest")) {
      GameObject enhancedParasiteParent = GameObject.FindGameObjectWithTag("Act 3 Boss");
      if (enhancedParasiteParent != null) {
        NPC enhancedParasite = enhancedParasiteParent.GetComponentInChildren<NPC>(true);
        if (enhancedParasite != null) {
          enhancedParasite.gameObject.SetActive(true);
        }
      }
    }
    EventController.OnDialogPanelClosed -= ActivateEnhancedParasite;

  }

  private void RemoveEnhancedParasite() {
    if (IsQuestCompleted("DefeatEnhancedParasiteQuest")) {
      GameObject enhancedParasite = GameObject.FindGameObjectWithTag("Act 3 Boss");
      if (enhancedParasite != null) {
        Destroy(enhancedParasite);
      }
    }
    EventController.OnDialogPanelClosed -= RemoveEnhancedParasite;
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

  private void OpenDecisionPanelForAct3() {
    decisionPanel.gameObject.SetActive(true);
    decisionPanel.SetTitle("Who is the REAL alien?");
    decisionPanel.AddChoice("Alan");
    decisionPanel.AddChoice(crewMemberWhoJoinedParty);
    EventController.OnDecisionMade += MakeAct3Decision;
    EventController.OnDialogPanelClosed -= OpenDecisionPanelForAct3;
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

  private void StartLabsDialog() {
    if (!HasQuestBeenStarted("CompleteTheCureQuest")) {
      string[] dialog = new string[] {
        "A voice echoes in your head.",
        "Kelly. The scientist is named Kelly.",
        "We must get to Kelly.",
        "Before it is too late."
      };
      dialogPanel.StartDialog(dialog);
    }
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
    if (IsQuestCompleted("KillPigAlienQuest")) {
      GameObject pigAlienParent = GameObject.FindGameObjectWithTag("Pig Alien");
      if (pigAlienParent != null) {
          Destroy(pigAlienParent);
        }
      EventController.OnDialogPanelClosed -= RemovePigAlien;
      UnlockShedEntrance();
    }
  }

  public void ActivateEvolvedBlob() {
    if (IsQuestInProgress("DefeatEvolvedBlobQuest")) {
      GameObject evolvedBlobParent = GameObject.FindGameObjectWithTag("Evolved Blob");
      if (evolvedBlobParent != null) {
        NPC evolvedBlob = evolvedBlobParent.GetComponentInChildren<NPC>(true);
        if (evolvedBlob != null) {
          evolvedBlob.gameObject.SetActive(true);
        }
      }
    }
  }
   public void ActivateDinosaurMonster() {
    if (IsQuestInProgress("DefeatDinosaurMonsterQuest")) {
      GameObject dinosaurMonsterParent = GameObject.FindGameObjectWithTag("Dinosaur Monster");
      if (dinosaurMonsterParent != null) {
        NPC dinosaurMonster = dinosaurMonsterParent.GetComponentInChildren<NPC>(true);
        if (dinosaurMonster != null) {
          dinosaurMonster.gameObject.SetActive(true);
        }
      }
    }
  }
   public void ActivateBirdMonster() {
    if (IsQuestInProgress("DefeatBirdMonsterQuest")) {
      GameObject birdMonsterParent = GameObject.FindGameObjectWithTag("Bird Monster");
      if (birdMonsterParent != null) {
        NPC birdMonster = birdMonsterParent.GetComponentInChildren<NPC>(true);
        if (birdMonster != null) {
          birdMonster.gameObject.SetActive(true);
        }
      }
    }
  }


  public void RemoveEvolvedBlob() {
    if (IsQuestCompleted("DefeatEvolvedBlobQuest")) {
      GameObject evolvedBlobParent = GameObject.FindGameObjectWithTag("Evolved Blob");
      if (evolvedBlobParent != null) {
        Destroy(evolvedBlobParent);
      }
    }
  }

  public void RemoveDinosaurMonster() {
    if (IsQuestCompleted("DefeatDinosaurMonsterQuest")) {
      GameObject dinosaurMonsterParent = GameObject.FindGameObjectWithTag("Dinosaur Monster");
      if (dinosaurMonsterParent != null) {
        Destroy(dinosaurMonsterParent);
      }
    }
  }

  public void RemoveBirdMonster() {
    if (IsQuestCompleted("DefeatBirdMonsterQuest")) {
      GameObject birdMonsterParent = GameObject.FindGameObjectWithTag("Bird Monster");
      if (birdMonsterParent != null) {
        Destroy(birdMonsterParent);
      }
    }
  }

  public void StartCompleteTheCureQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Kelly: This should cure everyone.",
      "But I'm picking up something weird.",
      "Can you go secure the lower labs?",
      "Take the Teleporter you came in through."
    };
    dialogPanel.StartDialog(dialog);
    ActivateGatewayToLowerLabs();
  }

  public void StartDefeatInfectedAndroidQuestCompletedDialog() {
    if (IsQuestCompleted("DefeatInfectedAndroidQuest") && !HasQuestBeenStarted("DefeatEnhancedParasiteQuest")) {
      string[] dialog = new string[] {
        string.Format("Alan: Wait a second! Is that {0}!?", deadCrewMember),
        "Barry! Use the cure!",
        string.Format("We can bring {0} back to our side!", deadCrewMember),
        string.Format("You inject {0} with the cure", deadCrewMember),
        string.Format("but {0} begins to glow.", GetSubjectivePronounForDeadCrewMember()),
        string.Format("In a matter of seconds, {0} is not recognizable.", deadCrewMember),
        "The cure appears to have failed.",
        "You have no choice but to fight",
        "the ally you already lost once before"
      };

      dialogPanel.StartDialog(dialog);
      EventController.OnDialogPanelClosed += RemoveInfectedAndroid;
      EventController.OnDialogPanelClosed += ActivateEnhancedParasite;
    }
  }

  public void StartDefeatEnhancedParasiteQuestCompletedDialog() {
    if (
        IsQuestCompleted("DefeatEnhancedParasiteQuest")&&
        !IsQuestCompleted("InterludeQuest")) {
      Debug.Log("Should start Dialog here in a second");
      RemoveEnhancedParasite();
      string[] dialog = new string[] {
        "You realize something.",
        string.Format("{0} wasn't even infected in your first battle.", deadCrewMember),
        "You see a shadowy image in a memory.",
        "Someone doubled back to the Central Core.",
        string.Format("After you killed {0}, someone went back.", deadCrewMember),
        string.Format("Someone infected {0} AFTER you killed {1}.", deadCrewMember, GetObjectivePronounForDeadCrewMember()),
        "But who was it?",
        "You can't quite remember.",
        "There's no time to waste, Barry.",
        "Make a decision NOW.",
        string.Format("Who infected {0}?", deadCrewMember),
        "Who is the REAL alien?"
      };

      dialogPanel.StartDialog(dialog);
      EventController.OnDialogPanelClosed += OpenDecisionPanelForAct3;
    }
  }

    private void FinishEndOfAct3Dialog() {
    if (IsQuestCompleted("DefeatEnhancedParasiteQuest")) {
      string[] dialog = new string[] {
        string.Format("{0}: Barry! You KNOW me.", characterKilledDuringInterlude),
        "You know it isn't me!",
        string.Format("{0}'s pleas fall on your deaf ears.", characterKilledDuringInterlude),
        string.Format("You see the aliens lining up by {0}'s side.", characterKilledDuringInterlude),
        string.Format("You no longer see your ally."),
        string.Format("You only see it for what it truly is:"),
        string.Format("The Parasite Leader!")
      };
      dialogPanel.StartDialog(dialog);
      EventController.OnDialogPanelClosed += StartInterludeBattle;
    }
  }


  private void SetInfectedAndroidParty() {
    Enemy enemyToFind = characterController.FindEnemyByName(deadCrewMember);
    BattleLaunchCharacter infectedAndroid = null;

    NPC[] npcs = GameObject.FindObjectsOfType<NPC>();
    foreach(NPC n in npcs) {
      if (n.npcName == "Infected Android") {
        infectedAndroid = n.GetComponent<BattleLaunchCharacter>();
      }
    }

    if (infectedAndroid != null && enemyToFind != null) {
      infectedAndroid.AddEnemy(enemyToFind);
    }
  }

  private void StartInterludeBattle() {
      List<Enemy> enemies = new List<Enemy>();
      enemies.Add(characterController.FindEnemyByName("Parasite Leader"));
      enemies.Add(characterController.FindEnemyByName("Enhanced Parasite"));
      Player player = GameObject.FindObjectOfType<Player>();
      Vector2 playerPosition = new Vector2();
      if (player != null) {
        playerPosition = player.GetRigidbody().position;
        battleLauncher.PrepareBattle(playerPosition, enemies);
        questController.AssignQuest("InterludeQuest");
      }

      EventController.OnDialogPanelClosed -= StartInterludeBattle;
    }

  public void CompleteInterlude() {
    if (IsQuestCompleted("InterludeQuest") && shouldTellToGoToBridge) {
      string[] dialog = new string[] {
        "You defeat the Parasite Leader",
        "and inject it with what remains of the cure",
        "holding on to a thin glimmer of hope",
        "that you can bring your friend back.",
        "But you know it's no use.",
        "You release a heavy sigh of despair.",
        "But still.",
        "It's over at last.",
        "The Parasite Leader is dead.",
        "But somethingd doesn't feel quite right.",
        "You begin to wonder...",
        "What if there are more?",
        "What if killing the leader",
        "doesn't kill the rest?",
        "There's only one course of action that remains.",
        "Make your way to the Bridge.",
        "Activate the self-destruction.",
        "Warn the crew to escape while they still can.",
        "Hurry, Barry. Take the Teleporter."
      };
      dialogPanel.StartDialog(dialog);
      shouldTellToGoToBridge = false;
    }
    ActivateGatewayToBridge();
  }

  private bool PlayerIsAlien() {
    return characterKilledDuringInterlude == "Alan";
  }

  private bool AlanDismantledAndroid() {
    return characterRemovedFromPartyForOctopusFight == "Alan";
  }

  private bool PlayerSavedMegan() {
    return crewMemberWhoJoinedParty == "Megan";
  }

  public void TriggerSelfDestruct() {
    finalBattleEnemyParty.Clear();
    List<string> dialog = new List<string>();
    dialog.Add("You key in the self-destruct sequence");
    dialog.Add("and prepare to make an announcement to the ship.");
    dialog.Add("Barry: Attention, crew of the USS Hecate.");
    dialog.Add("We can no longer hold the ship.");
    dialog.Add("We've lost the Android.");
    dialog.Add("We've lost too many crew members.");
    dialog.Add("Hurry to the escape pods.");
    dialog.Add("I've triggered the self-destruct protocol.");
    dialog.Add("The ship will blow up in T-minus 20 minutes.");
    dialog.Add("May God have mercy on your souls.");
    dialog.Add("Ten minutes pass.");
    dialog.Add("One by one, crew members radio back to you.");
    dialog.Add("They are starting to make it to the pods.");
    dialog.Add("You feel a sense of relief.");
    dialog.Add("Even though you lost some good friends");
    dialog.Add("and even though you are about to");
    dialog.Add("go down with the ship as you defend");
    dialog.Add("the self-destruct panel,");
    dialog.Add("you feel a sense of relief");
    dialog.Add("These people will make it home.");
    dialog.Add("These people will make it back to their families.");
    dialog.Add("These people will see Earth again.");
    dialog.Add("And then the screaming begins.");
    dialog.Add("You hear them all cry out in agony");
    dialog.Add("as the Parasites claw their way out of their hosts");
    dialog.Add("You feel horror and shock as you realize");
    dialog.Add("that you have just freed the aliens");
    dialog.Add("and have sent them to Earth.");

    if (PlayerIsAlien()) {
      // Scenario 3A
      dialog.Add("You then feel something stir within you.");
      dialog.Add("Flashes of your battles from the last few hours.");
      dialog.Add(string.Format("{0} was not the one who doubled back.", characterKilledDuringInterlude));
      dialog.Add(string.Format("{0} was not the one who killed {1}.", characterKilledDuringInterlude, deadCrewMember));
      dialog.Add("Voice: It was you.");
      dialog.Add("Yes, Barry.");
      dialog.Add("Don't you see?");
      dialog.Add("It's always been you.");
      dialog.Add("Megan and Jake? They were never infected.");
      dialog.Add(string.Format("But I made you come back and infect {0}!", deadCrewMember));
      dialog.Add("I made you infect the Android!");
      dialog.Add("I made you botch the cure!");
      dialog.Add("Don't you get it, Barry?");
      dialog.Add("The moment you touched the Tentacle Monster");
      dialog.Add("you belonged to us.");
      dialog.Add("Remember? We told you that you were already dead");
      dialog.Add("And now your body is MINE!");
      dialog.Add("The best part though?");
      dialog.Add("You didn't set off any kind of self destruct sequence!");
      dialog.Add("You actually set off a DISTRESS BEACON!!!");
      dialog.Add("We'll infect the ship that responds,");
      dialog.Add("and together, we shall rule the universe!!!!");
      dialog.Add("Barry: No... stop... THAT'S ENOUGH");
      dialog.Add("You let out an anguished yelp.");
      dialog.Add(string.Format("You begin to glow, just as {0} did", deadCrewMember));
      dialog.Add("right before you administered the botched cure");
      dialog.Add(string.Format("{0}: Barry? Can you hear me?", crewMemberWhoJoinedParty));
      dialog.Add("You've got to fight it, Barry.");
      dialog.Add("I know you're still in there.");
      dialog.Add("So fight it! We're all counting on you");
      dialog.Add("to stop the Parasites.");
      dialog.Add("REMEMBER WHO YOU ARE!!!!");
      dialog.Add(string.Format("As {0} screams, you do remember everything.", crewMemberWhoJoinedParty));

      if (PlayerSavedMegan()) {
        // Scenarios 3 and 4
        dialog.Add("You remember the feeling of joy");
        dialog.Add("when you first met Megan");
        dialog.Add("You feel fear at the thought of losing her.");
        dialog.Add("And you embrace this fear.");
        dialog.Add("You feel the Parasite trying to escape your body,");
        dialog.Add("and you fight back");
        dialog.Add("It begins to shriek,");
        dialog.Add("unable to tolerate the love that flows through you.");
        dialog.Add("It phases through you, freeing you from its thrall.");
        dialog.Add("True Parasite: Filthy humans...");
        dialog.Add("I am getting off this putrid ship.");
        dialog.Add("Now stand aside or PERISH!!!!!!!!!!");

        if (AlanDismantledAndroid()) {
          dialog.Add("Barry: We aren't afraid of you.");
          dialog.Add("We have a connection,");
          dialog.Add("and I can see how much energy you used");
          dialog.Add("getting out of my body.");
          dialog.Add("You don't stand a chance!");

          // Scenario 4
          finalBattleEnemyParty[0].ReduceFinalBossStats(); // Reduce boss stats by 75%
          finalBattleScenario = 4;
          Debug.Log("Final battle scenario? " + finalBattleScenario);

        } else {
          // Scenario 3
          dialog.Add("Megan: We're in this together, Barry!");
          dialog.Add("I'm glad I could bring you back.");
          dialog.Add("When I was dismantling the Android,");
          dialog.Add("I found out about the emergency Battle Mechs,");
          dialog.Add("and there's one in here.");
          dialog.Add("Let's use it to show this alien");
          dialog.Add("that Earthlings don't mess around!!!");
          GivePlayerControlOfBattleMech(1);
          finalBattleScenario = 3;
          Debug.Log("Final battle scenario? " + finalBattleScenario);

        }
      } else {
        // Scenarios 1 and 2
        finalBossShouldBeAlien = false; // Instead, should activate the "good guys" to come be your enemies
        dialog.Add("You remember the pain and suffering you felt");
        dialog.Add("as your crew continued to drop one by one.");
        dialog.Add("You remember how good it felt to give in");
        dialog.Add("to the fear and paranoia");
        dialog.Add("while you slaughtered them.");
        dialog.Add("You remember who you are.");
        dialog.Add("You are the True Parasite.");
        dialog.Add("You tear apart Barry's flesh and reveal your true form.");
        dialog.Add("True Parasite: Pitiful human.");
        dialog.Add("My way off this ship is on its way.");
        dialog.Add("Now stand aside or PERISH");
        dialog.Add("Jake: Barry... what happened?");
        dialog.Add("True Parasite: Barry is long dead.");
        dialog.Add("Now it's your turn to join him!!!");

        Enemy jake = characterController.FindEnemyById(46);
        Enemy mech = characterController.FindEnemyById(45);

        if (AlanDismantledAndroid()) {
          // Scenario 2
          dialog.Add("Jake: You're dead, punk!");
          finalBattleEnemyParty.Add(jake);
          jake.IncreaseFinalBossStats();
          finalBattleScenario = 2;
          Debug.Log("Final battle scenario? " + finalBattleScenario);

        } else {
          // Scenario 1
          dialog.Add("Jake: Joke's on you, punk!");
          dialog.Add("When I was fixing up the Android, I found something.");
          dialog.Add("I found our break glass in case of emergency:");
          dialog.Add("Battle Mechs!");
          dialog.Add("They're under my control");
          dialog.Add("and are programmed to take you out!");
          finalBattleEnemyParty.Add(mech);
          finalBattleEnemyParty.Add(jake);
          finalBattleEnemyParty.Add(mech);
          finalBattleScenario = 1;
          Debug.Log("Final battle scenario? " + finalBattleScenario);
        }
      }

    } else {
      // Scenarios 5 and 6
      dialog.Add("Alan: Hehehehehe");
      dialog.Add("HAHAHAAHAH");
      dialog.Add("BARRY, YOU FOOOOOL!!!!!");
      dialog.Add("It was me all along!");
      dialog.Add("And I've been manipulating you this entire time!");
      dialog.Add("Megan and Jake were never infected");
      dialog.Add("until you gave me the opportunity!");
      dialog.Add(string.Format("I got you to shoot {0}", deadCrewMember));
      dialog.Add(string.Format("so that I could go back and infect {0},", GetObjectivePronounForDeadCrewMember()));
      dialog.Add(string.Format("and I manipulated you into shooting {0} too!", characterKilledDuringInterlude));
      dialog.Add("As soon as you came in contact");
      dialog.Add("with the Tentacle Monster,");
      dialog.Add("you were exposed to its spores.");
      dialog.Add("I've been able to plant suggestions in your mind.");
      dialog.Add("The best part though?");
      dialog.Add("You actually set off a DISTRESS BEACON!!!");
      dialog.Add("It's time to complete your transformation, Barry");
      dialog.Add("We'll infect the ship that responds,");
      dialog.Add("and together, we shall rule the universe!!!!");

      dialog.Add("Barry: No, Alan. I'll stop you.");
      dialog.Add("Alan: Don't you get it, you fool?");
      dialog.Add("It's futile to resist.");
      dialog.Add("I've sealed your fate.");
      dialog.Add("I forced you to botch the cure.");
      dialog.Add("It doesn't cure you filthy humans.");
      dialog.Add("It makes you into one of us!");

      if (AlanDismantledAndroid()) {
        // Scenario 5

      dialog.Add(string.Format("{0} was never infected", characterKilledDuringInterlude));
      dialog.Add("I made you see what I wanted you to see.");
      dialog.Add("Together we butchered your crew.");
      dialog.Add("What makes this even sweeter?");
      dialog.Add(string.Format("I didn't make you inject {0}'s corpse!", characterKilledDuringInterlude));
      dialog.Add("You did that all on your own!");
      dialog.Add(string.Format("Now, {0} truly is one of us", characterKilledDuringInterlude));
      dialog.Add("and remembers your betrayal.");
      dialog.Add(string.Format("{0} and I will make you one of us.", characterKilledDuringInterlude));
      dialog.Add("Resistance is futile!.");
      dialog.Add("Barry: It doesn't matter.");
      dialog.Add("Like I said, I'll stop you.");
      dialog.Add("With my authority as acting captain,");
      dialog.Add("I have access to the ship's emergency battle mechs");
      dialog.Add("They're programmed to kill you,");
      dialog.Add("and they won't stop until you're dead!!!");
      finalBattleScenario = 5;
      Debug.Log("Final battle scenario? " + finalBattleScenario);

      Enemy parasiteLeader = characterController.FindEnemyByName("Parasite Leader");
      Enemy infectedCrewMember = characterController.FindEnemyByName("Infected Crew Member");
      finalBattleEnemyParty.Add(parasiteLeader);
      finalBattleEnemyParty.Add(infectedCrewMember);

      GivePlayerControlOfBattleMech(2);
      } else {
        // Scenario 6

        dialog.Add("Barry: Heh... you're wrong.");
        dialog.Add("You didn't have control of me.");
        dialog.Add("Not completely, anyway.");
        dialog.Add("I sensed something was off");
        dialog.Add("when things started feeling too easy.");
        dialog.Add("It was just a little too easy");
        dialog.Add("to get the components for the cure.");
        dialog.Add("It felt like someone was guiding me.");
        dialog.Add("That's why I went against my instincts.");
        dialog.Add("My gut told me to grab certain parts");
        dialog.Add("from the mechs.");
        dialog.Add("So I ignored it.");
        dialog.Add("The cure you tried to hard to make me botch?");
        dialog.Add("I added a little something extra.");
        dialog.Add("A delay.");
        dialog.Add("Alan: You did WHAT?");
        dialog.Add("Barry: The real effects should have kicked in");
        dialog.Add("about 5 minutes after you and I left the Labs.");
        dialog.Add("Megan and Jake should be on their way up here,");
        dialog.Add("And together, we'll stop you!");
        dialog.Add("Oh, and they'll still have their extra strength");
        dialog.Add("that your nasty serum gave.");
        dialog.Add("So thanks for that, pal.");
        dialog.Add("Alan: Im... impossible!");
        dialog.Add("But wait...");
        dialog.Add("When we took control of the captain,");
        dialog.Add("We learned about your emergency battle mechs.");
        dialog.Add("And reprogrammed them to obey us");
        dialog.Add("and shoot humans on sight!");
        dialog.Add("They should be in this room, shouldn't they?");
        dialog.Add("Ah yes.");
        dialog.Add("Now you're done for.");
        dialog.Add("I'll kill all three of you!");
        dialog.Add("AND I WILL RULE THIS UNIVERSE MYSELF!!!");

        characterController.AddPlayerToParty("Megan");
        characterController.AddPlayerToParty("Jake");

        PartyMember megan = characterController.FindPartyMemberByName("Megan");
        PartyMember jake = characterController.FindPartyMemberByName("Jake");
        PartyMember barry = characterController.FindPartyMemberByName("Barry");
        List<PartyMember> members = new List<PartyMember>{megan, jake};

        members.ForEach(member => {
          member.experience = barry.experience;
          characterController.LevelUp(member);
          member.IncreaseFinalBossStats();
        });

      // Give two mechs
        Enemy mech = characterController.FindEnemyById(45);
        finalBattleEnemyParty.Add(mech);
        finalBattleEnemyParty.Add(mech);

        finalBattleScenario = 6;
        Debug.Log("Final battle scenario? " + finalBattleScenario);
      }
    }

    dialogPanel.StartDialog(dialog.ToArray());
    hasBossBeenRevealed = true;
    EventController.OnDialogPanelClosed += ActivateFinalBoss;
  }

  private void GivePlayerControlOfBattleMech(int count) {
    string characterName = "Battle Mech #" + count;
    characterController.AddPlayerToParty(characterName);
    PartyMember mech = characterController.FindPartyMemberByName(characterName);
    mech.experience = characterController.FindPartyMemberByName("Barry").experience;
    characterController.LevelUp(mech);
    inventoryController.GiveItem("Heavy Module");
    inventoryController.GiveItem("Water Module");

    count--;

    if (count > 0) {
      GivePlayerControlOfBattleMech(count);
    }
  }

  private void ActivateFinalBoss() {
    if (IsQuestInProgress("DefeatFinalBossQuest") || hasBossBeenRevealed) {
      Sprite trueParasiteSprite = Resources.Load<Sprite>("True Parasite");
      GameObject finalBossParent = GameObject.FindGameObjectWithTag("Final Boss");
        if (finalBossParent != null) {
          NPC finalBoss = finalBossParent.GetComponentInChildren<NPC>(true);
          if (finalBoss != null) {
            finalBoss.gameObject.SetActive(true);
          }

          if (finalBossShouldBeAlien) {
            Enemy boss = characterController.FindEnemyByName("The True Parasite");
            finalBattleEnemyParty.Add(boss);
            finalBoss.SetSprite(trueParasiteSprite);
          } else {
            Player player = GameObject.FindObjectOfType<Player>();
            player.SetSprite(trueParasiteSprite);
            PartyMember barry = characterController.FindPartyMemberByName("Barry");
            PartyMember parasite = characterController.FindPartyMemberByName("The True Parasite");

            parasite.experience = barry.experience;
            parasite.CopyPartyMember(barry);
            parasite.IncreaseFinalBossStats();

            characterController.RemovePlayerFromParty("Barry");
            characterController.AddPlayerToParty("The True Parasite");
          }
        }
        Resources.UnloadAsset(trueParasiteSprite);
      EventController.OnDialogPanelClosed -= ActivateFinalBoss;
    }
  }

  private void RemoveFinalBoss() {
    if (IsQuestCompleted("DefeatFinalBossQuest")) {
      GameObject finalBossParent = GameObject.FindGameObjectWithTag("Final Boss");
      if (finalBossParent != null) {
          Destroy(finalBossParent);
        }
      EventController.OnDialogPanelClosed -= RemoveFinalBoss;
      // TODO show end of game dialog
    }
  }

  private void RemoveFinalBossTrigger() {
    if (hasBossBeenRevealed) {
      FinalBossTrigger trigger = FindObjectOfType<FinalBossTrigger>();
      if (trigger != null) {
        Destroy(trigger.gameObject);
      }
    }
  }

  public void LaunchFinalBattle() {
    Player player = GameObject.FindObjectOfType<Player>();
    Vector2 playerPosition = new Vector2();
    if (player != null) {
      playerPosition = player.GetRigidbody().position;
      battleLauncher.PrepareBattle(playerPosition, this.finalBattleEnemyParty);
    }
  }
}
