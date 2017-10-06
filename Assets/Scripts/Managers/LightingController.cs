using UnityEngine;
using System.Collections;

public enum ShaderType
{
    NO_GRADIENT,
    GREEN_GRADIENT,
    RED_GRADIENT,
}

public class LightingController : MonoBehaviour
{
    public MeshRenderer gradientRenderer;
    public Material[] gradientMaterials;

    public int DestShaderChangeCnt;
    public int CurShaderType;

    private int CurShaderChangeCnt;

    void Start()
    {
        CurShaderChangeCnt = 0;
    }

    void Update()
    {
        //Debug.Log("Hongyeol : " + LevelManager.Instance.GetBGMPlayHeadPosition());

        if (CurShaderChangeCnt >= DestShaderChangeCnt)
            return;

        //00:30에 그라디언트 교체(일반)
        if (CurShaderChangeCnt == 0 && LevelManager.Instance.GetBGMPlayHeadPosition()>30)
        {
            StartCoroutine(ChangeShaderCo(ShaderType.NO_GRADIENT));
            CurShaderChangeCnt++;
        }
        //01:20에 StartCoroutine그라디언트 교체
        else if (CurShaderChangeCnt == 1 && LevelManager.Instance.GetBGMPlayHeadPosition()>72) 
        {
            StartCoroutine(ChangeShaderCo(ShaderType.RED_GRADIENT));
            CurShaderChangeCnt++;
        }
    }

    IEnumerator ChangeShaderCo(ShaderType shaderType)
    {
        //바뀌어야 할 셰이더
        int destShaderType = (int)shaderType;

        bool isDone = false;
        float duration = 2f;
        float t = 0f;
        float r = gradientRenderer.material.color.r;
        float g = gradientRenderer.material.color.g;
        float b = gradientRenderer.material.color.b;
        Color fromColor = new Color(r,g,b,0.5f);
        Color toColor = new Color(r,g,b,0);

        while (CurShaderType != (int)ShaderType.NO_GRADIENT && !isDone)
        {
            gradientRenderer.material.color = Color.Lerp(fromColor, toColor, t);
            if(t>=1)
            {
                isDone = true;
            }
            t += Time.deltaTime / duration;
            yield return null;
        }

        gradientRenderer.material = gradientMaterials[destShaderType];
        CurShaderType = destShaderType;

        if (destShaderType == (int)ShaderType.NO_GRADIENT)
            yield break;

        isDone = false;
        t = 0;
        r = gradientRenderer.material.color.r;
        g = gradientRenderer.material.color.g;
        b = gradientRenderer.material.color.b;
        fromColor = new Color(r, g, b, 0f);
        toColor = new Color(r, g, b, 0.5f);

        while (!isDone)
        {
            gradientRenderer.material.color = Color.Lerp(fromColor, toColor, t);
            if (t >= 1)
            {
                isDone = true;
            }
            t += Time.deltaTime / duration;
            yield return null;
        }
    }
}
