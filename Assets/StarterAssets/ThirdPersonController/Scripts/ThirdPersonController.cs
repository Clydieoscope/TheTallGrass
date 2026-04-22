using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        public static ThirdPersonController Instance { get; private set; }

        [Header("Player")]
        [Tooltip("Crouch speed of the character in m/s")]
        public float CrouchSpeed = 1.0f;

        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioSource AudioFootsteps;
        public AudioSource LandingAudio;
        public AudioSource AudioFoley;
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animVelocityX;
        private float _animVelocityZ;
        // private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // Exhaustion speed multiplier — set by StaminaSystem (1.0 = normal, <1.0 = penalty)
        private float _exhaustionSpeedMultiplier = 1.0f;

        private bool _hasWeapon = false;

        [Header("Crouching")]
        [SerializeField] private float crouchHeight = 1.2f;
        [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.595f, 0);
        [SerializeField] private float crouchTransitionSpeed = 7f;
        private float standHeight;
        private Vector3 standCenter;
        private bool crouched;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDVelocityX;
        private int _animIDVelocityZ;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDAttack;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (_mainCamera == null)
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            standCenter = _controller.center;
            standHeight = _controller.height;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            GroundedCheck();
            JumpAndGravity();
            Move();
            UpdateControllerCollider();

            Attack();

            if (_input.crouch)
            {
                crouched = !crouched;
                _input.crouch = false;
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDVelocityX = Animator.StringToHash("VelocityX");
            _animIDVelocityZ = Animator.StringToHash("VelocityZ");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDAttack = Animator.StringToHash("Attack");
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : crouched ? CrouchSpeed : MoveSpeed;

            if (_input.sprint)
                crouched = false;

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // Apply exhaustion multiplier
            targetSpeed *= _exhaustionSpeedMultiplier;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // Rotate character to always face camera forward direction
            float cameraYaw = _mainCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0f, cameraYaw, 0f),
                RotationSmoothTime > 0 ? Time.deltaTime / RotationSmoothTime : 1f
            );

            // Move direction is camera-relative: W = forward, A = left, D = right, S = back
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            Vector3 moveDirection = Quaternion.Euler(0f, cameraYaw, 0f) * inputDirection;

            _controller.Move(moveDirection * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // Compute local-space velocity for the 2D blend tree
            // Project world velocity onto character's local axes
            Vector3 worldVelocity = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z);
            Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);

            float targetVelocityX = localVelocity.x;
            float targetVelocityZ = localVelocity.z;

            _animVelocityX = Mathf.Lerp(_animVelocityX, targetVelocityX, Time.deltaTime * SpeedChangeRate);
            _animVelocityZ = Mathf.Lerp(_animVelocityZ, targetVelocityZ, Time.deltaTime * SpeedChangeRate);

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDVelocityX, _animVelocityX);
                _animator.SetFloat(_animIDVelocityZ, _animVelocityZ);
                _animator.SetBool("Crouched", crouched);
            }
        }

        private void UpdateControllerCollider()
        {
            float targetHeight = crouched ? crouchHeight : standHeight;
            Vector3 targetCenter = crouched ? crouchCenter : standCenter;

            if (targetHeight > _controller.height)
            {
                float heightDelta = targetHeight - _controller.height;
                Vector3 topOfCapsule = transform.position + Vector3.up * (_controller.height + _controller.skinWidth);
                bool blocked = Physics.SphereCast(topOfCapsule, _controller.radius, Vector3.up, out _, heightDelta, GroundLayers);
                if (blocked) return;
            }

            float newHeight = Mathf.Lerp(_controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
            Vector3 newCenter = Vector3.Lerp(_controller.center, targetCenter, crouchTransitionSpeed * Time.deltaTime);

            if (Mathf.Abs(newHeight - targetHeight) < 0.01f) newHeight = targetHeight;
            if (Vector3.Distance(newCenter, targetCenter) < 0.01f) newCenter = targetCenter;

            _controller.height = newHeight;
            _controller.center = newCenter;
        }

        private void JumpAndGravity()
        {
            if (float.IsNaN(_verticalVelocity) || float.IsInfinity(_verticalVelocity))
                _verticalVelocity = -2f;

            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                if (_verticalVelocity < 0.0f)
                    _verticalVelocity = -2f;

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    if (!_animator.IsInTransition(0) && !crouched)
                    {
                        _input.jump = false;
                        float jumpCalc = JumpHeight * -2f * Gravity;
                        _verticalVelocity = Mathf.Sqrt(Mathf.Max(0f, jumpCalc));

                        if (_hasAnimator)
                            _animator.SetBool(_animIDJump, true);
                    }
                    else if (crouched)
                    {
                        _input.jump = false;
                        crouched = false;
                    }
                }

                if (_jumpTimeoutDelta >= 0.0f)
                    _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                    _fallTimeoutDelta -= Time.deltaTime;
                else
                {
                    if (_hasAnimator)
                        _animator.SetBool(_animIDFreeFall, true);
                }

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += Gravity * Time.deltaTime;
        }

        /// <summary>
        /// Called by StaminaSystem to apply or remove an exhaustion movement penalty.
        /// Pass 1.0 to restore normal speed, less than 1.0 to penalize.
        /// </summary>
        public void SetExhaustionMultiplier(float multiplier)
        {
            _exhaustionSpeedMultiplier = Mathf.Clamp01(multiplier);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (AudioFootsteps != null)
                    AudioFootsteps.Play();
                if (AudioFoley != null)
                    AudioFoley.Play();
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (LandingAudio != null)
                    LandingAudio.Play();
            }
        }

        private void Attack()
        {
            if (_input.attack)
            {
                if (!_hasWeapon || _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                {
                    _animator.ResetTrigger(_animIDAttack);
                    _input.attack = false;
                    return;
                }

                if (_hasAnimator)
                    _animator.SetTrigger(_animIDAttack);

                _input.attack = false;
            }
        }

        public bool IsCrouched()
        {
            return crouched;
        }

        public void EquipWeapon() => _hasWeapon = true;
    }
}