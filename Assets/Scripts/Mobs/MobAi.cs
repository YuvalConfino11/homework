using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mobs
{
    public class MobAi : MonoBehaviour
    {
        [SerializeField] 
        private float m_MobFieldOfViewRadius = 10f;
        [SerializeField] 
        private LayerMask m_PlayerLayerMask;
        [SerializeField] 
        private float m_BaseCastDistance;
        [SerializeField] 
        private LayerMask m_GroundLayerMask;
        [SerializeField] 
        private float m_MaxThinkPauseTime = 2f;
        [SerializeField] 
        private float m_MinThinkPauseTime = 0.5f;
        [SerializeField] 
        private float m_MinWalkingOnSameDirectionTime = 1.5f;
        [SerializeField] 
        private float m_MaxWalkingOnSameDirectionTime = 6f;
        [SerializeField] 
        private float m_WalkingSpeed = 4f;
        [SerializeField]
        private bool m_Grounded = true;

        private Rigidbody2D m_RigidBody;
        private GameObject m_PlayerGameObject;
        private Transform m_CastPosition;
        private bool m_CanSeePlayer;
        private float m_MovingDirection = -1;
        private float m_SameDirectionWalkTimer;
        private float m_RandomTimeOfWalkingInSameDirection;
        private RaycastHit2D  m_raycastHit;
        private BoxCollider2D m_feetBoxCollider2D;
        

        private void Awake()
        {
            m_SameDirectionWalkTimer = 0f;
            m_RandomTimeOfWalkingInSameDirection =
                Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_feetBoxCollider2D = GetComponent<BoxCollider2D>();
            m_CastPosition = transform.GetChild(0).transform;
            StartCoroutine(fovCheck());
        }

        private void Update()
        {
            m_SameDirectionWalkTimer += Time.deltaTime;
            Vector3 rayStartPosition =
                new Vector3(transform.position.x + 0.5f * m_MovingDirection, transform.position.y, 0);
            m_raycastHit = Physics2D.Raycast(rayStartPosition, Vector2.down, transform.localScale.y * 2.25f,m_GroundLayerMask);
            m_Grounded = m_raycastHit.collider != null;
            if (m_SameDirectionWalkTimer > m_RandomTimeOfWalkingInSameDirection && !getCanSeePlayer())
            {
                StartCoroutine(patrolStopForThink());
            }
            if (!GetIsGrounded() && m_RigidBody.velocity.y > 0)
            {
                m_feetBoxCollider2D.enabled = false;
            }
            if (m_RigidBody.velocity.y <= 0)
            {
                m_feetBoxCollider2D.enabled = true;
            }
        }

        private void FixedUpdate()
        {
            if (!getCanSeePlayer())
            {
                m_RigidBody.velocity = new Vector3(m_MovingDirection * m_WalkingSpeed, m_RigidBody.velocity.y);
            }
            else
            {
                m_MovingDirection = m_PlayerGameObject.transform.position.x.CompareTo(transform.position.x);
                m_RigidBody.velocity = new Vector2(m_MovingDirection * m_WalkingSpeed, m_RigidBody.velocity.y);
                changeLookingDirection();

            }

            if (isHittingWall() || isNearEdge())
            {
                changeMovingDirection();
            }
        }

        private bool isHittingWall()
        {
            bool isHittingWall = false;
            Vector3 targetPosition = m_CastPosition.position;
            targetPosition.x += m_MovingDirection * m_feetBoxCollider2D.size.x * m_BaseCastDistance;

            Debug.DrawLine(m_CastPosition.position, targetPosition, Color.red);
            if (Physics2D.Linecast(m_CastPosition.position, targetPosition, m_GroundLayerMask))
            {
                isHittingWall = true;
            }

            return isHittingWall;
        }

        private bool isNearEdge()
        {
            bool isNearEdge = true;
            float castDistance = m_BaseCastDistance;
            Vector3 targetPosition = m_feetBoxCollider2D.transform.position;
            targetPosition.y -= castDistance * 2f;
            targetPosition.x += castDistance * m_MovingDirection * 0.5f;
            Vector3 startPosition = m_CastPosition.position;
            startPosition.y -= transform.localScale.y * 1.5f;
            Debug.DrawLine(startPosition,targetPosition, Color.magenta);
            if (Physics2D.Linecast(m_CastPosition.position, targetPosition, m_GroundLayerMask))
            {
                isNearEdge = false;
            }

            return isNearEdge;
        }

        private void changeMovingDirection()
        {
            m_RandomTimeOfWalkingInSameDirection =
                Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);

            m_SameDirectionWalkTimer = 0;
            m_MovingDirection *= -1;
            changeLookingDirection();
        }

        private void changeLookingDirection()
        {
            Vector3 currentScale = transform.localScale;
            if (Math.Abs(Mathf.Sign(currentScale.x) - Mathf.Sign(m_MovingDirection)) > 0)
            {
                transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);
            }
        }

        private IEnumerator patrolStopForThink()
        {
            m_MovingDirection = 0;
            float randomPauseTime = Random.Range(m_MinThinkPauseTime, m_MaxThinkPauseTime);
            yield return new WaitForSeconds(randomPauseTime);

            m_MovingDirection = Random.Range(0, 1) > 0.5 ? 1 : -1;
            changeLookingDirection();
            m_SameDirectionWalkTimer = 0;
            m_RandomTimeOfWalkingInSameDirection =
                Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);
        }

        private IEnumerator fovCheck()
        {

            WaitForSeconds wait = new WaitForSeconds(0.2f);
            while (true)
            {
                yield return wait;

                fov();
            }
        }

        private void fov()
        {
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position,
                m_MobFieldOfViewRadius * transform.localScale.y, m_PlayerLayerMask);

            if (rangeCheck.Length > 0)
            {
                Transform target = rangeCheck[0].transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                Debug.DrawRay(transform.position,
                    (directionToTarget * (m_MobFieldOfViewRadius * transform.localScale.y)), Color.red, 2);

                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, m_GroundLayerMask))
                {
                    setCanSeePlayer(true);
                    m_PlayerGameObject = target.gameObject;
                }
                else
                {
                    setCanSeePlayer(false);
                }
            }

            else if (m_CanSeePlayer)
            {
                setCanSeePlayer(false);
            }
        }

        private bool getCanSeePlayer()
        {
            return m_CanSeePlayer;
        }


        private void setCanSeePlayer(bool i_canSeePlayer)
        {
            m_CanSeePlayer = i_canSeePlayer;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_MobFieldOfViewRadius * transform.localScale.y);
            Gizmos.color = Color.cyan;
            Vector3 rayStartPosition =
                new Vector3(transform.position.x + 0.5f * m_MovingDirection, transform.position.y, 0);
            Gizmos.DrawRay(rayStartPosition,new Vector3(0,-1 * transform.localScale.y * 2.25f,0));
        }
        
        private bool GetIsGrounded()
        {
            return m_Grounded;
        }
        
    }
}