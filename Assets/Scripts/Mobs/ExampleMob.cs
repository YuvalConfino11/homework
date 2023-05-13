using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mobs
{
    public class ExampleMob : MonoBehaviour
    {
        [SerializeField] 
        private float m_WalkingSpeed = 4f;
        [SerializeField]
        private float m_damage = 4f;
        [SerializeField]
        private float m_health = 30f;
        [SerializeField] 
        private float m_MobFieldOfViewRadius = 10f;
        [SerializeField] 
        private float m_MobFieldOfHitRadius = 1f;
        [SerializeField] 
        private LayerMask m_PlayerLayerMask;
        [SerializeField] 
        private Transform m_castPosition;
        [SerializeField] 
        private float m_baseCastDistance;
        [SerializeField] 
        private LayerMask m_GroundLayerMask;
        [SerializeField] 
        private float maxThinkPauseTime = 2f;
        [SerializeField] 
        private float minThinkPauseTime = 0.5f;
        [SerializeField] 
        private float minWalkingOnSameDirectionTime = 1.5f;
        [SerializeField] 
        private float maxWalkingOnSameDirectionTime = 6f;
        
        private PolygonCollider2D m_CapsuleCollider;
        private Rigidbody2D m_RigidBody;
        private GameObject m_PlayerGameObject;
        private bool m_canSeePlayer;
        private float m_movingDirection = 1;
        private float m_sameDirectionWalkTimer;
        private float m_randomTimeOfWalkingInSameDirection;
        private Player playerScript;


        private void Awake()
        {
            m_sameDirectionWalkTimer = 0;
            m_randomTimeOfWalkingInSameDirection = Random.Range(minWalkingOnSameDirectionTime, maxWalkingOnSameDirectionTime);
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_CapsuleCollider = GetComponent<PolygonCollider2D>();
            StartCoroutine(FOVCheck());
            StartCoroutine(playerHitCheck());
        }

        private void Update()
        {
            m_sameDirectionWalkTimer += Time.deltaTime;
            if (m_sameDirectionWalkTimer > m_randomTimeOfWalkingInSameDirection && !getCanSeePlayer())
            {
                StartCoroutine(patrolStopForThink());
            }
        }
        
        private void FixedUpdate()
        {
            if (!m_canSeePlayer)
            {
                m_RigidBody.velocity = new Vector3(m_movingDirection * m_WalkingSpeed, m_RigidBody.velocity.y);
            }
            else
            {
                m_movingDirection = m_PlayerGameObject.transform.position.x.CompareTo(transform.position.x);
                m_RigidBody.velocity = new Vector2(m_movingDirection * m_WalkingSpeed, m_RigidBody.velocity.y);
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
            float castDistance = m_baseCastDistance;
            
            if (m_movingDirection < 0)
            {
                castDistance = -m_baseCastDistance;
            }

            Vector3 targetPosition = m_castPosition.position;
            targetPosition.x += castDistance;
            
            Debug.DrawLine(m_castPosition.position, targetPosition,Color.red);
            if (Physics2D.Linecast(m_castPosition.position, targetPosition, m_GroundLayerMask))
            {
                isHittingWall = true;
            }

            return isHittingWall;
        }

        private bool isNearEdge()
        {
            bool isNearEdge = true;
            float castDistance = m_baseCastDistance;

            Vector3 targetPosition = m_castPosition.position;
            targetPosition.y -= castDistance;
            targetPosition.x += 2 * m_movingDirection * castDistance;
            Debug.DrawLine(m_castPosition.position, targetPosition, Color.green);
            if (Physics2D.Linecast(m_castPosition.position, targetPosition, m_GroundLayerMask))
            {
                isNearEdge = false;
            }

            return isNearEdge;
        }

        private void changeMovingDirection()
        {
            m_randomTimeOfWalkingInSameDirection = Random.Range(minWalkingOnSameDirectionTime, maxWalkingOnSameDirectionTime);
            m_sameDirectionWalkTimer = 0;
            m_movingDirection *= -1;
            changeLookingDirection();
        }

        private void changeLookingDirection()
        {
            Vector3 currentScale = transform.localScale;
            if (Math.Abs(Mathf.Sign(currentScale.x) - Mathf.Sign(m_movingDirection)) > 0)
            {
                transform.localScale = new Vector3(-currentScale.x,currentScale.y,currentScale.z); 
            }
        }

        private IEnumerator patrolStopForThink()
        {
            m_movingDirection = 0;
            float randomPauseTime = Random.Range(minThinkPauseTime, maxThinkPauseTime);
            yield return new WaitForSeconds(randomPauseTime);

            m_movingDirection = Random.Range(0, 1) > 0.5 ? 1 : -1;
            changeLookingDirection();
            m_sameDirectionWalkTimer = 0;
            m_randomTimeOfWalkingInSameDirection = Random.Range(minWalkingOnSameDirectionTime, maxWalkingOnSameDirectionTime);
        }
        
        private IEnumerator FOVCheck()
        {
           
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            while (true)
            {
                yield return wait;
                FOV();
            }
        }
        
        private IEnumerator playerHitCheck()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while (true)
            {
                yield return wait;
                hit();
            }
        }

        private void FOV()
        {
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, m_MobFieldOfViewRadius * transform.localScale.x, m_PlayerLayerMask);

            if (rangeCheck.Length > 0)
            {
                Transform target = rangeCheck[0].transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                Debug.DrawRay(transform.position, (directionToTarget * (m_MobFieldOfViewRadius * transform.localScale.x)),Color.red,2);
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
            
            else if (m_canSeePlayer)
            {
                setCanSeePlayer(false);
            }
        }

        private void hit()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, m_MobFieldOfHitRadius * transform.localScale.x, m_PlayerLayerMask);
            if (hitPlayer != null)
            {
                m_PlayerGameObject.GetComponent<Player>().getHit(m_damage);
            }
        }
        
        private void OnDrawGizmos()
        {
            if (m_CapsuleCollider != null && transform.position != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, m_MobFieldOfViewRadius * transform.localScale.x);
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(transform.position, m_MobFieldOfHitRadius * transform.localScale.x);
            }
        }

        private bool getCanSeePlayer()
        {
            return m_canSeePlayer;
        }
        
        
        private void setCanSeePlayer(bool i_canSeePlayer)
        {
            m_canSeePlayer = i_canSeePlayer;
        }
        
    }
}