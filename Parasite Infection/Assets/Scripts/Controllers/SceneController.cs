﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

  [SerializeField] private DialogPanel dialogPanel;
  [SerializeField] private DialogData gameIntroDialog;
  private int currentAct = 1;
  private bool hasPlayerDoneTutorial;

  private void Awake() {
    if (FindObjectsOfType<SceneController>().Length > 1) {
        Destroy(this.gameObject);
      }

    DontDestroyOnLoad(this.gameObject);
    SceneManager.sceneLoaded += OnSceneLoaded;

  }
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    switch(scene.name) {
      case "Intro": {
        DialogPanel introPanel = GameObject.FindGameObjectWithTag("UI/Intro Panel").GetComponent<DialogPanel>();
        introPanel.StartDialog(gameIntroDialog.dialog);
        EventController.OnDialogPanelClosed += LoadCommandCenter;
        break;
      }

      case "Command Center": {
        break;
      }
    }
  }

  void LoadCommandCenter() {
    SceneManager.LoadScene("Command Center");
    EventController.OnDialogPanelClosed -= LoadCommandCenter;
  }
}
