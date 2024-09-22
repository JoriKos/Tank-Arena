using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
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
        // TODO:
        // Add the feature for turning our vehicle before being able to move in that direction
        // Perhaps using a Dot product of the forward/backward facing side of our tank, then take the direction we want to move
        // and not allow it until a certain value. Perhaps fully, perhaps halfway? Figure out what's best
        print(_moveAction.ReadValue<Vector2>());

        // For some reason, the negative Y value needs to be the X direction, the positive X value needs to be the Z, Y dir (up/down) is alway zero
        // Drag on the Rigidbody accounts for slowing down
        _moveDir.Set(-_moveAction.ReadValue<Vector2>().y, 0, _moveAction.ReadValue<Vector2>().x);

        // Adds force to the rigidbody based on the Vector2 of _moveAction.
        _rb.AddForce(_moveDir * _moveSpeed * Time.deltaTime);
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
    
}
