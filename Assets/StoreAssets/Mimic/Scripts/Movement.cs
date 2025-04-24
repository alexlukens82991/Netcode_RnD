using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent m_NavMeshAgent;

        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 0.8f;
        public float speed = 5f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;

        private Transform m_CurrentTarget;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();

            StartCoroutine(FindTargetRoutine());
        }

        void Update()
        {
            //velocity = Vector3.Lerp(velocity, new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed, velocityLerpCoef * Time.deltaTime);

            //// Assigning velocity to the mimic to assure great leg placement
            //myMimic.velocity = velocity;

            //transform.position = transform.position + velocity * Time.deltaTime;
            //RaycastHit hit;
            //Vector3 destHeight = transform.position;
            //if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit))
            //    destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);


            myMimic.velocity = m_NavMeshAgent.velocity;
        }

        // needs refactor
        private IEnumerator FindTargetRoutine()
        {
            yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClients.Count > 0);

            m_CurrentTarget = PlayerTracker.Instance.GetClosestPlayer(transform.position);

            do
            {
                m_NavMeshAgent.SetDestination(m_CurrentTarget.position);
                yield return new WaitForSeconds(1f);
            } while (m_CurrentTarget != null);
        }
    }

}