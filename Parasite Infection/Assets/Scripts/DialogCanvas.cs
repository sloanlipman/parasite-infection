using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCanvas : MonoBehaviour {
  [SerializeField] private UnityEngine.UI.Text dialogText;
  [SerializeField] private GameObject dialogPanel;
  private string[] dialog;
  private int dialogIndex;

  void Start() {
    if (FindObjectsOfType<DialogCanvas>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
  }
}
