using UnityEngine;

public class SimpleRotate : MonoBehaviour
{

    [SerializeField] Transform _tf;
    [SerializeField] float x, y, z;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _tf.Rotate(new Vector3(x, y, z) * Time.deltaTime);
    }
}
