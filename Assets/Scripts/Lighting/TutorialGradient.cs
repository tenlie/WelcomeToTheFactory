using UnityEngine;
using System.Collections;

public class TutorialGradient : MonoBehaviour
{
    private MeshRenderer gradientRenderer;
    private Color alphaZero;
    private Color alphaOne;
    public float tweenDuration;
    public float tweenCount;
    private bool hasStarted;

    void Start()
    {
        hasStarted = false;
        gradientRenderer = GetComponent<MeshRenderer>();
        alphaZero = new Color(gradientRenderer.material.color.r, gradientRenderer.material.color.g, gradientRenderer.material.color.b, 0f);
        alphaOne = new Color(gradientRenderer.material.color.r, gradientRenderer.material.color.g, gradientRenderer.material.color.b, 1f);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        /*
        if (!hasStarted)
        {
            hasStarted = true;
            return;
        }
        */

        //FlickerCo();
    }

    public void Flip()
    {
        float value = transform.localPosition.x;
        transform.localPosition = new Vector3(-value, transform.localPosition.y, transform.localPosition.z);
    }

    public void Flicker()
    {
        StartCoroutine(FlickerCo());
    }

    IEnumerator FlickerCo()
    {
        Debug.Log("Hongyeol3");
        float t = 0f;
        float count = 0f;
        Color fromColor = alphaZero;
        Color toColor = alphaOne;

        while (count < tweenCount)
        {
            gradientRenderer.material.color = Color.Lerp(fromColor, toColor, t);

            if (t > 1)
            {
                t = 0f;
                count++;

                if (count % 2 == 0)
                {
                    fromColor = alphaZero;
                    toColor = alphaOne;
                }
                else
                {
                    fromColor = alphaOne;
                    toColor = alphaZero;
                }
            }
            else
            {
                t += Time.deltaTime / tweenDuration;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
