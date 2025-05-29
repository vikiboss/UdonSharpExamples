using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SliderCounter : UdonSharpBehaviour {
  public TextMeshProUGUI displayCount;

  private Slider _slider;
  private const float SYNC_THRESHOLD = 0.01f;
  private const float SYNC_THROTTLE = 0.1f;
  private float lastChangeAt = 0;

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
    if (Time.time != 0 && Time.time - lastChangeAt < SYNC_THROTTLE) return;
    if (Mathf.Abs(_slider.value - _sliderValue) <= SYNC_THRESHOLD) return;

    lastChangeAt = Time.time;

    if (Networking.IsOwner(gameObject)) {
      OnValueChanged(_slider.value);
    } else {
      SendCustomNetworkEvent(NetworkEventTarget.Owner, "OnValueChanged", _slider.value);
    }
  }

  [NetworkCallable]
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
