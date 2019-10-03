using UnityEngine;
namespace QuestSystem {
  public class QuestCanvas : MonoBehaviour {
    void Start() {
      if (FindObjectsOfType<QuestCanvas>().Length > 1) {
        Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
    }
  }
}
