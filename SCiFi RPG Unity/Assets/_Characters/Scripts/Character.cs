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

        [Header("Mouse Over")]
        [SerializeField] float mouseOverRadius;

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
        //[SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float animSpeedMultiplier = 1.5f;

        [Header("Character Stats")]
        [SerializeField] float strength;
        [SerializeField] public CharacterStat dexterity;

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
		Vector3 groundNormal = Vector3.up;

#endregion

#region CONSTANTS
        private const int mouseOverLayer = 10;
#endregion

#region UNITY METHODS
        void Awake()
        {
            AddRequiredComponents();
            AddMouseOver();
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

        public AnimatorOverrideController getAnimOverrideController { get { return animOverrideController; } }
        public float getAnimSpeedMultiplier { get { return animSpeedMultiplier; } }

#endregion

#region CUSTOM METHODS

        void AddMouseOver()
        {
            var mouseOverObj = new GameObject("MouseOver");
            mouseOverObj.transform.parent = this.gameObject.transform;
            mouseOverObj.transform.position = transform.position;
            mouseOverObj.layer = mouseOverLayer;

            var mouseOverCollider = mouseOverObj.AddComponent<CapsuleCollider>();
            mouseOverCollider.center = colliderCenter;
            mouseOverCollider.height = colliderHeight;
            mouseOverCollider.radius = mouseOverRadius;
        }

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
			
            Vector3 position = anim.rootPosition;
            position.y = agent.nextPosition.y;
            transform.position = position;
            
            //transform.position = agent.nextPosition;

            // if (Time.deltaTime > 0)
			// {
			// 	Vector3 velocity = (anim.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

			// 	// we preserve the existing y part of the current velocity.
			// 	// velocity.y = rb.velocity.y;
			// 	// rb.velocity = velocity;
			// }        
        }

        public void SetDestination(Vector3 worldPos)
        {
            agent.isStopped = false;
            agent.destination = worldPos;
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
			// convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction.
			var outString = "";
            
            outString += gameObject.name + " MOVEMENT AND TURN VALUES \n";

            outString += "Movement (Before Magnitude):" + movement + "\n";
			if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }

            outString += "Movement (After Magnitude): " + movement + "\n";

            var localMovement = transform.InverseTransformDirection(movement);

            localMovement = Vector3.ProjectOnPlane(localMovement, groundNormal);
            
            outString += "localMovement: " + localMovement + "\n";

			turnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            forwardAmount = localMovement.z;

            outString += "turnAmount: " + turnAmount + "\n";
            outString += "forwardAmount: " + forwardAmount + "\n";
            // Debug.Log(outString);
        }

        // void Turn()
        // {
        //     Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //     RaycastHit raycastHit;

        //     if(Physics.Raycast(camRay, out raycastHit, 100f))
        //     {
        //         Vector3 playerToMouse = raycastHit.point - transform.position;
        //         playerToMouse.y = 0f;
        //         Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
        //         rb.MoveRotation(newRotation);

        //     }
        // }

        void UpdateAnimator( )
		{
			anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            anim.speed = animSpeedMultiplier;			
		}
#endregion
    }
}