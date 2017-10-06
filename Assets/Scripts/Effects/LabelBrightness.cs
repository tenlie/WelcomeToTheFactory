using UnityEngine;
using System.Collections;

public class LabelBrightness : MonoBehaviour
{
    //public Material targetMaterial { get; set; }
    private float startVal;
    private float endVal;
    private float duration;
    private float time;
    public bool isBtnPressed { get; set; }
    public UILabel Label { get; set; }

    // Use this for initialization
    void Start()
    {
        startVal = 0.5f;
        endVal = 1.0f;
        time = 0.0f;
        duration = 1.0f;
        isBtnPressed = false;
        Label = GetComponent<UILabel>();
        Label.onRender = SetBrightness; //Material 매개변수 필요
    }

    void SetBrightness(Material mat)
    {
        Debug.Log("SetBrightness");
        time += Time.deltaTime / duration;
        float value = Mathf.Lerp(startVal, endVal, time);
        mat.SetFloat("_Brightness", value);

        if (isBtnPressed) return;

        if (time >= 1.0f)
        {
            time = 0.0f;
            if (startVal == 0.5f)
            {
                startVal = 1.0f;
                endVal = 0.5f;
            }
            else
            {
                startVal = 0.5f;
                endVal = 1.0f;
            }
        }
    }

    public void InitForBtnPressed()
    {
        isBtnPressed = true;
        startVal = 1.0f;
        endVal = 0.5f;
        time = 0.0f;
        duration = 0.2f;
    }

}
