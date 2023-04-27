using Player;
using UnityEngine;
using Dialogue;

namespace Camera
{
    /* Zoom in the characters when dialogue is happening */
    public class DialogCameraController : MonoBehaviour
    {
        [SerializeField] private float zoomLens = 7;
        [SerializeField] private float zoomDuration = 2f;
        [SerializeField] private float screenY = -.15f;
        
        private UnityEngine.Camera _mainCamera;
        
        private float _startLens;
        private float _targetLens;
        
        private float _startScreenY;
        private float _targetScreenY;
        
        private float _startScreenX;
        private float _targetScreenX;

        private Transform _playerTransform;

        /// <summary> Used to initialize any variables or game state before the game starts.</summary>
        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
            _playerTransform = FindObjectOfType<PlayerController>().transform;
            _startLens = _mainCamera.orthographicSize;
            _targetLens = _startLens;

            var position = _mainCamera.transform.position;
            _startScreenX = position.x;
            _targetScreenX = _startScreenX;
            
            _startScreenY = position.y;
            _targetScreenY = _startScreenY;
        }
        
        private void Start()
        {
            DialogueManager.Instance.OnDialogStart += HandleDialogStart;
            DialogueManager.Instance.OnDialogEnds += HandleDialogEnd;
            DialogueManager.Instance.OnDialogCancelled += HandleDialogEnd;
        }

        private void OnDisable()
        {
            DialogueManager.Instance.OnDialogStart -= HandleDialogStart;
            DialogueManager.Instance.OnDialogEnds -= HandleDialogEnd;
            DialogueManager.Instance.OnDialogCancelled -= HandleDialogEnd;
        }

        /// <summary> This section is responsible for smoothly zooming and moving the camera towards a target
        /// position and lens size. The `orthographicSize` property of the camera is set using the `Mathf.Lerp`
        /// function, which interpolates between the current size and the target size over time. The `Time.deltaTime`
        /// parameter ensures that the interpolation occurs at a consistent rate regardless of the frame rate.
        /// The camera's position is also interpolated using `Mathf.Lerp`. The `y` component of the position is set to
        /// the target screen height, while the `x` component is set to the target screen width. The `Time.deltaTime`
        /// parameter is used again to ensure that the interpolation occurs at a consistent rate. Finally, the new
        /// camera position is set back to the camera's transform.</summary>
        private void Update()
        {
            _mainCamera.orthographicSize = Mathf.Lerp(
                _mainCamera.orthographicSize, 
                _targetLens,
                zoomDuration * Time.deltaTime);

            var cameraPosition = _mainCamera.transform.position;
            cameraPosition.y = Mathf.Lerp(
                cameraPosition.y, 
                _targetScreenY, 
                zoomDuration * Time.deltaTime);
            
            cameraPosition.x = Mathf.Lerp(
                cameraPosition.x, 
                _targetScreenX, 
                zoomDuration * Time.deltaTime);

            _mainCamera.transform.position = cameraPosition;
        }

        /// <summary> The HandleDialogStart function is called when the player enters an interaction with an NPC.        
        /// It sets the target lens to zoomLens, and then it sets _targetScreenY to be equal to the player's y position
        /// plus screenY.
        /// Finally, it sets _targetScreenX equal to the player's x position.</summary>
        /// <returns> A float value.</returns>
        private void HandleDialogStart()
        {
            _targetLens = zoomLens;
            var position = _playerTransform.position;
            _targetScreenY = position.y + screenY;
            _targetScreenX = position.x;
        }

        /// <summary> The HandleDialogEnd function is called when the dialog ends. It resets the camera to its original
        /// position.</summary>
        /// <returns> A boolean value.</returns>
        private void HandleDialogEnd()
        {
            _targetLens = _startLens;
            _targetScreenY = _startScreenY;
            _targetScreenX = _startScreenX;
        }
    }
}