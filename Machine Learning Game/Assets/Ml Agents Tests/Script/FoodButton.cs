using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class FoodButton : MonoBehaviour
    {
        [SerializeField] private Material unusedMaterial;
        [SerializeField] private Material triggerdMaterial;

        private MeshRenderer _meshRenderer;

        private bool _canUseButton;

        public bool CanUseButton { get => _canUseButton;}

        [SerializeField] private FoodSpawner foodSpawner;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _canUseButton = true;
        }


        private void Start()
        {
            ResetButton();
        }


        public void UseButton()
        {
            if (!_canUseButton)return;

            _meshRenderer.material = triggerdMaterial;

            transform.localScale = new Vector3(.5f, .2f, .5f);
            _canUseButton = false;
            foodSpawner.SpawnFood();


        }

        public void ResetButton()
        {
            _meshRenderer.material = unusedMaterial;
            transform.localScale = new Vector3(.5f, .5f, .5f);
            
            transform.localPosition = new Vector3(Random.Range(-2, +2f), 0, Random.Range(-2, +2));

            _canUseButton = true;
        }
    }
}