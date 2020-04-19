using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class MoleTrap : MonoBehaviour, IPickable
    {
        [SerializeField]
        private GameObject _flamingLont;
        [SerializeField]
        private GameObject _explosionParticles;
        [SerializeField]
        private GameObject _mesh;
        private AudioSource _audioSource;
        public bool IsPickable { get; private set; } = true;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PickDown()
        {
            IsPickable = false;
            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.layer = 0;
            _flamingLont.SetActive(true);
        }

        private void OnTriggerEnter(Collider collision)
        {
            var mole = collision.gameObject.GetComponent<Mole>();
            if (mole != null)
            {
                _flamingLont.SetActive(false);
                _audioSource.Play();
                _mesh.SetActive(false);
                _explosionParticles.SetActive(true);
                mole.DestroyMole();
                Destroy(this.gameObject, 2f);
            }
        }
    }
}