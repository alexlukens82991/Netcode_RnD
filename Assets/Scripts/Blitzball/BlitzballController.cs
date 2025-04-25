using UnityEngine;

namespace Blitzball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BlitzballController : MonoBehaviour
    {
        [Header("Pitch Settings")]
        public float pitchSpeed = 80f;
        public float curveForce = 10f;

        [Header("Curve Settings")]
        public Vector3 curveDirection = Vector3.right;
        public float curveAmount = 1f;

        [Header("Spin")]
        public float spinVisualSpeed = 300f;

        [Header("References")]
        public Transform aimTarget;
        public MeshRenderer m_AimTargetRenderer;
        public BlitzballPitchPredictor m_BlitzballPitchPredictor;

        private Rigidbody rb;
        private Vector3 spinAxis;
        private Vector3 startPosition;
        private Quaternion startRotation;
        private bool hasLaunched = false;
        private bool pitchEnded = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = true;

            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        private void Update()
        {
            if (!hasLaunched && !pitchEnded && aimTarget != null)
            {
                transform.forward = aimTarget.forward;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !hasLaunched && !pitchEnded)
            {
                Launch();
            }

            if (Input.GetKeyDown(KeyCode.R) && pitchEnded)
            {
                ResetPitch();
            }
        }

        public void Launch()
        {
            hasLaunched = true;
            rb.isKinematic = false;
            rb.linearVelocity = aimTarget.forward.normalized * pitchSpeed;

            spinAxis = Vector3.Cross(rb.linearVelocity.normalized, curveDirection.normalized).normalized;

            m_AimTargetRenderer.enabled = false;
            m_BlitzballPitchPredictor.SetTrailActive(false);
        }

        private void FixedUpdate()
        {
            if (!hasLaunched) return;

            // Apply Magnus effect
            Vector3 magnusForce = Vector3.Cross(spinAxis, rb.linearVelocity).normalized * curveAmount * curveForce;
            rb.AddForce(magnusForce, ForceMode.Force);

            // Visual spin
            transform.Rotate(spinAxis, spinVisualSpeed * Time.fixedDeltaTime, Space.World);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasLaunched) return;

            // Let physics handle the rest of the interaction
            hasLaunched = false;
            pitchEnded = true;

            // Optional: trigger a hit effect, sound, etc.
        }

        private void ResetPitch()
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position = startPosition;
            transform.rotation = startRotation;

            hasLaunched = false;
            pitchEnded = false;

            m_AimTargetRenderer.enabled = true;
            m_BlitzballPitchPredictor.SetTrailActive(true);
        }

        public Vector3 CalculateMagnusForce(Vector3 velocity, Vector3 spinAxis)
        {
            return Vector3.Cross(spinAxis, velocity).normalized * curveAmount * curveForce;
        }

        public Vector3 CalculateSpinAxis(Vector3 velocity)
        {
            return Vector3.Cross(velocity.normalized, curveDirection.normalized).normalized;
        }


        public Vector3 GetSpinAxis() => spinAxis;
        public Vector3 GetVelocity() => rb.linearVelocity;
        public Vector3 GetPosition() => transform.position;
        public bool IsLaunched() => hasLaunched;
        public Rigidbody GetRigidbody() => rb;
    }
}
