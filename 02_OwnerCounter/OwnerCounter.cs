using TMPro;
using UdonSharp;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class OwnerCounter : UdonSharpBehaviour {
  public TextMeshProUGUI displayCount;
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
    UpdateCounterText();
    UpdateButtonVisibility();
  }

  [NetworkCallable]
  public void AddOwnerCount() {
    if (isOwner) {
      counter++;
      RequestSerialization();
    } else {
      SendCustomNetworkEvent(NetworkEventTarget.Owner, nameof(AddOwnerCount));
    }
  }

  public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner) {
    return false;
  }

  public override void OnOwnershipTransferred(VRCPlayerApi player) {
    isOwner = Networking.IsOwner(gameObject);
    UpdateButtonVisibility();
  }

  private void UpdateCounterText() {
    displayCount.text = _counter.ToString();
  }

  private void UpdateButtonVisibility() {
    TextMeshProUGUI btnText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    btnText.text = isOwner ? "Add Count" : "Add at Owner";
  }
}
