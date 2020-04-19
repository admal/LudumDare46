using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool Interact(GameObject item);
    bool IsInteractable(GameObject item);
}
