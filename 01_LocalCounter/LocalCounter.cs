using TMPro;
using UdonSharp;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class LocalCounter : UdonSharpBehaviour {
  public TextMeshProUGUI displayCount;

  private int _counter = 0;

  void Start() {
    UpdateText();
  }

  public void AddLocalCount() {
    _counter++;
    UpdateText();
  }

  private void UpdateText() {
    displayCount.text = _counter.ToString();
  }
}
