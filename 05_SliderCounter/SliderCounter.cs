using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SliderCounter : UdonSharpBehaviour {
  public TextMeshProUGUI displayCount;

  private Slider _slider;
  private const float SYNC_THRESHOLD = 0.01f;


  [UdonSynced, FieldChangeCallback(nameof(sliderValue))]
  private float _sliderValue = 0f;

  private float sliderValue {
    get => _sliderValue;
    set {
      _sliderValue = value;
      UpdateTextAndSlider();
    }
  }

  void Start() {
    _slider = GetComponent<Slider>();
    UpdateTextAndSlider();
  }

  public void HandleValueChangedBySlider() {
    if (Mathf.Abs(_slider.value - _sliderValue) <= SYNC_THRESHOLD) return;

    if (Networking.IsOwner(gameObject)) {
      OnValueChanged(_slider.value);
    } else {
      SendCustomNetworkEvent(NetworkEventTarget.Owner, nameof(OnValueChanged), _slider.value);
    }
  }

  public void OnValueChanged(float nextValue) {
    if (Networking.IsOwner(gameObject)) {
      sliderValue = nextValue;
      RequestSerialization();
    }
  }

  private void UpdateTextAndSlider() {
    displayCount.text = (sliderValue * 100).ToString("F0");
    _slider.value = sliderValue;
  }
}
