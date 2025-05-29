using TMPro;
using UdonSharp;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class SliderCounter : UdonSharpBehaviour
{
  public TextMeshProUGUI displayCount;

  private Slider _slider;
  private float _sliderValue = 0f;

  void Start()
  {
    _slider = GetComponent<Slider>();
    UpdateText();
  }

  public void OnValueChanged()
  {
    _sliderValue = _slider.value;
    UpdateText();
  }

  private void UpdateText()
  {
    displayCount.text = (_sliderValue * 100).ToString("F0");
  }
}
