using System;
using UnityEngine;
using UnityEngine.AI;

// TODO: Consider rewiring
using RPG.CameraUI; 
using RPG.Core;

namespace RPG.Characters
{
    
#region REQUIRED COMPONENTS
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
#endregion

    public class PlayerMovement : MonoBehaviour
    {
#region VARIABLES
        [SerializeField] MovementModes movementMode;

        ThirdPersonCharacter player = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint, movement;

        AICharacterControl aiCharacterControl;

        Rigidbody rb = null;

        NavMeshAgent agent = null;

        Animator anim = null;

        bool isInDirectMode = false;

        float speed = 5f;

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

        void FixedUpdate()
        {
            switch (movementMode)
            {
                case MovementModes.DirectGlobal:
                    float h = Input.GetAxis("Horizontal");
                    float v = Input.GetAxis("Vertical");
                    
                    Move(h, v);

                    Turn();

                    Animate(h, v);

                    break;

            }
        }

#endregion

#region CUSTOM METHODS
        void Animate(float h, float v)
        {
            bool walking = h != 0 || v != 0;
            if(walking)
                anim.SetFloat("Forward", 1);
        }
        
        private void InitializeComponents()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            player = GetComponent<ThirdPersonCharacter>();
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            aiCharacterControl = GetComponent<AICharacterControl>();
        }

         void Move(float h, float v)
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime; 
            rb.MovePosition(transform.position + movement);
                    
            Debug.Log("Movement: " + movement);
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.destination = enemy.transform.position; //aiCharacterControl.SetTarget(enemy.transform);
            }
        }

         void OnMouseOverTerrain(Vector3 destination)
        {           
            if(Input.GetMouseButton(0))
            {
                agent.destination = destination;
            }
        }

         private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            
            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 playerMove = v * cameraForward + h * Camera.main.transform.right;

            player.Move(playerMove, false, false);
        }

        private void RegisterObservers()
        {
            if (movementMode == MovementModes.Mouse)
                cameraRaycaster.notifyMouseOverTerrain += OnMouseOverTerrain;

            cameraRaycaster.notifyMouseOverEnemy += OnMouseOverEnemy;
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
#endregion
    }
}