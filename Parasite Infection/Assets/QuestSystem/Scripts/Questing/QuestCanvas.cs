using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCanvas : MonoBehaviour {
  private void Awake() {
    if (FindObjectsOfType<QuestCanvas>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
  }
}
