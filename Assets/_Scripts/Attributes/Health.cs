using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenPercentage = 70f;
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;
        
        //Fix SerializeField not handling generics UnityEvent<float> -> TakeDamageEvent
        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        private Animator _animator;
        private ActionScheduler _actionScheduler;
        //private BaseStats baseStats;
        
        private LazyValue<float> _healthPoints;
        private bool isDead = false;

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        //It subscribes and unsubscribes to the OnLevelUp event of the BaseStats class
        //to the RegenHealth function
        
        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenHealth;
        }

        public bool IsDead
        {
            get => isDead;
        }

        //Applies the corresponding damage to health. If it is 0 or less, invoke the death event
        //and add the experience that corresponds to the cause of the damage,
        //which can be the player or a npc
        public void TakeDamage(GameObject instigator ,float damage)
        {
            Debug.Log(gameObject.name + " damaged: " + damage + " health " + _healthPoints);
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
            if (!isDead) takeDamage.Invoke(damage);
            if (_healthPoints.value <= 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }

        //Reestablish health points
        public void Heal(float healthToRestore)
        {
            Debug.Log(gameObject.name + " healed " + healthToRestore);
            _healthPoints.value = Mathf.Min(_healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }
        
        public float GetHealthPoints()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        
        public float GetFraction()
        {
            return _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        
        //Sets the state of death, not playable, and triggers the corresponding animation
        private void Die()
        {
            if (isDead) return;
            isDead = true;
            _animator.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenHealth()
        {
            float regenHealtPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenPercentage / 100);
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealtPoints);
        }
        
        public object CaptureState()
        {
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            _healthPoints.value = (float) state;
            //This will trigger the dead animation -> insert bool parameter?
            if (_healthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}
