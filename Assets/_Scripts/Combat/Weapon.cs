using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private UnityEvent onHit;
        
        //Attack impact moment
        public void OnHit()
        {
            Debug.Log("Weapon Hit " + gameObject.name);
            onHit.Invoke();
        }
    }
}