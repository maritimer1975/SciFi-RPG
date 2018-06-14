using UnityEngine;
using UnityEngine.AI;

// TODO: Consider rewiring
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class Character : MonoBehaviour
    {
#region SERIALIZED VARIABLES

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animController;
        [SerializeField] AnimatorOverrideController animOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Audio Source")]
        [SerializeField] [Range(0,1)] float audioSourceSpatialBlend = .5f;

        [Header("Capsule Collider")]
        [SerializeField] float colliderRadius;
        [SerializeField] float colliderHeight;

        [SerializeField] Vector3 colliderCenter;

        [Header("Nav Mesh Agent")]
        [SerializeField] [Tooltip("NavMesh movement speed")] float speed = 1f;
        [SerializeField] float angularSpeed = 1000f;
        [SerializeField] float acceleration = 50f;

        [SerializeField] float stoppingDistance = 1.5f;
        [SerializeField] float navMeshRadius = 0.2f;
        [SerializeField] float navMeshHeight = 2f;

        [Header("Movement")]
        [SerializeField] float moveThreshold = 1f;
        [SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;

        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;

#endregion

#region VARIABLES
        CameraRaycaster cameraRaycaster;
        Vector3 movement;

        //AICharacterControl aiCharacterControl;

        Rigidbody rb;

        bool isAlive = true;

        NavMeshAgent agent;

        Animator anim;

        float turnAmount;
		float forwardAmount;
		Vector3 groundNormal;

#endregion

#region UNITY METHODS
        void Awake()
        {
            AddRequiredComponents();
        }

        void Start()
        {
            InitializeComponents();
            //RegisterObservers();
        }

        void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance && isAlive)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

#endregion

#region PROPERTIES SET/GET

        public AnimatorOverrideController getAnimatorOverrideController { get { return animOverrideController; } }

#endregion

#region CUSTOM METHODS
        
        void AddRequiredComponents()
        {
            // Animator setup
            anim = gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = animController;
            anim.avatar = characterAvatar;

            // Collider Setup
            var collider = gameObject.AddComponent<CapsuleCollider>();
            collider.center = colliderCenter;
            collider.radius = colliderRadius;
            collider.height = colliderHeight;

            // Audiosource Setup
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            // Rigidbody Setup
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            // NavMesh Setup
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.speed = speed;
            agent.angularSpeed = angularSpeed;
            agent.acceleration = acceleration;
            agent.radius = navMeshRadius;
            agent.height = navMeshHeight;
            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.autoBraking = false;
        }
        
        void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}
        
        private void InitializeComponents()
        {
        
        }

        public void Kill()
        {
            // to allow signaling of character kill
            isAlive = false;
        }

        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();
			
            UpdateAnimator();
        }

        void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (Time.deltaTime > 0)
			{
				Vector3 velocity = (anim.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				velocity.y = rb.velocity.y;
				rb.velocity = velocity;
			}        
        }

        public void SetDestination(Vector3 worldPos)
        {
            agent.destination = worldPos;
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

        void Turn()
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;

            if(Physics.Raycast(camRay, out raycastHit, 100f))
            {
                Vector3 playerToMouse = raycastHit.point - transform.position;
                playerToMouse.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                rb.MoveRotation(newRotation);

            }
        }

        void UpdateAnimator( )
		{
			anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            anim.speed = animationSpeedMultiplier;			
		}
#endregion
    }
}