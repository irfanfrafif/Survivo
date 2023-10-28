using UnityEngine;

namespace Potato {
    public class MouseInteractor : MonoBehaviour {

        private Camera cam;
        [SerializeField] private PlayerMain player;

        private void Awake() {
            cam = Camera.main;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                DoSomething();
            }
        }

        private void DoSomething() {
            Vector2 wpmp = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(wpmp, Vector2.zero);

            if (hit) { Debug.Log("HELLO"); } 

            if (hit) {
                var i = hit.transform.GetComponent<Interactable>();
                if (i) {
                    i.WaitForInteract(player.Move.OnArrived);
                    var o = i as ObjectDialogue;
                    Debug.Log(o.GetFrontGrid());
                    if (o != null) { player.Move.GoHere((Vector3Int)o.GetFrontGrid()); }
                }
            } else {
                player.Move.ClickGoHere(wpmp);
            }
        }

    }
}