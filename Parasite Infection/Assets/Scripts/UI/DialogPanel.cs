using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : MonoBehaviour {
  [SerializeField] protected UnityEngine.UI.Text dialogText;
  [SerializeField] protected GameObject dialogPanel;
  protected string[] dialog = new string[]{};
  protected int dialogIndex;
  protected SceneController sceneController;

  private void Awake() {
    sceneController = FindObjectOfType<SceneController>();
  }

  public void StartDialog(string[] dialog, bool shouldPauseGame = true) {
    if (dialog != null && dialog.Length > 0) {
      dialogIndex = 0;
      this.dialog = dialog;
      if (dialogPanel != null) {
        dialogPanel.SetActive(true);
        dialogText.text = dialog[0];
        if (sceneController != null && shouldPauseGame) {
          sceneController.FreezeTime();
        }
      }
    } else {
      gameObject.SetActive(false);
    }
  }

  public void NextLine() {
    dialogIndex = Mathf.Min(dialogIndex + 1, dialog.Length);
    if (dialogIndex >= this.dialog.Length) {
      CloseDialog();
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

  public virtual void CloseDialog() {
    dialog = null;
    dialogText.text = "";
    dialogPanel.SetActive(false);
    dialogIndex = 0;
  }
}
