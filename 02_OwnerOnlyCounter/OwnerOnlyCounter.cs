using TMPro;
using UdonSharp;
using VRC.SDKBase;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class OwnerOnlyCounter : UdonSharpBehaviour
{
  public TextMeshProUGUI displayCount;


  private Button btn;
  private Image btnImage;
  private TextMeshProUGUI btnText;


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
    btn = GetComponent<Button>();
    btnText = GetComponent<TextMeshProUGUI>();
    btnImage = GetComponent<Image>();

    UpdateText();
    UpdateButtonVisibility();
  }

  public void UpdateButtonVisibility()
  {
    bool isOwner = Networking.IsOwner(gameObject);

    btn.interactable = isOwner;
    btnText.enabled = isOwner;
    btnImage.enabled = isOwner;
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

  public override void OnDeserialization()
  {
    UpdateText();
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
