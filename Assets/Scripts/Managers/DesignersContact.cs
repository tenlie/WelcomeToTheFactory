using UnityEngine;
using System.Collections;

public class DesignersContact : MonoBehaviour {

    public string composerURL;
    public string illustratorURL;

    public void ConnectToComposer()
    {
        Application.OpenURL(string.Format("http://{0}", composerURL));
    }

    public void ConnectToIllustrator()
    {
        Application.OpenURL(string.Format("http://{0}", illustratorURL));
    }
}
