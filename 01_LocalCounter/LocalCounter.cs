using TMPro;
using UdonSharp;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class LocalCounter : UdonSharpBehaviour
{
  public TextMeshProUGUI displayCount;

  private int _counter = 0;

  public void AddLocalCount()
  {
    _counter++;
    displayCount.text = _counter.ToString();
  }
}
