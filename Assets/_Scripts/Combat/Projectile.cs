using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyedOnHit = null;
        [SerializeField] private float lifeAfterImpact = 0.2f;
        [SerializeField] private UnityEvent onHit;

        private Health target = null;
        private GameObject instigator = null;
        private CapsuleCollider _targetCapsule;
        private float damage = 0f;
        
        private void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        
        //Sets the target of the projectile and rotate its position to aim at it.
        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            _targetCapsule = target.GetComponent<CapsuleCollider>();
            transform.LookAt(GetAimLocation());
            this.damage = damage;
            this.instigator = instigator;
            //Todo object pooling
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (_targetCapsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * _targetCapsule.height / 2;
        }

        //Projectile enters a trigger of a gameobject susceptible to being hit by it
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead) return;
            target.TakeDamage(instigator, damage);

            speed = 0;

            onHit.Invoke();
            
            //Todo object pool
            //Todo destroy lost arrows (corroutine?)
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyedOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
