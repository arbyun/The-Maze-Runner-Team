using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        float xOffset;

        [SerializeField]
        float yOffset;
        [SerializeField]
        protected Transform trackingTarget;
        [SerializeField]
        protected float followSpeed;
        [SerializeField]
        protected bool isXLocked = false;
        public Transform TrackingTarget
        {
            get
            {
                return trackingTarget;
            }
            set
            {
                trackingTarget = value;
            }
        }
        [SerializeField]
        protected bool isYLocked = false;
        protected void Update()
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
            }
            
            float targetSize = originalSize * zoomFactor;
            if (targetSize != thisCamera.orthographicSize)
            {
                thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, 
                    targetSize, Time.deltaTime * zoomSpeed);
            }

            var position = trackingTarget.position;
            float xTarget = position.x + xOffset;
            float yTarget = position.y + yOffset;
            
            float xNew = transform.position.x;
            if (!isXLocked)
            {
                xNew = Mathf.Lerp(transform.position.x, xTarget, Time.deltaTime * followSpeed);
            }

            float yNew = transform.position.y;
            if (!isYLocked)
            {
                yNew = Mathf.Lerp(transform.position.y, yTarget, Time.deltaTime * followSpeed);
            }

            var transform1 = transform;
            transform1.position = new Vector3(xNew, yNew, transform1.position.z);
            
        }
        
        [SerializeField]
        float zoomFactor = 1.0f;

        [SerializeField]
        float zoomSpeed = 5.0f;

        private float originalSize = 0f;

        private Camera thisCamera;

        void Start()
        {
            thisCamera = GetComponent<Camera>();
            originalSize = thisCamera.orthographicSize;
        }


        void setZoom(float zoomFactor)
        {
            this.zoomFactor = zoomFactor;
        }
        
        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        public Transform camTransform;
	
        // How long the object should shake for.
        public float shakeDuration = 0f;
	
        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;
	
        Vector3 originalPos;
	
        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = GetComponent(typeof(Transform)) as Transform;
            }
        }
	
        void OnEnable()
        {
            originalPos = camTransform.localPosition;
        }
        
        
    }
}