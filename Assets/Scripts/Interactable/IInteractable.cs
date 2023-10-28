using System;
using UnityEngine;

namespace Potato {

    public abstract class Interactable : MonoBehaviour {

        private Action callback;

        public void WaitForInteract(Action onPlayerArrived) {
            callback = onPlayerArrived;
            callback += Interact;
        }

        private void Interact() {
            InteractLogic();
            callback -= Interact;
        }

        protected abstract void InteractLogic();

    }
}