﻿using System.Collections;
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

  private int currentAct = 0;
  private bool hasPlayerDoneTutorial;

// Central Core flags and strings
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

  private bool IsQuestCompleted(string questName) {
    return questController.IsQuestCompleted(questName);
  }

  private bool HasQuestBeenStarted(string questName) {
    return questController.HasQuestBeenStarted(questName);
  }

  public void Save() {
    ES3.Save<int>("currentAct", currentAct, "SceneController.json");
    ES3.Save<bool>("hasPlayerDoneTutorial", hasPlayerDoneTutorial, "SceneController.json");
    ES3.Save<string>("deadCrewMember", deadCrewMember, "SceneController.json");
    ES3.Save<string>("crewMemberWhoJoinedParty", crewMemberWhoJoinedParty, "SceneController.json");
    ES3.Save<bool>("hasJakeOrMeganBeenRemoved", hasJakeOrMeganBeenRemoved, "SceneController.json");
    ES3.Save<bool>("hasPlayerMadeAct1Decision", hasPlayerMadeAct1Decision, "SceneController.json");
    ES3.Save<bool>("isMalfunctioningAndroidDefeated", isMalfunctioningAndroidDefeated, "SceneController.json");
    ES3.Save<string>("characterRemovedFromPartyForOctopusFight", characterRemovedFromPartyForOctopusFight, "SceneController.json");
    ES3.Save<string>("characterKilledDuringInterlude", characterKilledDuringInterlude, "SceneController.json");
  }

  public void Load() {
    currentAct = ES3.Load<int>("currentAct", "SceneController.json", 1);
    hasPlayerDoneTutorial = ES3.Load<bool>("hasPlayerDoneTutorial", "SceneController.json", false);
    deadCrewMember = ES3.Load<string>("deadCrewMember", "SceneController.json", "");
    crewMemberWhoJoinedParty = ES3.Load<string>("crewMemberWhoJoinedParty", "SceneController.json", "");
    hasJakeOrMeganBeenRemoved = ES3.Load<bool>("hasJakeOrMeganBeenRemoved", "SceneController.json", false);
    hasPlayerMadeAct1Decision = ES3.Load<bool>("hasPlayerMadeAct1Decision", "SceneController.json", false);
    isMalfunctioningAndroidDefeated = ES3.Load<bool>("isMalfunctioningAndroidDefeated", "SceneController.json", false);
    characterRemovedFromPartyForOctopusFight = ES3.Load<string>("characterRemovedFromPartyForOctopusFight", "SceneController.json", "");
    characterKilledDuringInterlude = ES3.Load<string>("characterKilledDuringInterlude", "SceneController.json", "");
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

  private string GetSubjectivePronoun() {
    return deadCrewMember == "Jake" ? "he" : "she";
  }

  private string GetObjectivePronoun() {
    return deadCrewMember == "Jake" ? "him" : "her";
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

      case "Central Core": {
        currentAct = 1;
        if (!HasQuestBeenStarted("CraftWaterQuest")) {
            string[] dialog = new string[] {"Alan: Barry? I'm over here! Follow the green trail!"};
            dialogPanel.StartDialog(dialog);
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
        RemoveBirdMonster();
        RemoveDinosaurMonster();
        RemoveEvolvedBlob();
        RemoveInfectedAndroid();
        ActivateEnhancedParasite();
        RemoveEnhancedParasite();
        ActivateGatewayToLowerLabs();
        StartDefeatInfectedAndroidQuestCompletedDialog();
        StartDefeatEnhancedParasiteQuestCompletedDialog();
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

  private void ActivateGatewayToLowerLabs() {
    if (IsQuestCompleted("CompleteTheCureQuest")) {
      GameObject.FindGameObjectWithTag("Gateways/Lower Labs").GetComponent<Gateway>().isActive = true;
    }
  }

  private void ActivateGatewayToFinalBoss() {

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
      GameObject pigAlienGameObject = GameObject.FindGameObjectWithTag("Pig Alien");
      if (pigAlienGameObject != null) {
        BattleLaunchCharacter pigAlien = pigAlienGameObject.GetComponent<BattleLaunchCharacter>();
        if (pigAlien != null) {
          Destroy(pigAlien.gameObject);
        }
      }
      EventController.OnDialogPanelClosed -= RemovePigAlien;
      UnlockShedEntrance();
    }
  }

  public void RemoveEvolvedBlob() {
    if (IsQuestCompleted("DefeatEvolvedBlobQuest")) {
      GameObject evolvedBlobGameObject = GameObject.FindGameObjectWithTag("Evolved Blob");
      if (evolvedBlobGameObject != null) {
        BattleLaunchCharacter evolvedBlob = evolvedBlobGameObject.GetComponent<BattleLaunchCharacter>();
        if (evolvedBlob != null) {
          Destroy(evolvedBlob.gameObject);
        }
      }
    }
  }

  public void RemoveDinosaurMonster() {
    if (IsQuestCompleted("DefeatDinosaurMonsterQuest")) {
      GameObject dinosaurMonsterGameObject = GameObject.FindGameObjectWithTag("Dinosaur Monster");
      if (dinosaurMonsterGameObject != null) {
        BattleLaunchCharacter dinosaurMonster = dinosaurMonsterGameObject.GetComponent<BattleLaunchCharacter>();
        if (dinosaurMonster != null) {
          Destroy(dinosaurMonster.gameObject);
        }
      }
    }
  }

  public void RemoveBirdMonster() {
    if (IsQuestCompleted("DefeatBirdMonsterQuest")) {
      GameObject birdMonsterGameObject = GameObject.FindGameObjectWithTag("Bird Monster");
      if (birdMonsterGameObject != null) {
       BattleLaunchCharacter birdMonster = birdMonsterGameObject.GetComponent<BattleLaunchCharacter>();
       if (birdMonster != null) {
          Destroy(birdMonster.gameObject);
        }
      }
    }
  }

  public void StartCompleteTheCureQuestCompletedDialog() {
    string[] dialog = new string[] {
      "Kelly: This should cure everyone.",
      "But I'm picking up something weird.",
      "Can you go secure the lower labs?",
      "Take the Gateway you come in through."
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
        string.Format("but {0} begins to glow.", GetSubjectivePronoun()),
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
        string.Format("Someone infected {0} AFTER you killed {1}.", deadCrewMember, GetObjectivePronoun()),
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

    // Add dialog
    // Unlock the gateway
  }
}
