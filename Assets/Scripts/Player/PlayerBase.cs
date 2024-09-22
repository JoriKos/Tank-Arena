using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody Rigidbody { get { return _rigidbody; } }

    [SerializeField] private GameObject _playerObject;
    public GameObject PlayerObject { get { return _playerObject; } }

    [SerializeField] private PlayerInput _input;
    public PlayerInput Input { get { return _input; } }

}
