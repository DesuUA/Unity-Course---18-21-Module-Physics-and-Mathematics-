using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoverRigidBody : Mover
{
    [Header("Ground Check (Collision)")]
    [Tooltip("Max slop angle to jump (0 — plane, 90 — wall)")]
    [Range(0f, 89f)] [SerializeField] private float _maxSlopeAngle = 50f;
    
    [Header("Disable options")]
    [Tooltip("Colliders tags to disable movement")]
    [SerializeField] private string _disableTag = "Movable Table";
    
    private readonly Vector3 _jumpDirection = Vector3.up;
    
    private float _minGroundNormalY = 0.6f;
    private Rigidbody _rbMover;
    private bool _jumpRequest;
    private bool _isGrounded;
    private bool _disabled;
    
    public Vector3 MoveDirection { get; private set; }

    protected override void Start()
    {
        base.Start();
        _rbMover = GetComponent<Rigidbody>();
        _minGroundNormalY = Mathf.Cos(_maxSlopeAngle * Mathf.Deg2Rad);
    }

    public override void SetDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            MoveDirection = Vector3.zero;
            return;
        }
        MoveDirection = direction * (CurrentSpeed);
    }
    
    public override void SetJump(bool jump)
    {
        if (jump) 
        {
            _jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        if (_disabled)
            return;
        
        if (_jumpRequest && _isGrounded)
        {
            _rbMover.AddForce(_jumpDirection * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
        
        _jumpRequest = false;
        
        if (_isGrounded)
            _rbMover.AddForce(MoveDirection, ForceMode.Force);
        
        _isGrounded = false;
    }
    
    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint contact = collision.GetContact(i);
            
            if (contact.normal.y >= _minGroundNormalY)
            {
                _isGrounded = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_disableTag))
        {
            _disabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_disableTag))
        {
            _disabled = false;
        }
    }
}
