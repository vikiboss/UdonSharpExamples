using TMPro;
using UdonSharp;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class WorldCounter : UdonSharpBehaviour {
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
  }

  private void UpdateCounterText() {
    displayCount.text = _counter.ToString();
  }

  public void AddWorldCount() {
    if (!isOwner) {
      Networking.SetOwner(Networking.LocalPlayer, gameObject);
    }

    counter++;
    RequestSerialization();
  }

  public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner) {
    isOwner = requestingPlayer.isLocal;
    return true;
  }
}
