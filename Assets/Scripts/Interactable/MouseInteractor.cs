using System;
using UnityEngine;

namespace Potato {
    public class MouseInteractor : MonoBehaviour {

        private Camera cam;
        private bool clicked;
        [SerializeField] private IsoMovement move;

        private void Awake() {
            cam = Camera.main;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Debug.Log("click");
                DoSomething();
            }
        }

        private void DoSomething() {
            Vector3 wpmp = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(wpmp, Vector2.zero);

            if (hit.transform) {
                var i = hit.transform.GetComponent<Interactable>();
                if (i) { i.Interact(); }
            } else {
                move.ClickGoHere(wpmp);
            }
        }

    }
}