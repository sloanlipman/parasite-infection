using UnityEngine.SceneManagement;

public static class Helper {
  public static bool isBattleCurrentScene() {
    return SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Battle");
  }
}
