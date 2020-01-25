using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underxel
{
    public class Foot : MonoBehaviour
    {
        private PlayerController player = null;

        private void Start()
        {
            player = FindObjectOfType<PlayerController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                player.PlayFootStepAudio();
            }
        }
    }
}
