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
  private bool isOwner;
  private bool isSyncing = false;
  private float lastChangeAt = 0;
  private const float SYNC_THROTTLE = 1f;

  [UdonSynced, FieldChangeCallback(nameof(sliderValue))]
  private float _sliderValue = 0f;

  private float sliderValue {
    get => _sliderValue;
    set {
      _sliderValue = value;
      _slider.value = value;
      isSyncing = false;
      UpdateSliderText();
    }
  }

  void Start() {
    isOwner = Networking.IsOwner(gameObject);
    _slider = GetComponent<Slider>();
    UpdateSliderText();
  }

  void LateUpdate() {
    bool isIdle = Time.time - lastChangeAt >= SYNC_THROTTLE;
    bool isValueChanged = Mathf.Abs(_slider.value - sliderValue) >= 0.01f;

    if (!isSyncing && isIdle && isValueChanged) {
      if (isOwner) {
        OnValueChanged(_slider.value);
      } else {
        isSyncing = true;
        SendCustomNetworkEvent(NetworkEventTarget.Owner, "OnValueChanged", _slider.value);
      }
    }
  }

  public void HandleValueChangedBySlider() {
    lastChangeAt = Time.time;
    UpdateSliderText();
  }

  [NetworkCallable]
  public void OnValueChanged(float nextValue) {
    sliderValue = nextValue;
    RequestSerialization();
  }

  private void UpdateSliderText() {
    displayCount.text = (_slider.value * 100).ToString("F0");
  }
}
