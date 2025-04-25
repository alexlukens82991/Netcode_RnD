using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blitzball
{
    public class BlitzballPitchPredictor : MonoBehaviour
    {
        [Header("Prediction Settings")]
        public GameObject trailPointPrefab;
        public int predictionSteps = 50;
        public float timeStep = 0.02f;

        [Header("References")]
        public BlitzballController liveBall;
        public GameObject ballGhostPrefab;

        private List<GameObject> trailPoints = new();
        private PhysicsScene predictionScene;
        private Scene simScene;
        private GameObject ghostBall;
        private Rigidbody ghostRb;

        private void Start()
        {
            CreatePhysicsScene();

            // Sync physics settings
            Rigidbody liveRb = liveBall.GetRigidbody();
            ghostRb.mass = liveRb.mass;
            ghostRb.useGravity = true;
            ghostRb.linearDamping = liveRb.linearDamping;
            ghostRb.angularDamping = liveRb.angularDamping;

            InitializeTrailPoints();
        }

        private void Update()
        {
            if (!liveBall.IsLaunched())
            {
                SimulatePrediction();
            }
        }

        private void CreatePhysicsScene()
        {
            var parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            simScene = SceneManager.CreateScene("PredictionScene", parameters);
            predictionScene = simScene.GetPhysicsScene();

            ghostBall = Instantiate(ballGhostPrefab);
            SceneManager.MoveGameObjectToScene(ghostBall, simScene);
            ghostBall.SetActive(true);
            ghostRb = ghostBall.GetComponent<Rigidbody>();
        }

        private void InitializeTrailPoints()
        {
            for (int i = 0; i < predictionSteps; i++)
            {
                GameObject marker = Instantiate(trailPointPrefab, transform);
                marker.SetActive(true);
                trailPoints.Add(marker);
            }
        }

        private void SimulatePrediction()
        {
            // Reset ghost state
            ghostBall.transform.position = liveBall.GetPosition();
            ghostBall.transform.rotation = liveBall.transform.rotation;
            ghostRb.linearVelocity = liveBall.aimTarget.forward.normalized * liveBall.pitchSpeed;
            ghostRb.angularVelocity = Vector3.zero;

            // Compute spin axis using live logic
            Vector3 spinAxis = liveBall.CalculateSpinAxis(ghostRb.linearVelocity);

            // Simulate ahead
            for (int i = 0; i < predictionSteps; i++)
            {
                // Apply the exact same Magnus logic as the live ball
                Vector3 magnusForce = liveBall.CalculateMagnusForce(ghostRb.linearVelocity, spinAxis);
                ghostRb.AddForce(magnusForce, ForceMode.Force);

                // Apply gravity manually (ensures consistent result)
               // ghostRb.AddForce(Physics.gravity, ForceMode.Acceleration);

                predictionScene.Simulate(timeStep);

                trailPoints[i].transform.position = ghostBall.transform.position;

                float alpha = Mathf.Pow(1f - (i / (float)(predictionSteps - 1)), 1.5f);
                SetAlpha(trailPoints[i], alpha);
            }
        }

        public void SetTrailActive(bool active)
        {
            foreach (GameObject point in trailPoints)
                point.SetActive(active);
        }

        private void SetAlpha(GameObject obj, float alpha)
        {
            if (obj.TryGetComponent(out SpriteRenderer sr))
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
        }
    }
}
