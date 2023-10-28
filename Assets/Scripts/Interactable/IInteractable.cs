using System;
using UnityEngine;

namespace Potato {

    public abstract class Interactable : MonoBehaviour {

        public void Interact() {
            InteractLogic();
        }

        protected abstract void InteractLogic();

    }

}