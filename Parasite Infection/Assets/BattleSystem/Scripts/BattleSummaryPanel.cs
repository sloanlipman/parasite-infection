using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSummaryPanel : MonoBehaviour {

  [SerializeField] private Text titleText;
  [SerializeField] private Button loadLastSave;
  [SerializeField] private Button backToWorld;
  private SaveService saveService;


  void Start() {
    saveService = FindObjectOfType<SaveService>();
  }

  void Update() {

  }

  public void ShowVictoryPanel() {
    gameObject.SetActive(true);
    titleText.text = "Victory!";
    loadLastSave.gameObject.SetActive(false);
  }

  public void ShowDefeatPanel() {
    gameObject.SetActive(true);
    titleText.text = "You're dead!";
    backToWorld.gameObject.SetActive(false);
  }

  public void LoadLastSave() {
    EndBattle();
    saveService.Load();
  }

  public void EndBattle() {
    EventController.BattleCompleted();
  }
}
