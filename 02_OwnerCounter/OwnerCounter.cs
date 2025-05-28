using TMPro;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class OwnerCountCanvas : UdonSharpBehaviour
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
    UpdateButtonVisibility();
  }

  public void UpdateButtonVisibility()
  {
    TextMeshProUGUI btnText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    btnText.text = Networking.IsOwner(gameObject) ? "Add Count" : "Notice Owner";
  }

  private void UpdateText()
  {
    displayCount.text = counter.ToString();
  }

  public void AddOwnerCount()
  {
    if (Networking.IsOwner(gameObject))
    {
      counter++;
      RequestSerialization();
    }
    else
    {
      SendCustomNetworkEvent(NetworkEventTarget.Owner, nameof(AddOwnerCount));
    }
  }

  public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
  {
    return false;
  }

  public override void OnOwnershipTransferred(VRCPlayerApi player)
  {
    UpdateButtonVisibility();
  }
}
