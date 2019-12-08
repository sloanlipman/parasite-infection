public class ActualDialogPanel : DialogPanel {
  private void Awake() {
    sceneController = FindObjectOfType<SceneController>();
  }

  public override void CloseDialog() {
    base.CloseDialog();
    EventController.DialogPanelClosed();
    if (sceneController != null) {
      sceneController.UnfreezeTime();
    }
  }
}
