using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Vector2 velocity;

	public float smoothTimeX;
	public float smoothTimeY;

	public GameObject player;

	public bool bounds;

	public Vector3 minCameraPos;
	public Vector3 maxCameraPos;

	public BoxCollider2D cameraBounds;



	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		SetMinCamPosition();
		SetMaxCamPosition();
	}

	void FixedUpdate(){
		float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.x, smoothTimeY);
	
		transform.position = new Vector3 (posX, posY, transform.position.z);

		if (bounds) {
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
			                                 Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
			                                 Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
		}
	}

	public void SetMinCamPosition(){
		minCameraPos = cameraBounds.bounds.min; 
	}

	public void SetMaxCamPosition(){
		maxCameraPos = cameraBounds.bounds.max;
	}
	
}
