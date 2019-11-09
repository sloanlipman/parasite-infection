using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gateway : MonoBehaviour {

  [SerializeField]
  private string sceneName;

  [SerializeField]
  private Vector2 spawnLocation;

  public bool isActive = false;

 private void OnCollisionEnter2D(Collision2D collision) {
   if (collision.gameObject.CompareTag("Player") && isActive) {
     GatewayManager.Instance.SetSpawnPosition(spawnLocation);
     SceneManager.LoadScene(sceneName);
   }
 }
}
