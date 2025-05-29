using TMPro;
using UdonSharp;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class WorldCounter : UdonSharpBehaviour
{
  public TextMeshProUGUI displayCount;

  [UdonSynced, FieldChangeCallback(nameof(counter))]
  private int _counter = 0;

  private int counter
  {
    get => _counter;
    set
    {
      _counter = value;
      UpdateText();
    }
  }

  void Start()
  {
    UpdateText();
  }

  private void UpdateText()
  {
    displayCount.text = counter.ToString();
  }

  public void AddWorldCount()
  {
    if (!Networking.IsOwner(gameObject))
    {
      Networking.SetOwner(Networking.LocalPlayer, gameObject);
    }

    counter++;
    RequestSerialization();
  }

  public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
  {
    return true;
  }
}
