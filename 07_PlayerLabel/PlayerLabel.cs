using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerLabel : UdonSharpBehaviour {
  [Header("Label Settings")]
  [SerializeField] private Vector3 _offsetAboveHead = new Vector3(0, 0.001f, 0);

  [Header("References")]
  [SerializeField] private TextMeshProUGUI _label;

  private VRCPlayerApi _owner;
  private VRCPlayerApi _localPlayer;

  void Start() {
    _localPlayer = Networking.LocalPlayer;
    _owner = Networking.GetOwner(gameObject);

    if (_label == null) {
      _label = GetComponentInChildren<TextMeshProUGUI>();
    }

    UpdateLabel();

    transform.rotation = Quaternion.identity;
  }

  void LateUpdate() {
    if (!ValidatePlayers()) return;

    UpdatePosition();
    UpdateRotationBillboard();
  }

  private bool ValidatePlayers() {
    if (_owner == null || !_owner.IsValid() || _localPlayer == null || !_localPlayer.IsValid()) {
      _owner = Networking.GetOwner(gameObject);
      _localPlayer = Networking.LocalPlayer;

      if (_owner == null || _localPlayer == null) {
        gameObject.SetActive(false);
        return false;
      }
    }
    return true;
  }

  private void UpdatePosition() {
    var headPosition = _owner.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
    transform.position = headPosition + _offsetAboveHead;

    float distance = CalculateAdjustedDistance();
    float scaleFactor = Mathf.Clamp((10 + distance) / 10, 1, 10);

    transform.localScale = Vector3.one * scaleFactor;
  }

  private float CalculateAdjustedDistance() {
    Vector3 labelPosition = transform.position;
    Vector3 localHeadPosition = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
    return Vector3.Distance(labelPosition, localHeadPosition);
  }



  private void UpdateRotationBillboard() {
    Quaternion cameraRotation = _localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
    transform.rotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
  }


  private void UpdateLabel() {
    if (_label == null || _owner == null) return;

    string playerName = _owner.displayName;

    if (playerName.Equals("VikiQAQ", System.StringComparison.OrdinalIgnoreCase)) {
      _label.text = $"[Hero] {playerName}";
    } else {
      _label.text = $"[Guest] {playerName}";
    }
  }

  public override void OnPlayerLeft(VRCPlayerApi player) {
    if (player == _owner) {
      Destroy(gameObject);
    }
  }
}
