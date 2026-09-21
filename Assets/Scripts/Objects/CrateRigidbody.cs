using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CrateRigidbody : MonoBehaviour , IMovable, IGrabbable
{
    private Rigidbody _rigidbody;
    private Transform _grabber;
    private bool _isGrabbed;
    private Vector3 _offset;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isGrabbed)
            transform.position = _grabber.position;
    }
    
    public void Move(Vector3 direction, float force)
    {
        _rigidbody.AddForce(Vector3.Normalize(direction) * force);
    }

    public void Grab(Transform grabber)
    {
        if (_isGrabbed)
        {
            Ungrab();
            return;
        }

        _grabber = grabber;
        _isGrabbed = true;
        _rigidbody.isKinematic = true;
    }
    
    private void Ungrab()
    {
        _grabber = null;
        _offset = Vector3.zero;
        _isGrabbed = false;
        _rigidbody.isKinematic = false;
    }
}
