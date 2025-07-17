using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class FireBullet : NetworkBehaviour
{
    [SerializeField] private float _damage, _fireSpeed;
    [SerializeField] private ObjectPooling _pool;
    [SerializeField] private GameObject _bulletOrigin;
    [SerializeField] private PlayerBase _player;

    private bool _hasFired;
    private float _fireTimer;

    // Controls
    private PlayerControls _pControls;
    private PlayerInput _input;

    // These inputs are a shortcut for "PlayerControls.ActionMap.InputAction"
    private InputAction _fireAction;

    //Start() function for networking
    public override void OnNetworkSpawn()
    {
        if(!_pool)
            _pool = GameObject.Find("BulletPool").GetComponent<ObjectPooling>();

        _input = _player.Input;
        _fireAction = _input.actions["Fire"];
        _hasFired = false;
    }

    private void Update()
    {
        //If I am the owner, continue. Prevents other clients from controlling you
        if (!IsOwner)
            return;

        //transform.position += -transform.forward * Time.deltaTime * _speed;

        if (_hasFired)
            _fireTimer += Time.deltaTime;

        if(_fireTimer > _fireSpeed)
        {
            _hasFired = false;
            _fireTimer = 0;
        }

        if (_fireAction.IsPressed() && !_hasFired)
        {
            Fire();
            _hasFired = true;
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
        GameObject bullet = _pool.GetObject();
        bullet.transform.SetPositionAndRotation(_bulletOrigin.transform.position, _bulletOrigin.transform.rotation);
    }
}