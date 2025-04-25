using System.Collections.Generic;
using UnityEngine;

namespace Blitzball
{
    public class BlitzballTrailController : MonoBehaviour
    {
        [Header("Trail Settings")]
        public GameObject trailSpritePrefab;
        public int pointCount = 20;
        public float fadePower = 1.5f;
        public float timeStep = 0.05f; // Simulation step per point

        [Header("Pitch Reference")]
        public BlitzballController pitch;

        private List<GameObject> trailPoints = new();

        private void Start()
        {
            InitializePool();
        }

        private void Update()
        {
            if (!pitch.IsLaunched()) return;

            UpdateTrail();
        }

        private void InitializePool()
        {
            ClearTrail();

            for (int i = 0; i < pointCount; i++)
            {
                GameObject point = Instantiate(trailSpritePrefab, transform);
                point.SetActive(true);
                trailPoints.Add(point);
            }
        }

        private void UpdateTrail()
        {
            Vector3 velocity = pitch.GetVelocity();
            Vector3 position = pitch.GetPosition();
            Vector3 spinAxis = pitch.GetSpinAxis();

            for (int i = 0; i < trailPoints.Count; i++)
            {
                float t = i * timeStep;

                Vector3 simulatedPos = position + velocity * t;

                // Apply approximate curve offset from Magnus force over time
                Vector3 magnusDir = Vector3.Cross(spinAxis, velocity).normalized;
                Vector3 curveOffset = 0.5f * magnusDir * pitch.curveAmount * pitch.curveForce * (t * t);

                GameObject point = trailPoints[i];
                point.transform.position = simulatedPos + curveOffset;

                float alpha = Mathf.Pow(1f - (i / (float)(trailPoints.Count - 1)), fadePower);
                SetAlpha(point, alpha);
            }
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

        public void SetTrailActive(bool active)
        {
            foreach (GameObject point in trailPoints)
                point.SetActive(active);
        }

        public void ClearTrail()
        {
            foreach (var point in trailPoints)
                Destroy(point);
            trailPoints.Clear();
        }
    }
}
