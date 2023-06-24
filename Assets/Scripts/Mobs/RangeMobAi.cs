using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs {
    public class RangeMobAi : MonoBehaviour
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
        [SerializeField]
        private MobAnimation m_MobAnimation;
        [SerializeField]
        private float m_GroundCastDistance = 10f;
        [SerializeField]
        private bool m_MonsterGrowled;
        [SerializeField]
        private bool m_EnemyCanShoot;
        [SerializeField]
        private float m_TimeBetweenProjetiles;
        [SerializeField]
        private GameObject m_EnemyProj;
        [SerializeField]
        private Transform m_EnemyProjPos;
        [SerializeField]
        private float m_MobAttackAnimationDuration;

        private Rigidbody2D m_RigidBody;
        private GameObject m_PlayerGameObject;
        private bool m_CanSeePlayer;
        private float m_MovingDirection;
        private float m_SameDirectionWalkTimer;
        private float m_RandomTimeOfWalkingInSameDirection;
        private RaycastHit2D m_RaycastHit;
        private bool m_EncounterObstacle;
        private float m_Timer;




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
            Vector3 rayStartPosition = new Vector3(m_CastPosition.position.x, m_CastPosition.position.y, 0);
            m_RaycastHit = Physics2D.Raycast(rayStartPosition, Vector2.down, m_GroundCastDistance, m_GroundLayerMask);
            m_Grounded = m_RaycastHit.collider != null;
            if (m_SameDirectionWalkTimer >= m_RandomTimeOfWalkingInSameDirection && !getCanSeePlayer())
            {
                StartCoroutine(patrolStopForThink());
            }
            if (!getIsGrounded() && m_RigidBody.velocity.y > 0)
            {
                m_FeetBoxCollider2D.enabled = false;
            }
            if (m_RigidBody.velocity.y <= 0)
            {
                m_FeetBoxCollider2D.enabled = true;
            }
            m_MobAnimation.PlayMobAnimation(m_RigidBody.velocity.x);
            MakeSound();
            m_Timer += Time.deltaTime;
            if (m_Timer > m_TimeBetweenProjetiles)
            {
                m_EnemyCanShoot = true;
                m_Timer = 0;
                
            }
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
            targetPosition.x += m_MovingDirection * m_BaseCastDistance;

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
            m_EncounterObstacle = false;
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
                if (m_EnemyCanShoot)
                {
                    Shoot();
                    m_EnemyCanShoot = false;
                }


                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, m_GroundLayerMask).collider == null)
                {
                    m_PlayerGameObject = target.gameObject;
                    m_EncounterObstacle = !m_EncounterObstacle ? isHittingWall() || isNearEdge() : m_EncounterObstacle;
                    setCanSeePlayer(!m_EncounterObstacle);
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


        private void setCanSeePlayer(bool i_CanSeePlayer)
        {
            m_CanSeePlayer = i_CanSeePlayer;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(m_CastPosition.position, m_MobFieldOfViewRadius);

            Gizmos.color = Color.cyan;
            Vector3 nearEdgeTargetPosition = m_FeetBoxCollider2D.transform.position;
            Vector3 nearEdgeStartPosition = m_CastPosition.position;
            nearEdgeTargetPosition.y -= m_BaseCastDistance * 2f;
            nearEdgeTargetPosition.x += m_BaseCastDistance * m_MovingDirection * 1.25f;
            Gizmos.DrawLine(nearEdgeStartPosition, nearEdgeTargetPosition);
            Vector3 hitWallStartPosition = m_CastPosition.position;
            Vector3 hitWallTargetPosition = m_CastPosition.position;
            hitWallTargetPosition.x += m_MovingDirection * m_BaseCastDistance;
            Gizmos.DrawLine(hitWallStartPosition, hitWallTargetPosition);

            Gizmos.color = Color.green;
            Vector3 groundedPosition = m_CastPosition.position;
            Vector3 groundedTargetPosition = groundedPosition;
            groundedTargetPosition.y -= m_GroundCastDistance;
            Gizmos.DrawLine(groundedPosition, groundedTargetPosition);

        }

        private bool getIsGrounded()
        {
            return m_Grounded;
        }
        //// omer after adding sound add in parentasis of the function string i_ audio which will get the required audio
        private void MakeSound()
        {
            Collider2D playerInRangeCheck = Physics2D.OverlapCircle(transform.position, m_MobFieldOfViewRadius * transform.localScale.y, m_PlayerLayerMask);
            if (playerInRangeCheck != null)
            {
                if (m_MonsterGrowled == false)
                {
                    //AudioManager.Instance.PlaySFX(i_audio);
                    m_MonsterGrowled = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                m_MonsterGrowled = false;
            }
        }
        private void Shoot()
        {
            m_MobAnimation.PlayMobAttackAnimation();
            StartCoroutine(attackAnimationOff());
        }
        private IEnumerator attackAnimationOff()
        {
            yield return new WaitForSeconds(m_MobAttackAnimationDuration);
            m_MobAnimation.StopMobAttackAnimation();
            Instantiate(m_EnemyProj, m_EnemyProjPos.position, Quaternion.identity);
        }
    }
}
