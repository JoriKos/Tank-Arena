using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class FireBullet : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private ObjectPooling _pool;
    [SerializeField] private GameObject _target, _bulletOrigin;
    [SerializeField] private PlayerBase _player;

    // Controls
    private PlayerControls _pControls;
    private PlayerInput _input;

    // These inputs are a shortcut for "PlayerControls.ActionMap.InputAction"
    private InputAction _fireAction;

    //Start() function for networking
    public override void OnNetworkSpawn()
    {
        if(!_pool)
            _pool = transform.parent.GetComponent<ObjectPooling>();

        _input = _player.Input;
        _fireAction = _input.actions["Fire"];
    }

    private void Update()
    {
        //If I am the owner, continue. Prevents other clients from controlling you
        if (!IsOwner)
            return;

        //transform.position += -transform.forward * Time.deltaTime * _speed;

        if (_fireAction.IsPressed())
        {
            Debug.Log("Fire!");
            Fire();
        }
    }

    private void OnEnable()
    {
        //Checks if null
        //Should probably find a better way to implement this...
        _pControls ??= new PlayerControls();

        _pControls.TankControls.Enable();
    }

    private void Fire()
    {
        if (_target)
        {
            if (_target.activeInHierarchy)
                transform.position = Vector3.MoveTowards(_bulletOrigin.transform.position, _target.transform.position, _speed * Time.deltaTime);
            else
                _pool.ReturnObject(gameObject);
        }
        else
            _pool.ReturnObject(gameObject);
    }
}