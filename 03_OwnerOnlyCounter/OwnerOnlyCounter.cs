using TMPro;
using UdonSharp;
using VRC.SDKBase;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class OwnerOnlyCounter : UdonSharpBehaviour {
  public TextMeshProUGUI displayCount;
  private Button button;
  private TextMeshProUGUI buttonText;
  private bool isOwner;
  [UdonSynced, FieldChangeCallback(nameof(counter))]
  private int _counter = 0;
  private int counter {
    get => _counter;
    set {
      _counter = value;
      UpdateCounterText();
    }
  }

  void Start() {
    isOwner = Networking.IsOwner(gameObject);
    button = GetComponent<Button>();
    buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

    UpdateCounterText();
    UpdateButtonStatus();
  }

  public void AddOwnerCount() {
    if (isOwner) {
      counter++;
      RequestSerialization();
    }
  }

  public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner) {
    return false;
  }

  public override void OnOwnershipTransferred(VRCPlayerApi player) {
    isOwner = Networking.IsOwner(gameObject);
    UpdateButtonStatus();
  }

  private void UpdateButtonStatus() {
    button.interactable = isOwner;
    buttonText.text = isOwner ? "Add Count" : "Owner Only";
  }

  private void UpdateCounterText() {
    displayCount.text = _counter.ToString();
  }
}
