using UnityEngine;
using UnityEngine.UI;

public class UIDecisionPanel : MonoBehaviour {
  [SerializeField] private Button decisionButton;
  private VerticalLayoutGroup buttonParent;

  private GameObject title;
  
  private void Awake() {
    title = GameObject.FindGameObjectWithTag("UI/Decision Panel/Title");
    buttonParent = GetComponentInChildren<VerticalLayoutGroup>();
  }

  public void SetTitle(string newTitle) {
    title.GetComponent<Text>().text = newTitle;
  }

  public void AddChoice(string choiceName) {
    Button button = Instantiate(decisionButton, buttonParent.transform);
    button.GetComponentInChildren<Text>().text = choiceName;
    button.onClick.AddListener(() => {
      gameObject.SetActive(false);
      EventController.DecisionMade(choiceName);
    });
  }

}
