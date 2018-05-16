using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

// TODO: Consider rewiring
using RPG.CameraUI; 
using RPG.Core;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        //[SerializeField] float walkMoveStopRadius = 0.2f;
        //[SerializeField] float attackMoveStopRadius = 5f;

        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        [SerializeField] MovementModes movementMode;

        //[SerializeField] private TransformVariable playerTransform;

        ThirdPersonCharacter player = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint, movement;

        Rigidbody rb = null;

        NavMeshAgent agent = null;

        Animator anim = null;

        bool isInDirectMode = false;

        float speed = 5f;

        void Awake()
        {
            
        }

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            player = GetComponent<ThirdPersonCharacter>();

            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();

            anim = GetComponent<Animator>();

            // mouse click event subscriber, only if in mouse movement mode
            if(movementMode == MovementModes.Mouse)
                cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;

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

        void Move(float h, float v)
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime; 
            rb.MovePosition(transform.position + movement);
                    
            Debug.Log("Movement: " + movement);
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

        void Animate(float h, float v)
        {
            bool walking = h != 0 || v != 0;
            if(walking)
                anim.SetFloat("Forward", 1);
        }


        void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            // new movement based on event
            
            
            switch(layerHit)
            {
                case enemyLayerNumber:
                    agent.destination = raycastHit.point;
                    break;

                case walkableLayerNumber:         
                    agent.destination = raycastHit.point;
                    break;

                default:
                    Debug.LogError("Don't know how to handle the mouse click for playermovement.");
                    break;
            }
        }

        private Vector3 ShortDestination(Vector3 destination, float shortening)
        {
            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
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
    }
}