using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using System.Collections;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;
        public float inputValidSec = 0.5f;

        [Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		private float _deadZone = 0.1f;
		private Vector2 _lastValidMoveInput = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
        private Coroutine resetMoveCoroutine;

        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            Debug.Log("OnMove() ȣ�� : " + input.magnitude);

            if (input.magnitude > _deadZone)
            {
                move = input;
                _lastValidMoveInput = input;

                // �Է��� ��ȿ�ϹǷ� �ڷ�ƾ ���� (���� X)
                if (resetMoveCoroutine != null)
                {
                    StopCoroutine(resetMoveCoroutine);
                    resetMoveCoroutine = null;
                }
            }
            else
            {
                // ��ȿ���� ���� �Է��� ���, Ÿ�̸� ����
                if (resetMoveCoroutine == null)
                {
                    resetMoveCoroutine = StartCoroutine(ResetMoveAfterDelay(inputValidSec));
                }
            }
        }

        private IEnumerator ResetMoveAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            move = Vector2.zero;
            Debug.Log("move�� 0.5�ʰ� �Է� ��� Vector2.zero�� �ʱ�ȭ��");
        }

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}