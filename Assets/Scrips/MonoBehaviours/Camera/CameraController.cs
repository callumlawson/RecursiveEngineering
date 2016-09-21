using UnityEngine;

[RequireComponent(typeof (Camera))]
public class CameraController : MonoBehaviour {

    public float ScrollSensitivity;
    public float PanSpeed;
    public float MaxPanSpeed;

    public static Camera ActiveCamera;
    private Vector3 MouseOrigin;

	// Use this for initialization
	void Start () {
        ActiveCamera = GetComponent<Camera>();
        MouseOrigin = ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
	}
	
	// Update is called once per frame
	void Update () {

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        ActiveCamera.orthographicSize -= scroll * ScrollSensitivity * Time.deltaTime;
        if(ActiveCamera.orthographicSize < 0)
        {
            ActiveCamera.orthographicSize = 0;
        }

	    if (Input.GetMouseButtonDown(2))
	    {
            MouseOrigin = Input.mousePosition;
        }

	    if (Input.GetMouseButton(2))
        {
            var mouseDelta = Camera.main.ScreenToViewportPoint(Input.mousePosition - MouseOrigin);
            var move = - Vector3.ClampMagnitude(mouseDelta * PanSpeed * Time.deltaTime, MaxPanSpeed);
            transform.Translate(move, Space.World);
        }
    }
}
