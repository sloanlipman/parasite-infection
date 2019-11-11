using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : MonoBehaviour {
  [SerializeField] private UnityEngine.UI.Text dialogText;
  [SerializeField] private GameObject dialogPanel;
  private string[] dialog;
  private int dialogIndex;
  private SceneController sceneController;

  private void Awake() {
    sceneController = FindObjectOfType<SceneController>();
  }

  public void StartDialog(string[] dialog) {
    dialogIndex = 0;
    this.dialog = dialog;
    if (dialogPanel != null) {
      dialogPanel.SetActive(true);
      dialogText.text = dialog[0];
      sceneController.FreezeTime();
    }
  }

  public void NextLine() {
    dialogIndex = Mathf.Min(dialogIndex + 1, dialog.Length);
    if (dialogIndex >= this.dialog.Length) {
      ResetDialog();
    } else {
      dialogText.text = dialog[dialogIndex];
    }
  }

  public void PreviousLine() {
    if (dialogIndex == 0) {
      return;
    }

    dialogIndex = Mathf.Max(dialogIndex - 1, 0);
    dialogText.text = dialog[dialogIndex];
  }

  public void ResetDialog() {
    dialog = null;
    dialogText.text = "";
    dialogPanel.SetActive(false);
    dialogIndex = 0;
    EventController.DialogPanelClosed();
    sceneController.UnfreezeTime();
  }
}
