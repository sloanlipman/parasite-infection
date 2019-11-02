using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCanvas : MonoBehaviour {

  void Start() {
    if (FindObjectsOfType<DialogCanvas>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
  }
}
