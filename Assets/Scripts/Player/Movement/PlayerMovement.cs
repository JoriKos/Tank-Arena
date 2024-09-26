using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // TODO:
    // Create a system where going forwards/backwards compared to current pos doesn't need turning around
    // Sometimes, the dot product won't be registered
    // Change rotation to rotate fastest way rather than always the same way
    //
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private PlayerBase _player;
    
    // Movement
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody _rb;

    private Vector3 _moveDir;

    // Controls
    private PlayerControls _pControls;
    private PlayerInput _input;

    // These inputs are a shortcut for "PlayerControls.ActionMap.InputAction"
    private InputAction _moveAction;
    private InputAction _fireAction;

    private void Awake()
    {
        _pControls = new PlayerControls();
        _rb = _player.Rigidbody;
        _input = _player.Input;

        _moveAction = _input.actions["Move"];
        _fireAction = _input.actions["Fire"];

    }

    private void Update()
    {
        // This basically means "if any of the movement keys are held"
        if (_moveAction.inProgress)
            Move();

        // Triggered, libshart?
        if (_fireAction.triggered)
            Fire();
    }
    private void Move()
    {
        Vector3 playerDirNormal = gameObject.transform.up.normalized;
        Vector3 moveDirNormal = new Vector3(-_moveAction.ReadValue<Vector2>().y, 0, _moveAction.ReadValue<Vector2>().x).normalized;
        
        if (Vector3.Dot(playerDirNormal, moveDirNormal) > 1 || Vector3.Dot(playerDirNormal, moveDirNormal) < 1 )
        {
            gameObject.transform.Rotate(0, 0, _turnSpeed);
            return;
        }
        // For some reason, the negative Y value needs to be the X direction, the positive X value needs to be the Z, Y dir (up/down) is alway zero
        // Drag on the Rigidbody accounts for slowing down
        _moveDir.Set(-_moveAction.ReadValue<Vector2>().y, 0, _moveAction.ReadValue<Vector2>().x);

        // Adds force to the rigidbody based on the Vector2 of _moveAction.
        _rb.AddForce(_moveSpeed * Time.deltaTime * _moveDir);
    }

    private void Fire()
    {
        print("FIRE!");
    }


    private void OnEnable()
    {
        _pControls.Controls.Enable();
    }

    private void OnDisable()
    {
        _pControls.Controls.Disable();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Debug.DrawRay(gameObject.transform.position, new Vector3(-_moveAction.ReadValue<Vector2>().y, 0, _moveAction.ReadValue<Vector2>().x) * 10);
            Debug.DrawRay(gameObject.transform.position, -gameObject.transform.up * 10, Color.blue);
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.up * 10, Color.green);
        }
    }

}
