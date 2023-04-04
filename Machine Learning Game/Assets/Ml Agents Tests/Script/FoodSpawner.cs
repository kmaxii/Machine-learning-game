using UnityEngine;

namespace Script
{
    public class FoodSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject food;

        public bool HasSpawnedFood => food.activeSelf;
        public Transform GetLastFoodTransform => food.transform;


        private void Start()
        {
           ResetFoodSpawner();
        }

        public void SpawnFood()
        {
            food.SetActive(true);
            
            food.transform.localPosition = new Vector3(Random.Range(-2, +2f), 0, Random.Range(-2, +2));   
        }
        
        public void ResetFoodSpawner()
        {
            food.SetActive(false);
        }
    }
}