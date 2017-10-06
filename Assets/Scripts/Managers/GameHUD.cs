using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour {

    public GUISkin Skin;
    public Transform _xaxis;
    public Transform Foregrounds;

    public void OnGUI()
    {
        /*
        GUI.skin = Skin;
        //Debug.Log("milliseconds: "+LevelManager.instance.LevelMusic.timeSamples / LevelManager.instance.LevelMusic.clip.frequency);
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            GUILayout.BeginVertical(Skin.GetStyle("GameHud"));
            {
                var time = LevelManager.Instance.RunningTime;
                GUILayout.Label(string.Format(
                    "{0:00}:{1:00}:{2:000} : {3:000000} : {4:000000} : {5:00000000} : {6:0000000}",
                    time.Minutes + (time.Hours * 60),
                    time.Seconds,
                    time.Milliseconds,
                    time.TotalMilliseconds / 1000,
                    LevelManager.Instance.LevelMusic.time,
                    LevelManager.Instance.LevelMusic.time * 12.8f - 12.8f,
                    Foregrounds.position.x,
                    Skin.GetStyle("TimeText")));

                //if ( time.ToString().Substring(0,10) == "00:00:46.0")
                //if ( time.ToString().Substring(0,10) == "00:01:00.6")
                    //Debug.Log("_xaxis.position.x: " + _xaxis.position.x);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    */
    }
}
