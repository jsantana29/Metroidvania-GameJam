using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Jump : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
        [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
        [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
        [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
        private InputReaderSO _inputReader;
        
        private Rigidbody2D body;
        private Ground ground;
        private Vector2 velocity;

        private int jumpPhase;
        private float defaultGravityScale;

        private bool desiredJump;
        private bool onGround;

        private void OnJumpAction() => desiredJump = true;
        private void OnJumpCanceledAction() => desiredJump = false;
        
        void OnEnable()
        {
            _inputReader.JumpEvent += OnJumpAction;
            _inputReader.JumpCanceledEvent += OnJumpCanceledAction;
        }
        void OnDisable()
        {
            _inputReader.JumpEvent -= OnJumpAction;
            _inputReader.JumpCanceledEvent -= OnJumpCanceledAction;
        }
        
        void Awake()
        {
            _inputReader = GetComponent<InputInit>().GetInput();
            body = GetComponent<Rigidbody2D>();
            ground = GetComponent<Ground>();
            
            defaultGravityScale = 1f;
        }

        private void FixedUpdate()
        {
            onGround = ground.GetOnGround();
            velocity = body.velocity;

            if (onGround)
            {
                jumpPhase = 0;
            }
            if (desiredJump)
            {
                JumpAction();
            }
            if (body.velocity.y > 0)
            {
                body.gravityScale = upwardMovementMultiplier;
            }
            else if (body.velocity.y < 0)
            {
                body.gravityScale = downwardMovementMultiplier;
            }
            else if(body.velocity.y == 0)
            {
                body.gravityScale = defaultGravityScale;
            }

            body.velocity = velocity;
        }
        
        private void JumpAction()
        {
            Debug.Log("Jump Action executed");
            if (onGround || jumpPhase < maxAirJumps)
            {
                jumpPhase += 1;
                float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
                if (velocity.y > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
                }
                else if (velocity.y < 0f)
                {
                    jumpSpeed += Mathf.Abs(body.velocity.y);
                }
                velocity.y += jumpSpeed;
            }
        }
        
        public override string ToString()
        {
            return $"On Ground: {onGround}\n" +
                   $"Jump Phase: {jumpPhase}\n" +
                   $"Jump Velocity: {velocity}\n";
        }
    }