using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeaponConfig = null;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Animator _animator;

        private WeaponConfig currentWeaponConfig;
        private LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();

            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponConfig);
        }

        // calculates the elapsed time between attacks
        // checks if the target is within attack range
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if (target == null) return;
            
            if (target.IsDead) return;
            
            if (!GetIsInRange(target.transform))
            {
                _mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }
        
        // Instantiates and activates the specified weapon in the character's corresponding hand
        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            currentWeapon.value = AttachWeapon(weaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        // Starts the attack routine, rotating the attacker towards the target and playing the animations        private void AttackBehaviour()
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //Will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        //Animation Event Assets/Asset Packs/Animations/Unarmed/RPG-Character@Unarmed-Attack-L3.FBX
        // Executed based on an animation event
        // Calculates the damage done by the weapon
        // Triggers the OnHit event
        // If it's a ranged weapon, the projectile is fired. If not, the target is damaged
        private void Hit()
        {
            //ToDo Optimize this
            Debug.Log("Hit event!");
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        //Animation Event Assets/Asset Packs/Animations by Explosive/2Hand-Bow/RPG-Character@2Hand-Bow-Attack1.FBX
        private void Shoot()
        {
            Debug.Log("Shoot event!");
            Hit();
        }
        
        // If the target is within attack range.
        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
        }

        // If the target can be attacked
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!_mover.CanMoveTo(combatTarget.transform.position) && (GetIsInRange(combatTarget.transform)))
                return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        // Starts the attack action
        public void Attack(GameObject combatTarget)
        {
            Debug.Log("Attacking!");
            _actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        // Cancels the attack action
        public void Cancel()
        {
            StopAttack();
            target = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

        // Gets the damage and damage multiplier of the weapon from its configuration
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }

        // Gets the damage and damage multiplier of the weapon from its configuration
        public IEnumerable<float> GetPercentajeModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponPercentageBonus();
            }
        }

        public Health GetTarget()
        {
            return target;
        }
        
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            WeaponConfig weaponConfig = UnityEngine.Resources.Load<WeaponConfig>((string) state);
            EquipWeapon(weaponConfig);
        }
    }
}