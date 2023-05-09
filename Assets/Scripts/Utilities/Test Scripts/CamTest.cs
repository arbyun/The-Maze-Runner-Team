using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Utilities.Test_Scripts
{
    //[RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CamTest : MonoBehaviour
    {
        public bool zoomActive;
        public Vector3[] target;
        public GameObject zoomIn;
        public Camera cam;
        public float speed;
        public GameObject player;
        public CinemachineBrain cinemachineBrain;

        private void Start()
        {
            cam = Camera.main;
            cinemachineBrain = cam.GetComponent<CinemachineBrain>();
        }

        public void LateUpdate()
        {
            cinemachineBrain.enabled = !zoomActive;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomActive ? 3 : 5, speed);
            //cam.transform.position = Vector3.Lerp(cam.transform.position, zoomActive ? target[0] : player.transform.position, speed);

            if (zoomActive)
                cam.transform.position = Vector3.Lerp(cam.transform.position, target[0], speed);
        }


        /*[Range(0, 10)]
        public float zoomMultiplier = 1f;
        [Range(0, 100)]
        public float minZoom = 1f;
        [Range(0, 100)]
        public float maxZoom = 50f;
        [Range(0, 10)]
        public float zoomTime = 1f;

        public Collider2D door;
        public Collider2D player;
        public float zoomModifier;

        private RoughDoor _rdscript;

        CinemachineVirtualCamera _mVirtualCamera;
        float _mOriginalOrthoSize;
        private float _ogX;
        private float _ogY;
        Coroutine _mZoomCoroutine;
        private CinemachineCameraOffset _fuckingAnnoyingOffset;

        void Awake()
        {
            _mVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _mOriginalOrthoSize = _mVirtualCamera.m_Lens.OrthographicSize;
            _fuckingAnnoyingOffset = GetComponent<CinemachineCameraOffset>();
            _ogX = _fuckingAnnoyingOffset.m_Offset.x;
            _ogY = _fuckingAnnoyingOffset.m_Offset.y;
            _rdscript = door.gameObject.GetComponent<RoughDoor>();
        }

        private void Update()
        {
            if (door.IsTouching(player) | player.IsTouching(door))
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    OnTrigger();
                }
            }
            else
            {
                OnExitTrigger();
            }
        }

        void restoreOriginalOrthographicSize()
        {
            if (_mZoomCoroutine != null)
            {
                StopCoroutine(_mZoomCoroutine);
                _mZoomCoroutine = null;
            }
            
            _mVirtualCamera.m_Lens.OrthographicSize = _mOriginalOrthoSize;
            _fuckingAnnoyingOffset.m_Offset.x = _ogX;
            _fuckingAnnoyingOffset.m_Offset.y = _ogY;
        }
        
        void OnValidate()
        {
            maxZoom = Mathf.Max(minZoom, maxZoom);
        }

        IEnumerator zoomCoroutine(float targetSize)
        {
            _fuckingAnnoyingOffset.m_Offset.x = 0;
            _fuckingAnnoyingOffset.m_Offset.y = 1.7f;
            
            float t = 0;
            float startSize = _mVirtualCamera.m_Lens.OrthographicSize;
            while (t < zoomTime)
            {
                t += Time.deltaTime;
                float size = Mathf.Lerp(startSize, targetSize, t / zoomTime);
                _mVirtualCamera.m_Lens.OrthographicSize = size;
                yield return null;
            }
            _mVirtualCamera.m_Lens.OrthographicSize = targetSize;
        }

        private void OnTrigger()
        {
            float zoom = _mVirtualCamera.m_Lens.OrthographicSize + zoomModifier * zoomMultiplier;
            float targetZoom = _mVirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
            if (_mZoomCoroutine != null)
            {
                StopCoroutine(_mZoomCoroutine);
            }
            _mZoomCoroutine = StartCoroutine(zoomCoroutine(targetZoom));
        }

        private void OnExitTrigger()
        {
            restoreOriginalOrthographicSize();
        }*/
    }
}