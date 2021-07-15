using System;
using RPG.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private GameObject levelUpParticleEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;

        public event Action OnLevelUp;
        
        private LazyValue<int> currentLevel;
        private Experience _experience;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        //Subscribe and unsubscribe (observer pattern) the Experience's onExperiencieGained event
        //to the UpdateLevel function.
        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained -= UpdateLevel;
            }
        }
        
        //Calculate the player's experience level and level it up if applicable.
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                Debug.Log("Level Up!");
                LevelUpEffect();
                OnLevelUp();
            }
        }

        //Instantiates special effect when leveling up.
        private void LevelUpEffect()
        {
            //Todo object pooling
            Instantiate(levelUpParticleEffect, transform);
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }
        
        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1+GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetAdditiveModifiers(stat))
                {
                    total += modifiers;
                }
            }
            
            return total;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentajeModifiers(stat))
                {
                    total += modifiers;
                }
            }
            
            return total;
        }

        //Calculates the experience level the character is in based on the experience gained.
        public int CalculateLevel()
        {
            if (_experience == null) return startingLevel; 
                
            float currentXP = _experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevel = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevel > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel;
        }
    }
}