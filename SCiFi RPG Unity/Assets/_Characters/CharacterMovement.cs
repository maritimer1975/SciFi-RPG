using UnityEngine;
using UnityEngine.AI;

// TODO: Consider rewiring
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{

    #region REQUIRED COMPONENTS
    [RequireComponent(typeof(NavMeshAgent))]
#endregion

    public class CharacterMovement : MonoBehaviour
    {
#region VARIABLES
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveThreshold = 1f;

        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;

        [SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;

        CameraRaycaster cameraRaycaster;
        Vector3 clickPoint, movement;

        //AICharacterControl aiCharacterControl;

        Rigidbody rb;

        NavMeshAgent agent;

        Animator anim;

        bool isInDirectMode = false;

        float speed = 5f;
        float turnAmount;
		float forwardAmount;
		Vector3 groundNormal;

#endregion

#region UNITY METHODS
        void Awake()
        {
            InitializeComponents();
        }

        void Start()
        {
            RegisterObservers();
        }

        void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

#endregion

#region CUSTOM METHODS
        void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}
        
        private void InitializeComponents()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;
            
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            anim = GetComponent<Animator>();
            //anim.applyRootMotion = true;  // TODO: Consider if needed.
        }

         void Move(float h, float v)
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime; 
            rb.MovePosition(transform.position + movement);
        }

        public void Move(Vector3 movement)
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

        void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position); //aiCharacterControl.SetTarget(enemy.transform);
            }
        }

         void OnMouseOverTerrain(Vector3 destination)
        {           
            if(Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }

        private void RegisterObservers()
        {
            cameraRaycaster.notifyMouseOverTerrain += OnMouseOverTerrain;

            cameraRaycaster.notifyMouseOverEnemy += OnMouseOverEnemy;
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

        private Vector3 ShortDestination(Vector3 destination, float shortening)
        {
            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
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