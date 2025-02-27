using UnityEngine;

namespace Platformer.Scripts
{
    public class LavaScript : MonoBehaviour
    {

        public delegate void PlayerHitLava();

        public static event PlayerHitLava OnPlayerHitLava;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Mario"))
            {
                Destroy(other.gameObject);
                OnPlayerHitLava?.Invoke();
            }
        }
    }
}
