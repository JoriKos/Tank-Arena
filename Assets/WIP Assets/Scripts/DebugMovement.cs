using Unity.Netcode;
using UnityEngine;

public class DebugMovement : NetworkBehaviour
{
    [SerializeField] private float _speed;

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.forward);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(_speed * Time.deltaTime * -Vector3.forward);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(_speed * Time.deltaTime * -Vector3.right);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.right);
        }
    }
}
