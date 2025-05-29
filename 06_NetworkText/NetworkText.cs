using TMPro;
using UdonSharp;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class NetworkText : UdonSharpBehaviour {
  public TextMeshProUGUI textContainer;
  public VRCUrl url;

  void Start() {
    VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
  }

  public override void OnStringLoadSuccess(IVRCStringDownload result) {
    textContainer.text = result.Result;
  }

  public override void OnStringLoadError(IVRCStringDownload result) {
    textContainer.text = $"{result.ErrorCode} Error: {result.Error} ({result.Url})";
  }
}
