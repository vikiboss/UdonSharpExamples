using TMPro;
using UdonSharp;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SliderCounter : UdonSharpBehaviour
{
  public TextMeshProUGUI displayCount;

  private Slider _slider;

  [UdonSynced, FieldChangeCallback(nameof(sliderValue))]
  private float _sliderValue = 0f;

  private float sliderValue
  {
    get => _sliderValue;
    set
    {
      _sliderValue = value;
      UpdateTextAndSlider();
    }
  }

  void Start()
  {
    _slider = GetComponent<Slider>();
    UpdateTextAndSlider();
  }

  public void OnValueChanged(float? nextSliderValue)
  {
    if (Networking.IsOwner(gameObject))
    {
      sliderValue = nextSliderValue ?? _slider.value;
      RequestSerialization();
    }
    else
    {
      SendCustomNetworkEvent(NetworkEventTarget.Owner, nameof(OnValueChanged), _slider.value);
    }
  }

  private void UpdateTextAndSlider()
  {
    displayCount.text = (sliderValue * 100).ToString("F0");
    _slider.value = sliderValue;
  }
}
