using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Input
    private InputReader _inputReader;
    private Vector2 _inputVector;
    private bool _requireNewJumpPress;
    private bool _isJumpPressed;
    private bool _isMovePressed;

    // Movement
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    private Rigidbody2D _body;
    private Ground _ground;
    private Vector2 _desiredVelocity;
    
    //Dash
    [SerializeField, Range(0f, 100f)] private float dashDistance = 60f;
    private bool _isDashing = false;

    // Jump/Gravity
    [SerializeField, Range(0f, 30f)] private float jumpVelocity = 10f;
    [SerializeField, Range(0f, 5f)] private float fallMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float lowFallMultiplier = 3f;


    #region input enables
    // Subscribe to input events
    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.JumpCanceledEvent += OnJumpCanceled;
        _inputReader.DashEvent += DoDash;
    }
    
    // Unsubscribe from input events
    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.JumpCanceledEvent -= OnJumpCanceled;
        _inputReader.DashEvent -= DoDash;
    }
    #endregion

    void Awake()
    {
        _inputReader = ScriptableObject.CreateInstance<InputReader>();
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
    }
    
    private void Update()
    {
        // Calculate velocity from horizontal input
        _desiredVelocity = new Vector2(_inputVector.x, 0f) * Mathf.Max(maxSpeed - _ground.GetFriction(), 0f);
        HandleGravity();
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        var velocity = _body.velocity;

        var acceleration = _ground.GetOnGround() ? maxAcceleration : maxAirAcceleration;
        var maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, _desiredVelocity.x, maxSpeedChange);

        _body.velocity = velocity;
    }

    private void HandleGravity()
    {
        if (_body.velocity.y < 0)
        {
            _body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_body.velocity.y > 0 && !_isJumpPressed)
        {
            _body.velocity += Vector2.up * Physics2D.gravity.y * (lowFallMultiplier - 1) * Time.deltaTime;
        }
    }
    
    private void DoJump()
    {
        var velocity = Vector2.up * jumpVelocity;
        _body.velocity = velocity;
    }

    private void DoDash()
    {
        StartCoroutine(DashAction(_inputVector.x));
    }
    
    private IEnumerator DashAction(float direction)
    {
        _isDashing = true;
        _body.velocity = new Vector2(_body.velocity.x, 0f);
        _body.AddForce(new Vector2(dashDistance * _inputVector.x, 0f), ForceMode2D.Impulse);
        var gravityScale = _body.gravityScale;
        float gravity = gravityScale;
        yield return new WaitForSeconds(0.4f);
        _isDashing = false;
        gravityScale = gravity;
        _body.gravityScale = gravityScale;
    }

    private void OnJump()
    {
        _isJumpPressed = true;
        
        if (_ground.GetOnGround()) 
            DoJump();
    }    
    
    private void OnJumpCanceled()
    {
        _isJumpPressed = false;
    }

    private void OnMove(Vector2 inputVector)
    {
        _inputVector = inputVector;
        _isMovePressed = _inputVector != Vector2.zero;
    }
}
