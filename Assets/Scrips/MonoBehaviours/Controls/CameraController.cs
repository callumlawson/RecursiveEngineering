using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Controls
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour {

        [UsedImplicitly] public float ScrollSensitivity;
        [UsedImplicitly] public float PanSpeed;
        [UsedImplicitly] public float MaxPanSpeed;

        public static Camera ActiveCamera;
        public static CameraController Instance;

        private Vector3 mouseOrigin;

        public void SetPosition(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        [UsedImplicitly]
        void Start ()
        {
            Instance = this;
            ActiveCamera = GetComponent<Camera>();
            mouseOrigin = ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
        }
	
        [UsedImplicitly]
        void Update () {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            ActiveCamera.orthographicSize -= scroll * ScrollSensitivity * Time.deltaTime;
            if(ActiveCamera.orthographicSize < 0)
            {
                ActiveCamera.orthographicSize = 0;
            }

            if (Input.GetMouseButtonDown(2))
            {
                mouseOrigin = Input.mousePosition;
            }

            if (Input.GetMouseButton(2))
            {
                var mouseDelta = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
                var move = - Vector3.ClampMagnitude(mouseDelta * PanSpeed * Time.deltaTime, MaxPanSpeed);
                transform.Translate(move, Space.World);
            }
        }
    }
}
