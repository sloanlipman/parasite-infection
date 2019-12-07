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

  public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
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
            "<i><b>Check in with the Android to get your assignment. If you need help, press ESC to access the Tutorial!</b></i>",
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
      "Alan: Good stuff. This will help a lot. Let's get outta here. The captain should be right up ahead.",
      "I've opened the security gate. I'm right behind you, Barry."
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += OpenGateToTentacleMonster;
  }

  public void StartDefeatTentacleMonsterQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Tentacle Monster: IMPOSSIBLE!!! How could a human defeat my spawn? I won't forget this... Barry",
      "Alan: Let's head back to the teleporter so we can secure the Central Core."
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += RemoveTentacleMonster;
  }

  public void StartKillPigAlienQuestCompletedDialog() {
    string[] dialog = new string[] {
    "Pig Farmer: Thanks for the help! I think I saw the Android go into the shed. Here's the key!"
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
      "Android: Sensors are only picking up one alien now...",
      "Alan: These aliens must have mind control powers. Barry, what do we do?",
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += OpenDecisionPanelForAct1;
  }

    public void ActivatePigAlien() {
    GameObject pigAlienParent = GameObject.FindGameObjectWithTag("Pig Alien");
      if (pigAlienParent != null) {
        NPC pigAlien = pigAlienParent.GetComponentInChildren<NPC>(true);
        if (pigAlien != null) {
          pigAlien.gameObject.SetActive(IsQuestInProgress("KillPigAlienQuest"));
      }
    }
  }

  public void StartDefeatMalfunctioningAndroidQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Alan: Bar, there's a nasty Parasite incoming, but one of us needs to finish turning off the Android",
      "Who's it going to be?"
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += OpenDecisionPanelForAct2;
  }

  public void StartDefeatOctopusMonsterQuestCompletedDialog() {
    string[] dialog = new string[] {
      string.Format("{0}: I've deactivated him. He had some info about a potential cure. Let's get to the Labs.", characterRemovedFromPartyForOctopusFight),
    };
    dialogPanel.StartDialog(dialog);
    RemoveOctopusMonster();
    currentAct = 3;
    characterController.AddPlayerToParty(characterRemovedFromPartyForOctopusFight);
  }

  private void ActivateOctopusMonster() {
    GameObject octopusMonsterParent = GameObject.FindGameObjectWithTag("Act 2 Boss");
    if (octopusMonsterParent != null) {
      NPC octopusMonster = octopusMonsterParent.GetComponentInChildren<NPC>(true);
      if (octopusMonster != null) {
        octopusMonster.gameObject.SetActive(IsQuestCompleted("DefeatMalfunctioningAndroidQuest"));
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
    GameObject enhancedParasiteParent = GameObject.FindGameObjectWithTag("Act 3 Boss");
    if (enhancedParasiteParent != null) {
      NPC enhancedParasite = enhancedParasiteParent.GetComponentInChildren<NPC>(true);
      if (enhancedParasite != null) {
        enhancedParasite.gameObject.SetActive(IsQuestCompleted("DefeatInfectedAndroidQuest"));
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
      "Android: Alien life detected in the Biosphere. I'll go on ahead.",
      string.Format("{0}, stick with these guys. Here's an extra Heavy Module.", crewMemberWhoJoinedParty),
      "Take this extra Heavy Module."
    };
    dialogPanel.StartDialog(dialog);
    EventController.OnDialogPanelClosed += RemoveJakeOrMegan;
  }

  private void StartLabsDialog() {
    if (!HasQuestBeenStarted("CompleteTheCureQuest")) {
      string[] dialog = new string[] {
        "<i><b>A voice echoes in your head...</b></i>",
        "<i><b>Kelly... The scientist... She has the cure... We must get to her before it's too late....</b></i>"
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
    GameObject evolvedBlobParent = GameObject.FindGameObjectWithTag("Evolved Blob");
    if (evolvedBlobParent != null) {
      NPC evolvedBlob = evolvedBlobParent.GetComponentInChildren<NPC>(true);
      if (evolvedBlob != null) {
        evolvedBlob.gameObject.SetActive(IsQuestInProgress("DefeatEvolvedBlobQuest"));
      }
    }
  }
   public void ActivateDinosaurMonster() {
    GameObject dinosaurMonsterParent = GameObject.FindGameObjectWithTag("Dinosaur Monster");
    if (dinosaurMonsterParent != null) {
      NPC dinosaurMonster = dinosaurMonsterParent.GetComponentInChildren<NPC>(true);
      if (dinosaurMonster != null) {
        dinosaurMonster.gameObject.SetActive(IsQuestInProgress("DefeatDinosaurMonsterQuest"));
      }
    }
  }
   public void ActivateBirdMonster() {
    GameObject birdMonsterParent = GameObject.FindGameObjectWithTag("Bird Monster");
    if (birdMonsterParent != null) {
      NPC birdMonster = birdMonsterParent.GetComponentInChildren<NPC>(true);
      if (birdMonster != null) {
        birdMonster.gameObject.SetActive(IsQuestInProgress("DefeatBirdMonsterQuest"));
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
      "Kelly: That should do it! But now I'm picking up some weird readings. Aliens in the Lower Labs!",
      "Take the teleporter you came in through and go stop the threat!",
    };
    dialogPanel.StartDialog(dialog);
    ActivateGatewayToLowerLabs();
  }

  public void StartDefeatInfectedAndroidQuestCompletedDialog() {
    if (IsQuestCompleted("DefeatInfectedAndroidQuest") && !HasQuestBeenStarted("DefeatEnhancedParasiteQuest")) {
      string[] dialog = new string[] {
        string.Format("Alan: Wait a second! Is that {0}!? Barry, use the cure! We can bring {1} back!", deadCrewMember, GetObjectivePronounForDeadCrewMember()),
        string.Format("<i><b>You inject {0} with the cure, but {1} begins to grow. In a few seconds, you don't recognize {1}.</b></i>", deadCrewMember, GetObjectivePronounForDeadCrewMember(), GetSubjectivePronounForDeadCrewMember()),
        "The cure appears to have failed. You have no choice but to fight the ally you have already lost."
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

        string.Format("<i><b>You realize something. {0} wasn't even infected in your first battle.</b></i>", deadCrewMember),
        "<i><b>You see a shadowy image in a memory. Someone doubled back to the Central Core.</b></i>",
        string.Format("<i><b>After you killed {0}, someone went back.</b></i>", deadCrewMember),
        string.Format("<i></b>Someone infected {0} AFTER you killed {1}.</b></i>", deadCrewMember, GetObjectivePronounForDeadCrewMember()),
        "<i><b>Who was it though? You can't remember. But there's no time to waste, Barry.</b></i>",
        "<i><b>Make a decision NOW.</b></i>",
        string.Format("<i><b>Who infected {0}?</b></i>", deadCrewMember),
        "<i><b>Who is the REAL alien?</b></i>"
      };

      dialogPanel.StartDialog(dialog);
      EventController.OnDialogPanelClosed += OpenDecisionPanelForAct3;
    }
  }

    private void FinishEndOfAct3Dialog() {
    if (IsQuestCompleted("DefeatEnhancedParasiteQuest")) {
      string[] dialog = new string[] {
        string.Format("{0}: Barry! You KNOW me. You know it isn't me!!!", characterKilledDuringInterlude),
        "You know it isn't me!",
        string.Format("<b><i>{0}'s pleas fall on your deaf ears.</i></b>", characterKilledDuringInterlude),
        string.Format("<b><i>You see the aliens lining up by {0}'s side.</i></b>", characterKilledDuringInterlude),
        string.Format("<b><i>You no longer see your ally. You only see it for what it truly is: The Parasite Leader.</i></b>")
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
        "<i><b>You defeat the Parasite Leader and inject it with what remains of the cure.</b></i>",
        "<i><b>You hold on to a thin glimmer of hope that you can bring your friend back.</b></i>",
        "<i><b>Nothing happens.</b></i>",
        "<i><b>You sign in despair, but at least it's over. The Parasite Leader is dead.</b></i>",
        "<i><b>Something still feels off though. You begin to wonder...</b></i>",
        "<i><b>What if there is a stronger one out there? What if you didn't really stop the infection?</b></i>",
        "<i><b>There's only one course of action that remains. Make your way to the Bridge.</b></i>",
        "<i><b>Activate the self-destruction. Warn the crew to escape while they still can.</b></i>",
        "<i><b>Hurry, Barry....</b></i>"
      };
      dialogPanel.StartDialog(dialog);
      shouldTellToGoToBridge = false;
      currentAct = 4;
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
    dialog.Add("<i><b>You key in the self-destruct sequence and prepare to make an announcement to the ship.</b><i>");
    dialog.Add("Barry: Attention, crew of the USS Hecate. We can no longer hold the ship.");
    dialog.Add("We've lost the Android. We've lost too many crew members.");
    dialog.Add("Hurry to the escape pods.");
    dialog.Add("I've triggered the self-destruct protocol. The ship will blow up in T-minus 20 minutes.");
    dialog.Add("May God have mercy on your souls.");
    dialog.Add("<i><b>Ten minutes pass.</b></i>");
    dialog.Add("<i><b>One by one, crew members radio back to you.</i></b>");
    dialog.Add("<i><b>They are starting to make it to the pods.</i></b>");
    dialog.Add("<i><b>You feel a sense of relief.</i></b>");
    dialog.Add("<i><b>Even though you lost some good friends</i></b>");
    dialog.Add("<i><b>and even though you are about to go down with the ship</i></b>");
    dialog.Add("<i><b>as you defend the self-destruct panel, you feel a sense of relief</i></b>");
    dialog.Add("<i><b>These people will make it home. Back to their families. Back to Earth.</i></b>");
    dialog.Add("<i><b>And then the screaming begins.</i></b>");
    dialog.Add("<i><b>You hear them all cry out in agony as the Parasites claw their way out of their hosts</i></b>");
    dialog.Add("<i><b>You feel horror and shock as you realize that you have just sent an army of Parasites to Earth...</i></b>");

    if (PlayerIsAlien()) {
      // Scenario 3A
      dialog.Add("<i><b>You then feel something stir within you. Flashes of your battles from the last few hours.</b></i>");
      dialog.Add(string.Format("{0} was not the one who doubled back.", characterKilledDuringInterlude));
      dialog.Add(string.Format("{0} was not the one who killed {1}.", characterKilledDuringInterlude, deadCrewMember));
      dialog.Add("Voice: It was you. Don't you see, Barry? It's always been you.");
      dialog.Add(string.Format("Megan and Jake? They were never infected. But I made you come back and infect {0}!", deadCrewMember));
      dialog.Add("I made you infect the Android AND botch the cure!!!");
      dialog.Add("I made you botch the cure!");
      dialog.Add("Don't you get it, Barry? The moment you touched the Tentacle Monster, you belonged to us.");
      dialog.Add("Remember? We told you that you were already dead");
      dialog.Add("And now your body is MINE!");
      dialog.Add("The best part though?");
      dialog.Add("You didn't set off any kind of self destruct sequence!");
      dialog.Add("You actually set off a DISTRESS BEACON!!!");
      dialog.Add("We'll infect the ship that responds, and together, we shall rule the universe!!!!");
      dialog.Add("Barry: No... stop... THAT'S ENOUGH");
      dialog.Add("<i><b>You let out an anguished yelp.</b></i>");
      dialog.Add(string.Format("You begin to glow, just as {0} did right before you administered the botched cure</b></i>", deadCrewMember));
      dialog.Add(string.Format("{0}: Barry? Can you hear me?", crewMemberWhoJoinedParty));
      dialog.Add("You've got to fight it, Barry. I know you're still in there.");
      dialog.Add("So fight it! We're all counting on you to stop the Parasites!");
      dialog.Add("REMEMBER WHO YOU ARE!!!!");
      dialog.Add(string.Format("<i><b>As {0} screams, you do remember everything.</b></i>", crewMemberWhoJoinedParty));

      if (PlayerSavedMegan()) {
        // Scenarios 3 and 4
        dialog.Add("<i><b>You remember the feeling of joy when you first met Megan</b></i>");
        dialog.Add("<i><b>You feel fear at the thought of losing her and find strength in it.</b></i>");
        dialog.Add("<i><b>You feel the Parasite trying to commandeer your body, and you fight back.</b></i>");
        dialog.Add("<i><b>It begins to shriek,unable to tolerate the love that flows through you.</b></i>");
        dialog.Add("It phases through you, freeing you from its thrall.");
        dialog.Add("True Parasite: Filthy humans... I am getting off this putrid ship.");
        dialog.Add("Now stand aside or PERISH!!!!!!!!!!");

        if (AlanDismantledAndroid()) {
          dialog.Add("Barry: We aren't afraid of you. We're still connected.");
          dialog.Add("I can feel how much energy you used getting out of my body.");
          dialog.Add("You don't stand a chance!");

          // Scenario 4
          finalBattleEnemyParty[0].ReduceFinalBossStats(); // Reduce boss stats by 75%
          finalBattleScenario = 4;
          Debug.Log("Final battle scenario? " + finalBattleScenario);

        } else {
          // Scenario 3
          dialog.Add("Megan: Barry! I'm glad I could bring you back. We're in this together, until the end.");
          dialog.Add("When I was dismantling the Android, I got access to the emergency Battle Mechs");
          dialog.Add("Let's use one to show this alien that Earthlings don't mess around!");

          GivePlayerControlOfBattleMech(1);

          finalBattleScenario = 3;
          Debug.Log("Final battle scenario? " + finalBattleScenario);

        }
      } else {
        // Scenarios 1 and 2
        finalBossShouldBeAlien = false; // Instead, should activate the "good guys" to come be your enemies
        dialog.Add("<i><b>You remember the pain and suffering you felt as your crew dropped one by one.</b></i>");
        dialog.Add("<i><b>You remember how good it felt to give in to the fear and paranoia while you slaughtered them.</b></i>");
        dialog.Add("<i><b>You remember who you are. You are the True Parasite.</b></i>");
        dialog.Add("i><b>You tear apart Barry's flesh and reveal your true form.</b></i>");
        dialog.Add("True Parasite: Pitiful human. Stand aside or PERISH.");
        dialog.Add("Now stand aside or PERISH");
        dialog.Add("Jake: Barry... what happened?");
        dialog.Add("True Parasite: Barry is long dead. Now it's your turn to join him!!!!!!");

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
          dialog.Add("Jake: Joke's on you, punk! When I was taking apart the Android, I found out about something.");
          dialog.Add("We have emergency Battle Mechs that are gonna take you out! I WILL AVENGE MY FRIENDS!!");
          finalBattleEnemyParty.Add(mech);
          finalBattleEnemyParty.Add(jake);
          finalBattleEnemyParty.Add(mech);
          finalBattleScenario = 1;
          Debug.Log("Final battle scenario? " + finalBattleScenario);
        }
      }

    } else {
      // Scenarios 5 and 6
      dialog.Add("Alan: Hehehehehe... HAHAHAAHAH... BARRY, YOU FOOOOL!!!!");
      dialog.Add("HAHAHAAHAH");
      dialog.Add("BARRY, YOU FOOOOOL!!!!!");
      dialog.Add("It's been me all along! I've been manipulating you this entire time!");
      dialog.Add("Megan and Jake were never infected until you gave me the opportunity!");
      dialog.Add(string.Format("I got you to shoot {0} so that I could go back and infect {1}! I got you to shoot {2} too!", deadCrewMember, GetObjectivePronounForDeadCrewMember(), characterKilledDuringInterlude));
      dialog.Add("As soon as you came in contact with the Tentacle Monster, you were exposed to its spores!");
      dialog.Add("I've been able to plant suggestions in your mind.");
      dialog.Add("The best part though?");
      dialog.Add("You actually set off a DISTRESS BEACON!!!");
      dialog.Add("It's time to complete your transformation, Barry");
      dialog.Add("We'll infect the ship that responds, and together, we shall rule the universe!!!!");
      dialog.Add("Barry: No, Alan. I'll stop you.");
      dialog.Add("Alan: Don't you get it, you fool? It's futile to resist. I've sealed your fate.");
      dialog.Add("I forced you to botch the cure. It doesn't cure you filthy humans! It makes you into one of us!");

      if (AlanDismantledAndroid()) {
        // Scenario 5

      dialog.Add(string.Format("{0} was never infected. I made you see what I <b>wanted</b> you to see! Together we BUTCHERED your crew!", characterKilledDuringInterlude));
      dialog.Add(string.Format("I didn't make you inject {0}'s corpse! You did that all on your own!", characterKilledDuringInterlude));
      dialog.Add(string.Format("{0}} remembers your betray, and we will make you join us now. Resistance is <B>FUTILE</B>", characterKilledDuringInterlude));
      dialog.Add("Barry: It doesn't matter, Alan. Like I said, I'll stop you.");
      dialog.Add("As acting captain, I have direct acess to our emergency Battle Mechs.");
      dialog.Add("While you've been blabbering, I've programmed them to kill you. Then won't stop until the infection is over!");

      finalBattleScenario = 5;
      Debug.Log("Final battle scenario? " + finalBattleScenario);

      Enemy parasiteLeader = characterController.FindEnemyByName("Parasite Leader");
      Enemy infectedCrewMember = characterController.FindEnemyByName("Infected Crew Member");
      finalBattleEnemyParty.Add(parasiteLeader);
      finalBattleEnemyParty.Add(infectedCrewMember);

      GivePlayerControlOfBattleMech(2);
      } else {
        // Scenario 6

        dialog.Add("Barry: Heh... you're wrong. You didn't have control over me. Not completely, anyway.");

        dialog.Add("Something felt off when we were making the cure. It felt too... easy. Like someone guiding me.");
        dialog.Add("It felt like someone was guiding me.");
        dialog.Add("That's why I went against my instincts.");
        dialog.Add("When we were dismantling the mechs for the delivery device, my gut told me to take certain parts.");
        dialog.Add("I ignored the voice in my head. I ignored your voice. I took a few different parts to change the way the cure worked.");
        dialog.Add("I added a delay.");
        dialog.Add("Alan: You did WHAT?");
        dialog.Add("Barry: The <b>real</b> effects should have kicked in about 5 minutes after you and I got up here.");
        dialog.Add("Megan and Jake should be on their way up here,");
        dialog.Add("Megan and Jake are on their way up here, and together, we WILL stop you!!!");
        dialog.Add("Oh, and the extra strength from your nasty serum? They'll keep that. So thanks, pal.");
        dialog.Add("Alan: Im... im.... IMPOSSIBLE!!!!!!");
        dialog.Add("Alan: Heh... I just remembered something. When we took over the captain's body, we learned a nice secret.");
        dialog.Add("The Hecate is equipped with emergency Battle Mechs.");
        dialog.Add("They've been programmed to obey us and shoot humans on sight.");
        dialog.Add("They should be in this room, shouldn't they?");
        dialog.Add("I believe there are some hidden in this very room, aren't there? Ah yes. Now you're done for.");
        dialog.Add("<b>I WILL KILL ALL THREE OF YOU AND RULE THIS WRETECHED UNIVERSE ALONE!!!!!</b>");

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
    Sprite trueParasiteSprite = Resources.Load<Sprite>("True Parasite");
    GameObject finalBossParent = GameObject.FindGameObjectWithTag("Final Boss");
      if (finalBossParent != null) {
        NPC finalBoss = finalBossParent.GetComponentInChildren<NPC>(true);
        if (finalBoss != null) {
          finalBoss.gameObject.SetActive(IsQuestInProgress("DefeatFinalBossQuest") || hasBossBeenRevealed);
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
