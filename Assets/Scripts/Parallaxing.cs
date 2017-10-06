using UnityEngine;

public class Parallaxing : MonoBehaviour 
{

	public Transform[] backgrounds;
	private float[] parallaxScales;
	public float smoothing;

	private Vector3 previousCamaraPosition;

	// Use this for initialization
	void Start () {
        //시작할 때의 카메라의 tranform.position
        previousCamaraPosition = transform.position; 
	
		parallaxScales = new float[backgrounds.Length];
		for(int i = 0; i < parallaxScales.Length; i++)
		{
			parallaxScales[i] = backgrounds[i].position.z * (-1);
		}
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		for(int i = 0; i < backgrounds.Length; i++)
		{
            //smoothing의 값이 클수록 배경이 움직이는 속도가 감소한다.
			Vector3 parallax = (previousCamaraPosition - transform.position) * (parallaxScales[i] / smoothing);
			backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallax.x, backgrounds[i].position.y + parallax.y, backgrounds[i].position.z);
		}

		previousCamaraPosition = transform.position;
	}
}
