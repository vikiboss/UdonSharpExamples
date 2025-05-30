using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class PlayerLabel : UdonSharpBehaviour {
  [SerializeField] private Vector3 _offsetAboveHead = new(0, 0.001f, 0);
  [SerializeField] private TextMeshProUGUI _label;

  private VRCPlayerApi _owner;
  private VRCPlayerApi _localPlayer;

  void Start() {
    _label = GetComponentInChildren<TextMeshProUGUI>();
    _owner = Networking.GetOwner(gameObject);
    _localPlayer = Networking.LocalPlayer;
    UpdateLabelText();
  }

  void LateUpdate() {
    Vector3 ownerHeadPos = _owner.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
    Vector3 localHeadPos = _localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
    Quaternion cameraRotation = _localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

    transform.position = ownerHeadPos + _offsetAboveHead;
    float distance = Vector3.Distance(transform.position, localHeadPos);
    transform.localScale = Vector3.one * Mathf.Clamp((3 + distance) / 3, 1, 3);
    transform.rotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
  }

  private void UpdateLabelText() {
    string playerName = _owner.displayName;
    bool isSuperUser = playerName.Equals("VikiQAQ", System.StringComparison.OrdinalIgnoreCase);
    _label.text = isSuperUser ? $"[Hero] {playerName}" : $"[Guest] {playerName}";
  }
}
