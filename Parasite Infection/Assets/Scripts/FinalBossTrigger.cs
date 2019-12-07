using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossTrigger : MonoBehaviour {
  private SceneController sceneController;

  void Awake() {
    sceneController = FindObjectOfType<SceneController>();
  }

  public void Interact() {
    this.sceneController.TriggerSelfDestruct();
  }
}
