using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vEKUGames.SimpleSand
{
    public class Movement : MonoBehaviour
    {
        private const float FORCE_MOVEMENT_MULTIPLIER = 100f;
        private const float FORCE_JUMP_MULTIPLIER = 100f;

        [SerializeField]
        private Camera m_camera;
        [SerializeField]
        private Rigidbody m_rigidbody;
        [SerializeField]
        private float m_movementSpeed = 2;
        [SerializeField]
        private float m_jumpSpeed = 2;

        private void Reset()
        {
            m_camera = Camera.main;
            TryGetComponent(out m_rigidbody);
        }

        private void Update()
        {
            Vector3 force = new Vector3(0, 0, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                force = m_jumpSpeed * Vector3.up * FORCE_JUMP_MULTIPLIER;
            }

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            movement = m_camera.transform.TransformDirection(movement);

            force += movement * m_movementSpeed * FORCE_MOVEMENT_MULTIPLIER * Time.deltaTime;

            m_rigidbody.AddForce(force);
        }
    }
}
