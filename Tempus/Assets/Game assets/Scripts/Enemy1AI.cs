using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Pathfinding {
    public class Enemy1AI : MonoBehaviour
    {
        // Field of view variables 
        public float viewRadius;
        public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public GameObject targetPlayer;
        public Transform target;

        // Visibility level conditions
        public float awarenessSpeed;
        public float awarenessSlowDown;
        public float selfAwarenessLevelDiscovery = 30;
        public float selfAwarenessLevelMax = 100;
        [SerializeField]
        private float currentAwarenessLevel;
        [SerializeField]
        private float currentAwarenessPercentage;

        [HideInInspector]
        public List<Transform> visibleTargets = new List<Transform>();

        // Pathfinding controls
        public AIPath pathingAIScript;
        public AIDestinationSetter destinationSetterScript;
        public GameObject nextPatrolPoint;
        public GameObject previousPatrolPoint;
        public float minStandingDistance;


        // Idle state controls 
        public float idleTimeMax;
        public float idleRotationAngle;
        public float idleRotationSpeed;
        [SerializeField]
        private Quaternion initialRotation;
        [SerializeField]
        private Quaternion targetRotation;
        [SerializeField]
        private bool setRotation = true;

        private Vector3 pointA;
        private Vector3 pointB;

        public float idleTimerMax;
        [SerializeField]
        private float idleTimer;
        [SerializeField]
        private bool panningDirection = true;

        // Nearest Path point object 
        public float searchRadius;
        public GameObject[] allWayPointObjects;
        bool patrolPointReached = false;
        float nearestWayPointDistance = Mathf.Infinity;

        // Sprite Renderer
        public SpriteRenderer spriteRenderer;
        public float colorChangeSpeed;
        public Color tintedColor;

        // Global events 
        public UnityEvent discoveredEvent;

        // State Variables
        enum EnemyStates
        {
            Idle,
            IdletoPatrol,
            Patrolling,
            Discovering,
            Discovered
        }

        [SerializeField]
        private EnemyStates currentState;
        EnemyStates previousState;
        public float idleReturnTimeCounter;

        // ============== State handler ==================
        void StateHandler()
        {
            switch (currentState)
            {
                case EnemyStates.Idle:
                    IdleState();
                    break;
                case EnemyStates.IdletoPatrol:
                    IdletoPatrollingState();
                    break;
                case EnemyStates.Patrolling:
                    PatrollingState();
                    break;
                case EnemyStates.Discovering:
                    DiscoveringState();
                    break;
                case EnemyStates.Discovered:
                    DiscoveredState();
                    break;
            }
        }

        // ============== State Logic ====================
        void IdleState()
        {
            StateSwitcher();
            // Fix the rotation back and forth.

            if ( idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            }
            if (idleTimer <= 0)
            {
                idleTimer = idleTimeMax;
                currentState = EnemyStates.IdletoPatrol;
            }
        }

        void IdletoPatrollingState()
        {
            StateSwitcher();
            // Search for a nearby patrol point and move to it
            foreach (GameObject obj in allWayPointObjects)
            {
                // Calculate the distance between the player and the GameObject
                float distance = Vector2.Distance(gameObject.transform.position, obj.transform.position);

                // Check if the GameObject is within the radius and closer than the current nearest GameObject
                if (distance < searchRadius && distance < nearestWayPointDistance)
                {
                    nextPatrolPoint = obj;
                    nearestWayPointDistance = distance;
                }
            }

            // Enable navigation system and set the destination.
            destinationSetterScript.target = nextPatrolPoint.transform;
            pathingAIScript.enabled = true;

            // If i have reached an enemy patrol point switch to patrolling state
            if (Vector2.Distance(gameObject.transform.position, nextPatrolPoint.transform.position) <= minStandingDistance)
            {
                currentState = EnemyStates.Patrolling;
            }
        }

        void PatrollingState()
        {
            StateSwitcher();
            // If i am at a patrol point check for the next patrol point
            if (Vector2.Distance(gameObject.transform.position, nextPatrolPoint.transform.position) <= minStandingDistance)
            {
                previousPatrolPoint = nextPatrolPoint;
                WayPoint nextWayPoint = nextPatrolPoint.GetComponent<WayPoint>();
                nextPatrolPoint = nextWayPoint.nextWayPoint;
                destinationSetterScript.target = nextPatrolPoint.transform;
            }
            // Proceed to move to the next patrol point
            pathingAIScript.enabled = true;
        }

        void DiscoveringState()
        {
            // Stop all pathfinding
            pathingAIScript.enabled = false;
            StateSwitcher();
            // (gameObject.GetComponent("AIPath") as MonoBehaviour).enabled = false;

            // TODO Face the player while awarenes is at a certain level 
            
            // If awareness level drops switch back to the previous state
            if (currentAwarenessLevel < selfAwarenessLevelDiscovery)
            {
                currentState = previousState;
            }

        }

        void DiscoveredState()
        {
            // Trigger unity event for game over.
            // discoveredEvent.Invoke();
        }

        // ============== State Switcher for Discovery and Discovered ==================
        void StateSwitcher()
        {
            if (currentState != EnemyStates.Discovering && currentAwarenessLevel > selfAwarenessLevelDiscovery)
            {
                previousState = currentState;
                currentState = EnemyStates.Discovering;
            }
            if (currentState != EnemyStates.Discovered && currentAwarenessLevel > selfAwarenessLevelMax)
            {
                currentState = EnemyStates.Discovered;
            }
        }


        // ============== Field of view ==================
        void checkForPlayerVisibility()
        {
            Vector2 targetPosition = target.transform.position;
            Vector2 toTarget = (target.position - gameObject.transform.position).normalized;
            float angle = Vector2.Angle(gameObject.transform.up, toTarget);

            if ((Vector2.Distance(transform.position, targetPosition) < viewRadius) && (angle < viewAngle / 2))
            {
                // Debug.Log(target.gameObject + "is within the radius");
                if (currentAwarenessLevel <= 100)currentAwarenessLevel += awarenessSpeed * Time.deltaTime;
            }
        }



        private void OnDrawGizmos()
        {
            // Draw the view radius
            Handles.color = Color.white;
            Handles.DrawWireArc(gameObject.transform.position, Vector3.forward, Vector3.right, 360, viewRadius);

            Vector3 viewAngleA = new Vector3(Mathf.Sin((-viewAngle / 2 - gameObject.transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((-viewAngle / 2 - gameObject.transform.eulerAngles.z) * Mathf.Deg2Rad), 0);
            Vector3 viewAngleB = new Vector3(Mathf.Sin((viewAngle / 2 - gameObject.transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((viewAngle / 2 - gameObject.transform.eulerAngles.z) * Mathf.Deg2Rad), 0);

            Handles.DrawLine(gameObject.transform.position, gameObject.transform.position + viewAngleA * viewRadius);
            Handles.DrawLine(gameObject.transform.position, gameObject.transform.position + viewAngleB * viewRadius);
        }


        // ============= Start ===============
        void Start()
        {
            currentState = EnemyStates.Idle;
            currentAwarenessLevel = selfAwarenessLevelMax - selfAwarenessLevelMax; // Redundant code but i like it should just equal 0
            allWayPointObjects = GameObject.FindGameObjectsWithTag("Waypoints");
            idleTimer = idleTimeMax;
        }

        // ============= Update =================
        void Update()
        {
            StateHandler();
            if (currentAwarenessLevel > 0 && currentAwarenessLevel < 100) currentAwarenessLevel -= awarenessSlowDown * Time.deltaTime;

            // Calculate the tint color based on the awareness level
            
            // TODO Tint the enemy Character red based on the enemy awarenessLevel

            checkForPlayerVisibility();
        }
    }
}
