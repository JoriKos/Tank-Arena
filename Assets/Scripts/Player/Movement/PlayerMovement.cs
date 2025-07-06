using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    //=============================================================================
    // TODO:
    // Implement this into networking

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

    //Start() function for networking
    public override void OnNetworkSpawn()
    {
        _rb = _player.Rigidbody;
        _input = _player.Input;

        _moveAction = _input.actions["Move"];
    }

    private void Update()
    {
        //If I am the owner, continue. Prevents other clients from controlling you (like 1984)
        if (!IsOwner)
        {
            return;
        }
        // This basically means "if any of the movement keys are held"
        if (_moveAction.inProgress)
            Move();
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


        // We check the scaled dot product result against the scaler
        if (iDotP != DotPScaler && iDotP != -DotPScaler)
        {
            float turnDir;

            //Check which direction has an angle closest to the desired direction
            if (Vector3.Angle(playerDir, moveDir) > Vector3.Angle(-playerDir, moveDir))
                turnDir = Vector3.Cross(playerDir, moveDir).y;
            else
                turnDir = Vector3.Cross(-playerDir, moveDir).y;

            //The Vector3.Cross output can range from -1 to 1, but we only need to know if it's negative or positive
            //We change it to 1 if it's anything positive or 0, and to -1 if it's a negative number
            turnDir = (turnDir >= 0) ? 1 : -1;

            //Turn the bloody thing!
            gameObject.transform.Rotate(0, 0, turnDir * Time.deltaTime * _turnSpeed);

            return;
        }

        // For some reason, the negative Y value needs to be the X direction, the positive X value needs to be the Z, Y dir (up/down) is alway zero
        // Drag on the Rigidbody accounts for slowing down
        _moveDir.Set(-_moveAction.ReadValue<Vector2>().y, 0, _moveAction.ReadValue<Vector2>().x);

        // Adds force to the rigidbody based on the Vector2 of _moveAction.
        _rb.AddForce(_moveSpeed * Time.deltaTime * _moveDir);
    }

    private void OnEnable()
    {
        //Checks if null
        //Should probably find a better way to implement this...
        _pControls ??= new PlayerControls();

        _pControls.TankControls.Enable();
    }

    private void OnDisable()
    {
        _pControls.TankControls.Disable();
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
