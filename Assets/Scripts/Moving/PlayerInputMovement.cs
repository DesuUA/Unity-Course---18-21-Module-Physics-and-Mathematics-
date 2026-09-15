using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMovement : MonoBehaviour
{
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";
    private Mover _mover;
    
    [SerializeField] private KeyCode _boostKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    private void Start()
    {
        _mover = GetComponent<Mover>();
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw(HorizontalAxisName), 0, Input.GetAxisRaw(VerticalAxisName));
        
        Vector3 inputDirection = input.sqrMagnitude > 0.01f ? new Vector3(input.x, 0, input.z).normalized : Vector3.zero;
        
        bool isBoosting = Input.GetKey(_boostKey);
        bool jump = Input.GetKeyDown(_jumpKey);
        
        _mover.SetDirection(inputDirection);
        _mover.SetSpeedBoost(isBoosting);
        _mover.SetJump(jump);
    }
}
