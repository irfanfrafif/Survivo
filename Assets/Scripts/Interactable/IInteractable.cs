using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Potato {
    public interface IInteractable {
        void Interact();
    }

    public abstract class Interactable : MonoBehaviour, IInteractable {

        public abstract void Interact();
    }
}