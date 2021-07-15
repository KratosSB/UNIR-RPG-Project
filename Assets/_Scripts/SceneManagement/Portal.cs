using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIndentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIndentifier destination;

        [SerializeField] private float fadeOutTime = 0.5f;
        [SerializeField] private float fadeInTime = 2f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        
        //starts the scene transition coroutine
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Portal reached!");
                StartCoroutine(Transition());
            }
        }

        //starts the scene change
        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene not set correctly!");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            
            DisableControl();
            
            yield return fader.FadeOut(fadeOutTime);

            savingWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            DisableControl();
            
            Debug.Log("Scene " + sceneToLoad + " loaded");
            
            //Todo - dont load player position, as the position is for another scene and in scene-change the position vector is in SpawnPoint.transform.position 
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            
            EnableControl();

            Destroy(gameObject);
        }
        
        //Disables player control
        private void DisableControl()
        {
            Debug.Log("DisableControl");
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        //Enables player control
        private void EnableControl()
        {
            Debug.Log("EnableControl");
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;
        }
        
        //Moves the player to the spawn point of the new scene and activates him as a navmesh agent
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            //Avoid error on navmesh agent updating position
            NavMeshAgent playerNavMeshAgent = player.GetComponent<NavMeshAgent>();
            //Awake fix -> avoid navmesh not initialized at portals loading
            if (playerNavMeshAgent != null)
            {
                //Todo enable this after disable load position of other scene at portal
                playerNavMeshAgent.enabled = false;
                player.transform.position = otherPortal.spawnPoint.position;
                playerNavMeshAgent.enabled = true;
            }
            else
            {
                player.transform.position = otherPortal.spawnPoint.position;
            }
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        // Search the destination portal where the player should appear
        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }
    }
}
