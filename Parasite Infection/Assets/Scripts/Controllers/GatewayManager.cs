using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatewayManager : MonoBehaviour {

  private Vector2 spawnPosition;
  private bool spawnPrepared;
  public static GatewayManager Instance {get; set;}
  // Start is called before the first frame update
  void Start() {
    if (Instance != null && Instance != this) {
      Destroy(this.gameObject);
    }
    else {
      Instance = this;
    }

    DontDestroyOnLoad(this.gameObject);
    SceneManager.sceneLoaded += SceneLoaded;
  }

  private void SceneLoaded(Scene scene, LoadSceneMode mode) {
    MoveInNewScene();
  }

  public void MoveInNewScene() {
    if (spawnPrepared) {
      MovePosition();
    }
  }

  public void SetSpawnPosition (Vector2 spawnPosition) {
    spawnPrepared = true;
    this.spawnPosition = spawnPosition;
  }

  private void MovePosition() {
    FindObjectOfType<Player>().TeleportTo(spawnPosition);
    spawnPrepared = false;
  }
}
