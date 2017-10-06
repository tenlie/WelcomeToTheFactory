using UnityEngine;
using System.Collections;

public class MissileAfterImageEffect : MonoBehaviour {

    public SpriteRenderer spriteSrc;
    public bool afterImageEnabled;

    void Start()
    {
        afterImageEnabled = false;
        //StartCoroutine("AfterImageUpdate");
    }

    IEnumerator AfterImageUpdate()
    {
        while (true)
        {
            while (afterImageEnabled)
            {
                //Debug.Log("AfterImage");
                SpriteRenderer spriteCopy = Instantiate(spriteSrc) as SpriteRenderer;
                spriteCopy.transform.position = spriteSrc.transform.position;
                spriteCopy.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                spriteCopy.sortingLayerName = "Foreground";
                spriteCopy.sortingOrder = 1;
                GameObject newGO = new GameObject();
                newGO.transform.position = spriteSrc.transform.position;
                newGO.AddComponent<SpriteRenderer>();
                SpriteRenderer spriteRenderer = newGO.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = spriteCopy.sprite;
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                spriteRenderer.transform.rotation = spriteCopy.transform.rotation;
                spriteRenderer.sortingLayerName = "Foreground";
                spriteRenderer.sortingOrder = 1;
                Destroy(spriteCopy.gameObject);
                Destroy(newGO, 0.3f);
                yield return new WaitForSeconds(0.06f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
