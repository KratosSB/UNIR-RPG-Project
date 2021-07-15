using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig weaponConfig = null;
        [SerializeField] private float healthToRestore = 0f;
        [SerializeField] private float respawnTime = 5;
        
        //Triggered when the player enters the trigger of the object to be collected
        //at which point the Pickup function is called.
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        //The item is equipped in the player's corresponding hand or health is added to it
        private void Pickup(GameObject subject)
        {
            if (weaponConfig != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weaponConfig);
            }

            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            //Todo add item pool
            //Destroy(gameObject);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)  //transform return collection of childrens
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            /*
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
                //playerController.GetComponent<Mover>().StartMoveAction(transform.position, 1f);
                //GameObject.FindWithTag("Player").GetComponent<RPG.Movement.Mover>().StartMoveAction(transform.position, 1f);
            }
            return true;
            */
            //Todo - Temporal Fix
            return false;
        }
    }
}
