using TMPro;
using UdonSharp;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class NetworkText : UdonSharpBehaviour {
  public TextMeshProUGUI text;
  public VRCUrl url;

  void Start() {
    VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
  }

  public override void OnStringLoadSuccess(IVRCStringDownload result) {
    text.text = result.Result;
  }

  public override void OnStringLoadError(IVRCStringDownload result) {
    text.text = $"{result.ErrorCode} Error: {result.Error} ({result.Url})";
  }
}
