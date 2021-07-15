using System.Collections;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeTime = 1f;
        private const string defaultSaveFile = "save";

        private void Awake()
        {
            //Fix load scene -> awake ok, start ko
            StartCoroutine(LoadLastScene());
        }
        
        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutInmediate();
            yield return fader.FadeIn(fadeTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Switching to main menu...");
                SceneManager.LoadScene(0);
            }
        }

        public void Load()
        {
            Debug.Log("Loading!");
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            Debug.Log("Saving!");
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            Debug.Log("Deleting!");
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
