using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSummaryPanel : MonoBehaviour {

  [SerializeField] private Text titleText;
  [SerializeField] private Button loadLastSave;
  [SerializeField] private Button backToWorld;


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
    EventController.BattleLost();
  }

  public void EndBattle() {
    EventController.BattleWon();
  }
}
