using UnityEngine;

namespace RPG.Characters
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;

		[SerializeField] float animSpeedMultiplier = 1f;
		[SerializeField] float groundCheckDistance = 0.1f;
		[SerializeField] float moveThreshold = 1f;

		Rigidbody rb;
		Animator anim;

		float origGroundCheckDistance;

		float turnAmount;
		float forwardAmount;
		Vector3 groundNormal;


		void Start()
		{
			anim = GetComponent<Animator>();
			rb = GetComponent<Rigidbody>();

			rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			origGroundCheckDistance = groundCheckDistance;

			//m_GroundNormal = hitInfo.normal;
			//m_IsGrounded = true;
			anim.applyRootMotion = true;
		}


		public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();
			
            UpdateAnimator();
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
			// convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction.
			
			if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }

            var localMovement = transform.InverseTransformDirection(movement);

            localMovement = Vector3.ProjectOnPlane(localMovement, groundNormal);
            
			turnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            forwardAmount = localMovement.z;
        }

        void UpdateAnimator( )
		{
			anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);			
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}
	}
}
