using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Item : MonoBehaviour
    {
        public int pickUpDamage;
        public int dropDamage;

        public string info;

        [SerializeField] private string targetTag;

        public static event Action OnWrongBox;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                OnWrongBox?.Invoke();
            }
        }
    }
}