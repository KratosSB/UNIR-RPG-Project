using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour,IRaycastable
    {
        //Mouse cursor must be of the combat type
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        //Raycast mouse cursor over game space
        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter fighter = callingController.GetComponent<Fighter>();
            if (fighter == null) return false;
            if (!fighter.CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                fighter.Attack(gameObject);
            }

            return true;
        }
    }
}
