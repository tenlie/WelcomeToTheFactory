using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Vector3 StartCameraPosition;
    private Vector3
        _min,
        _max;

    public Transform CameraFlag;
	public Vector2 Margin;
	public Vector2 Smoothing;
	public BoxCollider2D Bounds;

	public bool IsFollowing { get; set; }

	public void Start()
	{
        StartCameraPosition = transform.position;
        _min = Bounds.bounds.min;
		_max = Bounds.bounds.max;
		IsFollowing = true;
	}

	public void Update()
	{
        var x = transform.position.x;
		var y = transform.position.y;

        if (IsFollowing)
        {
            if (Mathf.Abs(x - CameraFlag.position.x) > Margin.x)
                x = Mathf.Lerp(x, CameraFlag.position.x, Smoothing.x * Time.deltaTime);

            if (Mathf.Abs(y - CameraFlag.position.y) > Margin.y)
                y = Mathf.Lerp(y, CameraFlag.position.y, Smoothing.y * Time.deltaTime);
        }

            var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);

            x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
            y = Mathf.Clamp(y, _min.y + GetComponent<Camera>().orthographicSize, _max.y - GetComponent<Camera>().orthographicSize);

            transform.position = new Vector3(x, y, transform.position.z);
	}

    public void ReturnToStartCameraPosition() {
        transform.position = StartCameraPosition;
    }
}
