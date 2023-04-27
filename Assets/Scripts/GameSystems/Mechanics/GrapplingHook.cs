using UnityEngine;

namespace GameSystems.Mechanics
{
    public class GrapplingHook : MonoBehaviour
    {
        public GrapplingRope grappleRope;
        [SerializeField] private bool grappleToAll = false;
        [SerializeField] private int grappableLayerNumber = 9;
        public UnityEngine.Camera mCamera;

        public Transform gunHolder;
        public Transform gunPivot;
        public Transform firePoint;

        public SpringJoint2D mSpringJoint2D;
        public Rigidbody2D mRigidbody;

        [SerializeField] private bool rotateOverTime = true;
        [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

        [SerializeField] private bool hasMaxDistance = false;
        [SerializeField] private float maxDistance = 20;

        private enum LaunchType
        {
            TransformLaunch,
            PhysicsLaunch
        }

        [SerializeField] private bool launchToPoint = true;
        [SerializeField] private LaunchType launchType = LaunchType.PhysicsLaunch;
        [SerializeField] private float launchSpeed = 1;

        [SerializeField] private bool autoConfigureDistance = false;
        [SerializeField] private float targetDistance = 3;
        [SerializeField] private float targetFrequency = 1;

        [HideInInspector] public Vector2 grapplePoint;
        [HideInInspector] public Vector2 grappleDistanceVector;

        private void Start()
        {
            grappleRope.enabled = false;
            mSpringJoint2D.enabled = false;
        }

        /// <summary>
        /// Checks for user input from the mouse and updates the game accordingly. Here's the step by step:
        /// 1. Checks if the left mouse button is pressed down with Input.GetKeyDown(KeyCode.Mouse0).
        /// 2. If the left mouse button is pressed down, it calls the SetGrapplePoint method.
        /// 3. If the left mouse button is held down with Input.GetKey(KeyCode.Mouse0), it does the following:
        ///     a. If the grappleRope object is enabled, it calls the RotateGun method with the grapplePoint and false
        ///        parameters.
        ///     b. If the grappleRope object is not enabled, it gets the mouse position and calls the RotateGun method
        ///        with the mouse position and true parameter.
        ///     c. If launchToPoint is true and the grappleRope object is grappling, it calculates the target position
        ///        for the gunHolder object and moves it towards that position using Vector2.Lerp and Time.deltaTime.
        /// 4. If the left mouse button is released with Input.GetKeyUp(KeyCode.Mouse0), it disables the grappleRope
        /// object, mSpringJoint2D object, and sets the Rigidbody.gravityScale to 1.
        /// 5. If no mouse input is detected, it gets the mouse position and calls the RotateGun method with the mouse
        /// position and true parameter.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SetGrapplePoint();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                if (grappleRope.enabled)
                {
                    RotateGun(grapplePoint, false);
                }
                else
                {
                    Vector2 mousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
                    RotateGun(mousePos, true);
                }

                if (launchToPoint && grappleRope.isGrappling)
                {
                    if (launchType == LaunchType.TransformLaunch)
                    {
                        Vector2 firePointDistance = firePoint.position - gunHolder.localPosition;
                        Vector2 targetPos = grapplePoint - firePointDistance;
                        gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, 
                            Time.deltaTime * launchSpeed);
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                grappleRope.enabled = false;
                mSpringJoint2D.enabled = false;
                mRigidbody.gravityScale = 1;
            }
            else
            {
                Vector2 mousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }
        }

        /// <summary> The RotateGun function rotates the gunPivot object to face a given point in space.</summary>
        /// <param name="lookPoint"> The point in space that the gun will look at. </param>
        /// <param name="allowRotationOverTime"> If we should allow the gun to rotate over time. </param>
        /// <returns> The angle of the gunpivot in relation to the lookpoint, which is where you want it to be
        /// looking.</returns>
        void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
        {
            Vector3 distanceVector = lookPoint - gunPivot.position;

            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            if (rotateOverTime && allowRotationOverTime)
            {
                gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 
                    Time.deltaTime * rotationSpeed);
            }
            else
            {
                gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        /// <summary> The SetGrapplePoint function is called when the player clicks the mouse button.        
        /// It creates a raycast from the firePoint to where they clicked, and if it hits something in
        /// the grappable layer, it sets that point as grapplePoint.</summary>
        /// <returns> The grapplepoint vector2.</returns>
        void SetGrapplePoint()
        {
            Vector2 distanceVector = mCamera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
            if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
            {
                RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
                if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
                {
                    if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                    {
                        grapplePoint = _hit.point;
                        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                        grappleRope.enabled = true;
                    }
                }
            }
        }

        /// <summary> The Grapple function is called when the player presses the grapple button.        
        /// It sets up a SpringJoint2D component to connect the player's rigidbody to a point in space,
        /// and then launches them towards that point.</summary>
        ///
        ///
        /// <returns> Nothing.</returns>
        public void Grapple()
        {
            mSpringJoint2D.autoConfigureDistance = false;
            if (!launchToPoint && !autoConfigureDistance)
            {
                mSpringJoint2D.distance = targetDistance;
                mSpringJoint2D.frequency = targetFrequency;
            }
            if (!launchToPoint)
            {
                if (autoConfigureDistance)
                {
                    mSpringJoint2D.autoConfigureDistance = true;
                    mSpringJoint2D.frequency = 0;
                }

                mSpringJoint2D.connectedAnchor = grapplePoint;
                mSpringJoint2D.enabled = true;
            }
            else
            {
                switch (launchType)
                {
                    case LaunchType.PhysicsLaunch:
                        mSpringJoint2D.connectedAnchor = grapplePoint;

                        Vector2 distanceVector = firePoint.position - gunHolder.position;

                        mSpringJoint2D.distance = distanceVector.magnitude;
                        mSpringJoint2D.frequency = launchSpeed;
                        mSpringJoint2D.enabled = true;
                        break;
                    case LaunchType.TransformLaunch:
                        mRigidbody.gravityScale = 0;
                        mRigidbody.velocity = Vector2.zero;
                        break;
                }
            }
        }

        /// <summary> The OnDrawGizmosSelected function is called when the object is selected in the editor.</summary>        
        ///
        ///
        /// <returns> A void.</returns>
        private void OnDrawGizmosSelected()
        {
            if (firePoint != null && hasMaxDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(firePoint.position, maxDistance);
            }
        }

    }
}
