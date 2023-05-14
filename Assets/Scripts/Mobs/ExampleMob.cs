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
        private float m_Damage = 4f;
        [SerializeField]
        private float m_Health = 30f;
        [SerializeField] 
        private float m_MobFieldOfViewRadius = 10f;
        [SerializeField] 
        private float m_MobFieldOfHitRadius = 1f;
        [SerializeField] 
        private LayerMask m_PlayerLayerMask;
        [SerializeField] 
        private Transform m_CastPosition;
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

        private PolygonCollider2D m_CapsuleCollider;
        private Rigidbody2D m_RigidBody;
        private GameObject m_PlayerGameObject;
        private bool m_CanSeePlayer;
        private float m_MovingDirection = 1;
        private float m_SameDirectionWalkTimer;
        private float m_RandomTimeOfWalkingInSameDirection;
        private Player m_PlayerScript;
        private MobStats m_mobStats;

        private void Awake()
        {
            m_mobStats = new MobStats(m_Health, m_Damage); 
            m_SameDirectionWalkTimer = 0;
            m_RandomTimeOfWalkingInSameDirection = Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_CapsuleCollider = GetComponent<PolygonCollider2D>();
            StartCoroutine(fovCheck());
            StartCoroutine(playerHitCheck());
        }

        private void Update()
        {
            if (m_mobStats.isDead())
            {
                Destroy(this.gameObject);
            }
            m_SameDirectionWalkTimer += Time.deltaTime;
            if (m_SameDirectionWalkTimer > m_RandomTimeOfWalkingInSameDirection && !getCanSeePlayer())
            {
                StartCoroutine(patrolStopForThink());
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
            float castDistance = m_BaseCastDistance;
            
            if (m_MovingDirection < 0)
            {
                castDistance = -m_BaseCastDistance;
            }

            Vector3 targetPosition = m_CastPosition.position;
            targetPosition.x += castDistance;
            
            Debug.DrawLine(m_CastPosition.position, targetPosition,Color.red);
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

            Vector3 targetPosition = m_CastPosition.position;
            targetPosition.y -= castDistance;
            targetPosition.x += 2 * m_MovingDirection * castDistance;
            Debug.DrawLine(m_CastPosition.position, targetPosition, Color.green);
            if (Physics2D.Linecast(m_CastPosition.position, targetPosition, m_GroundLayerMask))
            {
                isNearEdge = false;
            }

            return isNearEdge;
        }

        private void changeMovingDirection()
        {
            m_RandomTimeOfWalkingInSameDirection = Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);
            m_SameDirectionWalkTimer = 0;
            m_MovingDirection *= -1;
            changeLookingDirection();
        }

        private void changeLookingDirection()
        {
            Vector3 currentScale = transform.localScale;
            if (Math.Abs(Mathf.Sign(currentScale.x) - Mathf.Sign(m_MovingDirection)) > 0)
            {
                transform.localScale = new Vector3(-currentScale.x,currentScale.y,currentScale.z); 
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
            m_RandomTimeOfWalkingInSameDirection = Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);
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
        
        private IEnumerator playerHitCheck()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while (true)
            {
                yield return wait;
                hit();
            }
        }

        private void fov()
        {
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, m_MobFieldOfViewRadius * transform.localScale.y, m_PlayerLayerMask);

            if (rangeCheck.Length > 0)
            {
                Transform target = rangeCheck[0].transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                Debug.DrawRay(transform.position, (directionToTarget * (m_MobFieldOfViewRadius * transform.localScale.y)),Color.red,2);
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

        private void hit()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, m_MobFieldOfHitRadius * transform.localScale.x, m_PlayerLayerMask);
            if (hitPlayer != null)
            {
                m_PlayerGameObject.GetComponent<Player>().getHit(m_Damage);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, m_MobFieldOfViewRadius * transform.localScale.y);
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(transform.position, m_MobFieldOfHitRadius * transform.localScale.y);
        }

        private bool getCanSeePlayer()
        {
            return m_CanSeePlayer;
        }
        
        
        private void setCanSeePlayer(bool i_canSeePlayer)
        {
            m_CanSeePlayer = i_canSeePlayer;
        }
        
    }
}