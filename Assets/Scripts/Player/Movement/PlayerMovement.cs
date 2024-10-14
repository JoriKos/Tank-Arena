using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //=============================================================================
    // TODO:
    // Create a system where going forwards/backwards compared to current pos doesn't need turning around
    // Change rotation to rotate fastest way rather than always the same way

    [SerializeField] private PlayerBase _player;
    
    // Movement
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private int DotPScaler = 1000;

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
        _fireAction = _input.actions["Fire"]; //Move this

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

    //---------------------------------------------------------------------------------------------------
    // Purpose: 
    // Moving and rotating the player
    // If the tank is not facing the same way as the player wants to move, rotate the tank first.
    //---------------------------------------------------------------------------------------------------
    private void Move()
    {
        //Normalise vectors
        Vector3 playerDir = gameObject.transform.up;
        //Mimic _moveDir.Set() we do below
        Vector3 moveDir = new(-_moveAction.ReadValue<Vector2>().y, 0, _moveAction.ReadValue<Vector2>().x);

        float fDotP = Vector3.Dot(playerDir.normalized, moveDir.normalized);

        // Due to a float's imprecision, it sometimes refuses to be acknowledged as 1 even with Mathf.Approximately
        // Because of this, we have a scaler that we use so we have an int to work with. Without scaling, it turns into 0 or 1
        int iDotP = Convert.ToInt32(fDotP * DotPScaler);

        Debug.Log(Vector3.Distance(playerDir, moveDir));

        // We check the scaled dot product result against the scaler
        if (iDotP != DotPScaler && iDotP != -DotPScaler)
        {

            //BUG: if they need to move to something that's in the middle, it switches between left and right constantly and ends up not moving
            //CAUSE: one possible cause is that it moves in the wrong direction
            //float turnDir = Vector3.Distance(playerDir, moveDir) < 1.5 ? 1 : -1;
            float turnDir = 1;
            //Just a failsafe in case they're equal
            //if (Vector3.Distance(playerDir, moveDir) == Vector3.Distance(-playerDir, moveDir))
             //   turnDir = 1;

            gameObject.transform.Rotate(0, 0, turnDir * Time.deltaTime * _turnSpeed);

            return;
        }

        // For some reason, the negative Y value needs to be the X direction, the positive X value needs to be the Z, Y dir (up/down) is alway zero
        // Drag on the Rigidbody accounts for slowing down
        _moveDir.Set(-_moveAction.ReadValue<Vector2>().y, 0, _moveAction.ReadValue<Vector2>().x);

        // Adds force to the rigidbody based on the Vector2 of _moveAction.
        _rb.AddForce(_moveSpeed * Time.deltaTime * _moveDir);
    }

    //Move this
    private void Fire()
    {
        //print("FIRE!");
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
