using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BulletBehaviour : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;

    //Networking Start()
    public override void OnNetworkSpawn()
    {
        //Ideally shouldn't ever be executed
        if (_rb)
            _rb.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //_rb.AddRelativeForce(transform.forward * _speed);

        //Doesn't have the forward local position

        //Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
        //transform.position = Vector3.MoveTowards(transform.position, localForward, _speed * Time.deltaTime);

        //transform.Translate(_speed * Time.deltaTime * transform.forward);

        transform.position += _speed * Time.deltaTime * transform.forward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.worldToLocalMatrix.MultiplyVector(transform.forward));
    }
}
