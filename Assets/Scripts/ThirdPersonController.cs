using UnityEngine;
using System.Collections;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;

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
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        public int nowStage = 0;
        public bool UsingXRDevice = true;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

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


        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 100.0f;
        private float _xRotation = 0.0f;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        // XR Controller
        private UnityEngine.XR.InputDevice _leftController;
        private UnityEngine.XR.InputDevice _rightController;
        private Vector3 _lastVelocity;
        private Vector3 _lastPosition;

        private float _lastShakeTime = -1f;
        private float _shakeCooldown = 0.5f;
        public int movementState = 0; // 0: 정지, 1: 걷기

        private float _baseSpeed = 2.0f;
        private float _walkSpeed = 2.0f;
        private float _runSpeed = 5.0f;

        private bool _moveTrigger = false;

        public WhipEffect whipEffect;
        public Animator whipAnimator;

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
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            _hasAnimator = TryGetComponent(out _animator);
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            //Debug.Log("_hasAnimator : " + _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _input.analogMovement = false;
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform; // Main Camera 자동 할당
            }
            if (playerBody == null)
            {
                playerBody = this.transform; // 현재 오브젝트(Player) 자동 할당
            }

            AssignAnimationIDs();

            //오른쪽 컨트롤러 가져오기
            var devices = new List<UnityEngine.XR.InputDevice>();
            var L_devices = new List<UnityEngine.XR.InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, L_devices);
            if (devices.Count > 0 && L_devices.Count > 0)
            {
                _leftController = L_devices[0];
                _rightController = devices[0];
                UsingXRDevice = true;
            } else
            {
                UsingXRDevice = false;
            }
            //Debug.Log("XR디바이스 사용 여부 : " + UsingXRDevice + " 정보 : " + _rightController);

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            StartCoroutine(VRDeviceProcess());
        }

        private void Update()
        {
            if (!GameManager.Instance.CheckPaused())
            {
                if (UsingXRDevice)
                {
                    //switch (nowStage)
                    //{
                    //    case 1:
                    //        DetectShakeGesture();
                    //        DetectPullGesture();
                    //        break;
                    //    case 2:
                    //        TriggerMoveOfSecondStage();
                    //        break;
                    //    default:
                    //        break;
                    //}

                    // 현재 움직임 상태에 따라 속도 설정
                    if (nowStage != 3)
                    {
                        switch (movementState)
                        {
                            case 0: _speed = 0f; break;
                            case 1: _speed = _input.sprint ? SprintSpeed : MoveSpeed; break;
                        }
                        JumpAndGravity();
                        GroundedCheck();
                        Move();
                    }
                }
            }  
        }

        private void LateUpdate()
        {
            if (!GameManager.Instance.CheckPaused())
            {
                //CameraRotation();
                RotateCamera();
            }
        }

        private IEnumerator VRDeviceProcess()
        {
            while (true)
            {
                if (!GameManager.Instance.CheckPaused())
                {
                    if (UsingXRDevice)
                    {
                        switch (nowStage)
                        {
                            case 1:
                                DetectShakeGesture();
                                DetectPullGesture();
                                break;
                            case 2:
                                TriggerMoveOfSecondStage();
                                break;
                            case 3:
                                DetectShakeGesture();
                                DetectPullGesture();
                                break;
                            default:
                                break;
                        }
                    }  
                }
                yield return new WaitForSeconds(0.1f);
            }
            
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            //Debug.Log("spherePosition : " + spherePosition);
            //Debug.Log(_hasAnimator);
            //Debug.Log("GroundLayers : " + GroundLayers);
            //Debug.Log("Grounded : " + Grounded);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move() //Move() 함수 역할별 분리
        {
            float targetSpeed = GetTargetSpeed();
            UpdateSpeed(targetSpeed);

            Vector3 inputDirection = GetInputDirection();
            Quaternion moveRotation = GetMoveRotation();

            Vector3 moveDirection = (moveRotation * inputDirection).normalized;

            ApplyMovement(moveDirection);
        }

        private float GetTargetSpeed() // 현재 속도
        {
            if (!UsingXRDevice && _input.move.magnitude < 0.01f)
                return 0f;

            if (_moveTrigger)
                return _speed;

            return 0f;
        }

        private Vector3 GetInputDirection() // 입력 방향
        {
            if (nowStage == 1 || nowStage == 2 || nowStage == 3)
            {
                if (_moveTrigger)
                    return Vector3.forward;

                return new Vector3(_input.move.x, 0f, _input.move.y);
            }

            return Vector3.forward;
        }

        private Quaternion GetMoveRotation() //움직임 회전
        {
            if (InputDevices.GetDeviceAtXRNode(XRNode.Head).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion hmdRotation))
            {
                return Quaternion.Euler(0f, hmdRotation.eulerAngles.y, 0f);
            }
            else
            {
                return Quaternion.Euler(0f, CinemachineCameraTarget.transform.eulerAngles.y, 0f);
            }
        }

        private void UpdateSpeed(float targetSpeed) //속도 업데이트
        {
            float currentSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (Mathf.Abs(currentSpeed - targetSpeed) > speedOffset)
            {
                _speed = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
        }

        private void ApplyMovement(Vector3 moveDirection) //움직임 적용
        {
            _controller.Move(moveDirection * (_speed * Time.deltaTime) + new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

            if (_hasAnimator)
            {
                float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }


        private void RotateCamera()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // 상하 회전 제한 (카메라만)
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            // 좌우 회전 (플레이어 전체 회전)
            playerBody.Rotate(Vector3.up * mouseX);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
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

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        public bool GetJumpValueForStage3()
        {
            return _input.jump;
        }

        public void SetJumpfalseForStage3()
        {
            _input.jump = false;
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        private void DetectShakeGesture()
        {
            if (!_rightController.isValid)
            {
                _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                return;
            }

            if (_rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out Vector3 velocity))
            {
                float shakeStrength = (velocity - _lastVelocity).magnitude;

                if (shakeStrength > 1.5f && Time.time - _lastShakeTime > _shakeCooldown)
                {
                    _lastShakeTime = Time.time;
                    movementState = 1;
                    _moveTrigger = true;
                    SendHapticFeedback(_rightController, 0.7f, 0.15f);
                    whipEffect?.PlayWhip();
                    whipAnimator?.SetTrigger("Whip");
                }

                _lastVelocity = velocity;
            }
        }

        private void DetectPullGesture()
        {
            if (!_rightController.isValid)
            {
                _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                return;
            }

            if (_rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 currentPosition))
            {
                float deltaZ = currentPosition.z - _lastPosition.z;

                if (deltaZ < -0.05f)
                {
                    // Debug.Log("⏬ 속도 줄이기 (몸 쪽으로 당김)");
                    movementState = 0; 
                    // Debug.Log($"🔄 상태 변경: {movementState} (0: 정지, 1: 걷기)");
                    //_speed *= 0.5f;
                    _moveTrigger = false;
                    SendHapticFeedback(_rightController, 0.7f, 0.15f);
                }

                _lastPosition = currentPosition;
            }
        }

        private void SendHapticFeedback(UnityEngine.XR.InputDevice device, float amplitude = 0.5f, float duration = 0.2f)
        {
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0, amplitude, duration);
            }
        }

        private void TriggerMoveOfSecondStage()
        {
            if (UsingXRDevice)
            {
                float leftTrigger = 0f;
                float rightTrigger = 0f;
                // 트리거 값을 읽기 (0~1)
                if (_leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out leftTrigger))
                {
                    Debug.Log($"Left Trigger: {leftTrigger}");
                }

                if (_rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out rightTrigger))
                {
                    Debug.Log($"Right Trigger: {rightTrigger}");
                }


                if (rightTrigger > 0.5f)
                {
                    _speed = 4.0f; // 앞으로
                    _moveTrigger = true;
                    movementState = 1;
                }
                else if (leftTrigger > 0.5f)
                {
                    _speed = -4.0f; // 앞으로
                    _moveTrigger = true;
                    movementState = 1;
                }
                else
                {
                    _speed = 0f; // 앞으로
                    _moveTrigger = false;
                    movementState = 0;
                }
            }
        }
    }
}