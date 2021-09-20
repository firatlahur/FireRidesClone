using UnityEngine;
using UnityEngine.Serialization;

namespace FireRidesClone.Wall
{
    public class WallMovement : MonoBehaviour
    {
        [FormerlySerializedAs("_speed")] [HideInInspector] public float speed;

        public GameObject wallTeleportation;

        private void Start()
        {
           speed = 5;
        }

        private void FixedUpdate()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, wallTeleportation.transform.position, Time.fixedDeltaTime * speed);
        }
    }
}
