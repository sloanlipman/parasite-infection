using UnityEngine;

public class Gateway : MonoBehaviour {

  [SerializeField] private string sceneName;
  [SerializeField] private Vector2 spawnLocation;
  private SceneController sceneController;


  private void Awake() {
    sceneController = FindObjectOfType<SceneController>();
  }

  public bool isActive = false;

 private void OnCollisionEnter2D(Collision2D collision) {
   if (collision.gameObject.CompareTag("Player") && isActive) {
     GatewayManager.Instance.SetSpawnPosition(spawnLocation);
     Debug.Log("Gateway trying to load scene: " + sceneName);
     sceneController.LoadSceneFromGateway(sceneName);
   }
 }
}
