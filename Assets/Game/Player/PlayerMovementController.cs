using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _walkingSound;
        private CharacterController _characterController;
        private Animator _animator;

        [SerializeField]
        private float _speed;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            var movementX = Input.GetAxis("Horizontal");
            var movementY = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3();

            if (movementX != 0)
            {
                movement = new Vector3(0f, 0f, -movementX); //TMP
                if (movementX > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                if (movementX < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            
            if (movementY != 0)
            {
                movement = new Vector3(movementY, 0f, 0); //TMP
                if (movementY > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                if (movementY < 0)
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }

            if (movementX == 0 && movementY == 0)
            {
                _animator.SetBool("IsWalking", false);
            }
            else
            {
                _animator.SetBool("IsWalking", true);
            }

            if (transform.position.y > 0)
            {
                movement = new Vector3(movement.x, -9f, movement.z);
            }
            _characterController.Move(movement * _speed * Time.deltaTime);
        }

        public void PlayWalkingSound()
        {
            _audioSource.clip = _walkingSound;
            _audioSource.Play();
        }
    }
}