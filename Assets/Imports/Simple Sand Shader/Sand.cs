using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vEKUGames.SimpleSand
{
    public class Sand : MonoBehaviour
    {
        [SerializeField]
        private TrackedEntity m_trackedEntity;
        [SerializeField]
        private Renderer m_renderer;

        private int m_groundLayer;

        private void Awake()
        {
            m_groundLayer = LayerMask.GetMask("UndergroundDetection");
        }

        private void Reset()
        {
            TryGetComponent(out m_renderer);
        }

        private void Update()
        {
            UpdateEntityPosition(m_trackedEntity.Position);
            UpdateEntityVelocity(m_trackedEntity.Velocity);
            UpdateEntityRadius(m_trackedEntity.Radius);
            UpdateEntityPercentUnderground();
        }

        private void UpdateEntityVelocity(Vector3 velocity)
        {
            Vector4 entityVelocity = new Vector4(velocity.x, velocity.y, velocity.z, 0);
            m_renderer.sharedMaterial.SetVector("_TrackedEntityVelocity", entityVelocity);
        }

        private void UpdateEntityPosition(Vector3 position)
        {
            Vector4 entityPosition = new Vector4(position.x, position.y, position.z, 0);
            m_renderer.sharedMaterial.SetVector("_TrackedEntityPosition", entityPosition);
        }

        private void UpdateEntityRadius(float radius)
        {
            m_renderer.sharedMaterial.SetFloat("_TrackedEntityRadius", radius);
        }

        private void UpdateEntityPercentUnderground()
        {
            float percentUnderground = 0f;

            Ray ray = new Ray(m_trackedEntity.Position + (Vector3.up * m_trackedEntity.Radius), Vector3.down);
            RaycastHit raycastHit;

            //Debug.DrawRay(m_trackedEntity.Position + (Vector3.up * m_trackedEntity.Radius), (Vector3.down * m_trackedEntity.Radius * 2));

            if (Physics.Raycast(ray, out raycastHit, m_trackedEntity.Radius * 3, m_groundLayer))
            {
                float distance = Vector3.Distance(m_trackedEntity.Position, raycastHit.point) ;
                percentUnderground = Mathf.InverseLerp(m_trackedEntity.Radius, 0, distance);
            }
            m_renderer.sharedMaterial.SetFloat("_TrackedEntityPercentUndergroud", percentUnderground);
        }
    }
}
