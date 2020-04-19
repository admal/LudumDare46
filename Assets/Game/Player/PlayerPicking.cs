using Game.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Player
{
    public class PlayerPicking : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _pickSound;

        [SerializeField]
        private Transform _pickPoint;

        private GameObject _pickedItem;

        [SerializeField]
        private LayerMask _interactionsMask;
        [SerializeField]
        private float _interactionRadius;

        private InteractionMarker _selectedItem;

        private bool CanPickItem(bool wasHit)
        {
            return _pickedItem == null && wasHit;
        }

        private bool CanInteract(bool wasHit)
        {
            return _pickedItem != null && wasHit;
        }
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        private void Update()
        {
            var origin = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            var ray = new Ray(origin, transform.forward);


            var wasHit = Physics.Raycast(ray, out var hit, _interactionRadius, _interactionsMask);

            if (CanPickItem(wasHit) && _selectedItem == null)
            {
                var pickable = hit.transform.gameObject.GetComponent<IPickable>();
                var selectable = hit.transform.gameObject.GetComponent<InteractionMarker>();
                if (pickable != null && selectable != null && pickable.IsPickable)
                {
                    _selectedItem = selectable;
                    _selectedItem.ShowMarker();
                }
            }
            else if (CanInteract(wasHit) && _selectedItem == null)
            {
                var interactables = hit.transform.gameObject.GetComponents<IInteractable>();
                var interactable = interactables.FirstOrDefault(x => x.IsInteractable(_pickedItem));
                if (interactable != null)
                {
                    var selectable = hit.transform.gameObject.GetComponent<InteractionMarker>();
                    if (selectable != null)
                    {
                        _selectedItem = selectable;
                        _selectedItem.ShowMarker();
                    }
                }
            }
            else if(wasHit == false && _selectedItem != null)
            {
                _selectedItem.HideMarker();
                _selectedItem = null;
            }

            if (CanPickItem(wasHit) && Input.GetButtonUp("Action"))
            {
                var pickable = hit.transform.gameObject.GetComponent<IPickable>();
                if (pickable != null && pickable.IsPickable)
                {
                    PickItem(hit.transform.gameObject);
                }
            }
            else if (CanInteract(wasHit) && Input.GetButtonUp("Action"))
            {
                Debug.Log("Do action");
                var interactables = hit.transform.gameObject.GetComponents<IInteractable>();
                if (interactables.Length > 0)
                {
                    foreach (var interactable in interactables)
                    {
                        var success = interactable.Interact(_pickedItem);
                        if (success)
                        {
                            _pickedItem = null;
                        }
                    }
                }
            }
            else if (_pickedItem != null && Input.GetButtonUp("Action"))
            {
                var position = _pickedItem.transform.position;
                _pickedItem.transform.position = new Vector3(position.x, 0, position.z);
                _pickedItem.transform.SetParent(null);
                _pickedItem.GetComponent<IPickable>().PickDown();
                _pickedItem = null;

                _audioSource.clip = _pickSound;
                _audioSource.Play();
            }

            Debug.DrawRay(origin, transform.forward * _interactionRadius, Color.red);
        }

        private void PickItem(GameObject item)
        {
            Debug.Log("Pick item");
            _pickedItem = item;
            _pickedItem.transform.position = _pickPoint.position;
            _pickedItem.transform.SetParent(_pickPoint);

            _audioSource.clip = _pickSound;
            _audioSource.Play();
        }
    }
}