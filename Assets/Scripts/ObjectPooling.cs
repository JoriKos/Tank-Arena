using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private GameObject _objectPool;
    [SerializeField] private Queue<GameObject> _objectPooler = new();
    [SerializeField] private int _poolStartSize;
    [SerializeField] private bool _activeOnStart = false;
    public int PoolStartSize { get { return _poolStartSize; } }

    // Game starts, create pool
    private void Awake()
    {
        for (int i = 0; i < _poolStartSize; i++)
        {
            GameObject obstacle = Instantiate(_objectPool);
            obstacle.transform.parent = transform;
            _objectPooler.Enqueue(obstacle);
            obstacle.SetActive(_activeOnStart);
        }
    }

    // Enable object. If no object is available, spawn a new one
    public GameObject GetObject(bool setActiveImmediately = true)
    {
        if (_objectPooler.Count > 0)
        {
            GameObject tObject = _objectPooler.Dequeue();
            tObject.SetActive(setActiveImmediately);
            return tObject;
        }
        else
        {
            GameObject tObject = Instantiate(_objectPool);
            tObject.transform.parent = transform;
            _objectPooler.Enqueue(tObject);
            tObject.SetActive(setActiveImmediately);
            return tObject;
        }
    }

    //Put back into queue
    public void ReturnObject(GameObject tObject)
    {
        _objectPooler.Enqueue(tObject);
        tObject.SetActive(false);
    }
}