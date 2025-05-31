using TMPro;
using UdonSharp;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class LocalCounter : UdonSharpBehaviour {
  public TextMeshProUGUI displayCount;
  private int _counter = 0;

  void Start() {
    UpdateCounterText();
  }

  public void AddLocalCount() {
    _counter++;
    UpdateCounterText();
  }

  private void UpdateCounterText() {
    displayCount.text = _counter.ToString();
  }
}
