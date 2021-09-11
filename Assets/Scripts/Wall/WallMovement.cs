using UnityEngine;

public class WallMovement : MonoBehaviour
{
    [HideInInspector] public float _speed;

    public GameObject wallTeleportation;

    void Start()
    {
        _speed = 5;
    }

    void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, wallTeleportation.transform.position, Time.fixedDeltaTime * _speed);
    }
}
