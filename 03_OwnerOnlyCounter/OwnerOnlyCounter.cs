using TMPro;
using UdonSharp;
using VRC.SDKBase;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class OwnerOnlyCounter : UdonSharpBehaviour
{
  public TextMeshProUGUI displayCount;


  private Button button;


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
    button = GetComponent<Button>();

    UpdateText();
    UpdateButtonVisibility();
  }

  public void UpdateButtonVisibility()
  {
    bool isOwner = Networking.IsOwner(gameObject);
    button.interactable = isOwner;
    TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();
    btnText.text = isOwner ? "Add Count" : "Owner Only";
  }

  public void UpdateText()
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
