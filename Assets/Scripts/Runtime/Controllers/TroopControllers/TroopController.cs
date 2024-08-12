using Runtime.UI;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Controller
{
    public class TroopController : MonoBehaviour
    {
        public TroopsSO troopData;
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] private Transform radiusPos;
        [SerializeField] private float radius;
        [SerializeField] private string targetTag;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] protected HealthBar healthBar;
        [SerializeField] protected Animator animator;
        
        protected TroopManager source;

        public GameObject highLight;

        public float health { get; set; }
        public float maxHealth { get; protected set; }

        protected float _attackCoolDown, _speed, _damage, _range;
        protected GameObject _target;

        private void Awake()
        {
            maxHealth = troopData.data.health;
            health = maxHealth;
            _speed = agent.speed = troopData.data.speed;
            _attackCoolDown = troopData.data.coolDown;
            _damage = troopData.data.damage;
            _range = troopData.data.attackRange;
            source = FindObjectOfType<TroopManager>();
            UpdateStats();
        }

        public virtual void UpdateStats() { }
        public virtual void AttackedByWizard(float effect) { }

        private void Update()
        {
            Battle();
        }

        private void Battle()
        {
            if (_target == null)
            {
                FindNearestEnemy();
            }

            if (_target != null)
            {
                if (!_target.activeSelf)
                {
                    _target = null;
                    return;
                }
                float distance = Vector3.Distance(this.transform.position, _target.transform.position);
                if (distance > _range)
                {
                    MoveTowardsTarget();
                }
                else
                {
                    Attack();
                }
            }
        }

        public virtual void MoveTowardsTarget()
        {
            _attackCoolDown = troopData.data.coolDown;
            agent.isStopped = false;
            agent.SetDestination(_target.transform.position);
        }

        public virtual void Attack()
        {
            agent.isStopped = true;
            transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            _attackCoolDown -= Time.deltaTime;

            if (CoolDown()) { return; }

            LaunchAttack();
        }

        public virtual void LaunchAttack()
        {
            _attackCoolDown = troopData.data.coolDown;
        }

        public virtual bool CoolDown()
        {
            return _attackCoolDown > 0;
        }

        public virtual void Damage()
        {
            if (_target == null) return;
            if (_target.GetComponent<BuildController>())
            {
                _target.GetComponent<BuildController>().Damage(_damage);
                return;
            }
            _target.GetComponent<TroopController>().health -= _damage;
            _target.GetComponent<TroopController>().health = Mathf.Max(0, _target.GetComponent<TroopController>().health);
            _target.GetComponent<TroopController>().UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (health <= 0)
            {
                Die();
            }
            healthBar.UpdateHealthBar(health, maxHealth);
        }
        protected void FindNearestEnemy()
        {
            Collider[] inRange = Physics.OverlapSphere(radiusPos.position, radius, targetLayer);
            float shortestDistance = float.MaxValue;
            GameObject nearestEnemy = null;
            if (inRange.Length > 0)
            { 
                foreach (Collider enemy in inRange)
                {
                    if (enemy.gameObject.CompareTag(targetTag))
                    {
                        float distance = Vector3.Distance(transform.position, enemy.gameObject.transform.position);
                        if (distance <= shortestDistance)
                        {
                            shortestDistance = distance;
                            nearestEnemy = enemy.gameObject;
                        }
                    }
                }
            }

            if (nearestEnemy != null)
            {
                _target = nearestEnemy;
            }
            else _target = null;
        }

        public void ChangeTarget(GameObject other)
        {
            if (!other.CompareTag(targetTag)) return;
            _target = other;
        }

        public virtual void Die()
        {
            animator.SetTrigger("Die");
        }
        private void OnEnable()
        {
            health = maxHealth;
        }
    }
}
