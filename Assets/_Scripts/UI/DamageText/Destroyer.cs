using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroy = null;

        public void DestroyTarget()
        {
            //Todo object pooling
            Destroy(targetToDestroy);
        }
    }
}