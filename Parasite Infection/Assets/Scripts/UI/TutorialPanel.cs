using UnityEngine.SceneManagement;

public class TutorialPanel : DialogPanel {

  public override void CloseDialog() {
    base.CloseDialog();
    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Intro")) {
      EventController.DialogPanelClosed();
    }
  }
}
