using System.Collections;
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
        [SerializeField]
        private BoxCollider2D m_FeetBoxCollider2D;
        [SerializeField]
        private Transform m_CastPosition;

        [SerializeField] private MobAnimation m_MobAnimation;
        
        private Rigidbody2D m_RigidBody;
        private GameObject m_PlayerGameObject;
        private bool m_CanSeePlayer;
        private float m_MovingDirection;
        private float m_SameDirectionWalkTimer;
        private float m_RandomTimeOfWalkingInSameDirection;
        private RaycastHit2D  m_RaycastHit;
        private bool m_encounterObstacle;
        


        private void Awake()
        {
            m_SameDirectionWalkTimer = 0f;
            m_RandomTimeOfWalkingInSameDirection =
                Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_FeetBoxCollider2D = m_FeetBoxCollider2D == null ? GetComponent<BoxCollider2D>() : m_FeetBoxCollider2D;
            m_CastPosition = m_CastPosition == null ? transform.GetChild(0).transform : m_CastPosition;
            m_PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            m_MovingDirection = Mathf.Sign(transform.localScale.x);
            StartCoroutine(fovCheck());
        }

        private void Update()
        {
            m_SameDirectionWalkTimer += Time.deltaTime;
            Vector3 rayStartPosition =
                new Vector3(transform.position.x + 0.5f * m_MovingDirection, transform.position.y, 0);
            m_RaycastHit = Physics2D.Raycast(rayStartPosition, Vector2.down, transform.localScale.y * 2.25f,m_GroundLayerMask);
            m_Grounded = m_RaycastHit.collider != null;
            if (m_SameDirectionWalkTimer >= m_RandomTimeOfWalkingInSameDirection && !getCanSeePlayer())
            {
                StartCoroutine(patrolStopForThink());
            }
            if (!GetIsGrounded() && m_RigidBody.velocity.y > 0)
            {
                m_FeetBoxCollider2D.enabled = false;
            }
            if (m_RigidBody.velocity.y <= 0)
            {
                m_FeetBoxCollider2D.enabled = true;
            }
            m_MobAnimation.PlayMobAnimation(m_RigidBody.velocity.x);
        }

        private void FixedUpdate()
        {
            if (isHittingWall() || isNearEdge())
            {
                StartCoroutine(obstacleEncounterHandler());
            }
            
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
        }

        private bool isHittingWall()
        {
            Vector3 targetPosition = m_CastPosition.position;
            
            targetPosition.x += m_MovingDirection * m_FeetBoxCollider2D.size.x * m_BaseCastDistance * 1.25f;
           
            return Physics2D.Linecast(m_CastPosition.position, targetPosition, m_GroundLayerMask).collider != null;
        }

        private bool isNearEdge()
        {
            float castDistance = m_BaseCastDistance;
            Vector3 targetPosition = m_FeetBoxCollider2D.transform.position;
            Vector3 startPosition = m_CastPosition.position;
            
            startPosition.y -= transform.localScale.y;
            targetPosition.y -= castDistance * 2f;
            targetPosition.x += castDistance * m_MovingDirection * 0.5f;
            
            return Physics2D.Linecast(startPosition, targetPosition, m_GroundLayerMask).collider == null;
        }

        private void changeMovingDirection()
        {
            m_RandomTimeOfWalkingInSameDirection =
                Random.Range(m_MinWalkingOnSameDirectionTime, m_MaxWalkingOnSameDirectionTime);

            m_SameDirectionWalkTimer = 0;
            m_MovingDirection *= -1;
            changeLookingDirection();
        }

        private IEnumerator obstacleEncounterHandler()
        {
            changeMovingDirection();
            yield return new WaitForSeconds(m_RandomTimeOfWalkingInSameDirection);
            m_encounterObstacle = false;
        }

        
        private void changeLookingDirection()
        {
            Vector3 currentScale = transform.localScale;
            if (!Mathf.Sign(currentScale.x).Equals(Mathf.Sign(m_MovingDirection)))
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
            Collider2D playerInRangeCheck = Physics2D.OverlapCircle(transform.position,
                m_MobFieldOfViewRadius * transform.localScale.y, m_PlayerLayerMask);

            if (playerInRangeCheck != null)
            {
                Transform target = playerInRangeCheck.transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                Debug.DrawRay(transform.position,
                    (directionToTarget * (m_MobFieldOfViewRadius * transform.localScale.y)), Color.red, 1);

                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, m_GroundLayerMask).collider == null)
                {
                    m_PlayerGameObject = target.gameObject;
                    m_encounterObstacle = !m_encounterObstacle ? isHittingWall() || isNearEdge() : m_encounterObstacle;
                    setCanSeePlayer(!m_encounterObstacle);
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
            float castDistance = m_BaseCastDistance;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_MobFieldOfViewRadius * transform.localScale.y);
            
            Gizmos.color = Color.green;
            
            Vector3 startPosition = m_CastPosition.position;
            startPosition.y -= transform.localScale.y;
            
            Vector3 targetPosition = m_FeetBoxCollider2D.transform.position;
            targetPosition.y -= castDistance * 2f;
            targetPosition.x += castDistance * m_MovingDirection * 0.5f;
            
            Gizmos.DrawLine(startPosition, targetPosition);
            
            Gizmos.color = Color.magenta;
            targetPosition = m_CastPosition.position;
            targetPosition.x += m_MovingDirection * m_FeetBoxCollider2D.size.x * m_BaseCastDistance * 1.25f;
            Gizmos.DrawLine(m_CastPosition.position, targetPosition);
            
            Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
            startPosition = new Vector3(transform.position.x + 0.5f * m_MovingDirection, transform.position.y, 0);
            targetPosition = startPosition;
            targetPosition.y -= transform.localScale.y * 2.25f;
            Gizmos.DrawLine(m_CastPosition.position, targetPosition);
        }
        
        private bool GetIsGrounded()
        {
            return m_Grounded;
        }
        
    }
}