using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Potato {
    public class IsoMovement : MonoBehaviour {
        private enum MoveState {
            Idle,
            StartMoving,
            IsMoving,
            ContinueMoving,
            EndMoving
        }
        
        public enum FacingDirection {
            PosX,
            PosY,
            NegX,
            NegY
        }

        [SerializeField] private float defaultSpeed = 5;
        [SerializeField] private Grid movementGrid;
        [SerializeField] private Grid tileGrid;
        [SerializeField] private float baseZ;
        [SerializeField] private bool invertNorth;

        [SerializeField] private GridManager gridManager;

        //Temporary sprites
        [Header("Sprites")]
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Sprite posX;
        [SerializeField] private Sprite posY;
        [SerializeField] private Sprite negX;
        [SerializeField] private Sprite negY;


        private Vector3Int currentGridPos;
        private Vector3Int targetGridPos;
        private Vector3Int direction;

        private Vector3 currentWorldPos;
        private Vector3 targetWorldPos;

        private Vector3 moveWorldPos;

        private float x;
        private float y;
        private bool onMoveInput;
        private bool onClickMove;

        public bool openingMachineUI;


        private float journeyLength;
        private float startTime;
        private float distanceCovered;
        private float fractionTraveled;

        MoveState moveState;
        public FacingDirection facingDirection;

        private List<NodeBase> paths;


        //-------------------------------------------------------------------------------------------------

        //private void Awake() {
        //    //gridManager = ServiceLocator.Instance.gridManager;
        //}

        private void Start() {
            currentGridPos = new Vector3Int(0, 0, 0);

            Vector3 gridZero = movementGrid.CellToWorld(currentGridPos);
            gridZero.z = baseZ;

            transform.position = gridZero;
            currentWorldPos = gridZero;

            paths = new List<NodeBase>();

            moveState = MoveState.Idle;
        }


        private void Update() {
            //if (!onClickMove) {

            //    ReadInput();

            //    switch (moveState) {
            //        case MoveState.Idle:
            //            if (onMoveInput) {
            //                StartToMoveKeyboard();
            //                moveState = MoveState.StartMoving;
            //            }
            //            break;


            //        case MoveState.StartMoving:
            //            MoveCharacter(moveState);

            //            if (fractionTraveled >= 1) {
            //                UpdateCurretPosition();
            //                moveState = onMoveInput ? MoveState.IsMoving : MoveState.EndMoving;
            //            }

            //            break;


            //        case MoveState.IsMoving:
            //            if (!onMoveInput)
            //                moveState = MoveState.EndMoving;

            //            MoveCharacter(moveState);

            //            if (fractionTraveled >= 1) {
            //                transform.position = currentWorldPos = moveWorldPos;
            //                StartToMoveKeyboard();
            //                moveState = MoveState.ContinueMoving;
            //            }
            //            break;


            //        case MoveState.ContinueMoving:
            //            MoveCharacter(moveState);

            //            if (fractionTraveled >= 1) {
            //                UpdateCurretPosition();
            //                moveState = onMoveInput ? MoveState.IsMoving : MoveState.EndMoving;
            //            }
            //            break;


            //        case MoveState.EndMoving:
            //            MoveCharacter(moveState);

            //            if (fractionTraveled >= 1) {
            //                transform.position = currentWorldPos = moveWorldPos;
            //                moveState = MoveState.Idle;
            //            }
            //            break;
            //    }
            //}
            //
            {
                // This part handles the auto movement by the pathfinding algorithm

                ReadInput(); // Read Keyboard Input for cancelling auto movement

                switch (moveState) {
                    case MoveState.Idle:
                        if (onClickMove) // Mouse Input
                        {
                            StartToMoveMouse(); // Mouse Input
                            moveState = MoveState.StartMoving;
                        }
                        break;


                    case MoveState.StartMoving:
                        MoveCharacter(moveState);

                        if (fractionTraveled >= 1) {
                            UpdateCurretPosition();
                            moveState = paths.Count > 0 ? MoveState.IsMoving : MoveState.EndMoving; // Mouse Input
                        }

                        break;


                    case MoveState.IsMoving:

                        MoveCharacter(moveState);

                        if (fractionTraveled >= 1) {
                            transform.position = currentWorldPos = moveWorldPos;
                            StartToMoveMouse(); // Mouse Input
                            moveState = MoveState.ContinueMoving;
                        }
                        break;


                    case MoveState.ContinueMoving:
                        MoveCharacter(moveState);

                        if (fractionTraveled >= 1) {
                            UpdateCurretPosition();
                            moveState = paths.Count > 0 ? MoveState.IsMoving : MoveState.EndMoving; // Mouse Input
                        }
                        break;


                    case MoveState.EndMoving:
                        MoveCharacter(moveState);

                        if (fractionTraveled >= 1) {
                            transform.position = currentWorldPos = moveWorldPos;
                            moveState = MoveState.Idle;

                            if (openingMachineUI) {
                                //ServiceLocator.Instance.uiManager.OpenUI();
                                openingMachineUI = false;
                            }

                            onClickMove = false; // Mouse Input
                        }
                        break;

                }
            }
        }



        void SetSprite() {
            switch (facingDirection) {
                case FacingDirection.PosX:
                    sprite.sprite = posX;
                    break;
                case FacingDirection.PosY:
                    sprite.sprite = posY;
                    break;
                case FacingDirection.NegX:
                    sprite.sprite = negX;
                    break;
                case FacingDirection.NegY:
                    sprite.sprite = negY;
                    break;
            }
        }

        float EaseIn(float x) {
            return Mathf.Min(1, x * x);
        }

        float EaseOut(float x) {
            x--;
            if (x >= 0) return 1;
            return Mathf.Min(1, 1 - x * x);
        }

        void ReadInput() {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            if (invertNorth) {
                float temp = x;
                x = y;
                y = -temp;
            }

            if (x != 0 && y != 0) {
                if (!invertNorth) {
                    if (x > 0 && y > 0)
                        y = 0;
                    else if (x > 0 && y < 0)
                        x = 0;
                    else if (x < 0 && y < 0)
                        y = 0;
                    else if (x < 0 && y > 0)
                        x = 0;
                } else {
                    if (x > 0 && y > 0)
                        x = 0;
                    else if (x > 0 && y < 0)
                        y = 0;
                    else if (x < 0 && y < 0)
                        x = 0;
                    else if (x < 0 && y > 0)
                        y = 0;
                }
            }

            direction = new Vector3Int((int)x, (int)y, 0);

            if (x != 0 || y != 0) {
                onMoveInput = true;
                onClickMove = false;
                paths.Clear();
            } else {
                onMoveInput = false;
            }
        }

        void SetTargetWorldPosFromGridPos() {
            targetWorldPos = movementGrid.CellToWorld(targetGridPos);
            targetWorldPos.z = baseZ;
        }

        void UpdateCurretPosition() {
            transform.position = currentWorldPos = moveWorldPos;
            moveWorldPos = targetWorldPos;
            currentGridPos = targetGridPos;
            startTime = Time.time;
        }

        void MoveCharacter(MoveState state) {
            distanceCovered = (Time.time - startTime) * defaultSpeed;

            switch (state) {
                case MoveState.StartMoving:
                    fractionTraveled = EaseIn(distanceCovered / journeyLength);
                    break;

                case MoveState.IsMoving:
                case MoveState.ContinueMoving:
                    distanceCovered *= 2;
                    fractionTraveled = distanceCovered / journeyLength;
                    break;

                case MoveState.EndMoving:
                    fractionTraveled = EaseOut(distanceCovered / journeyLength);
                    break;
            }

            transform.position = Vector3.Lerp(currentWorldPos, moveWorldPos, fractionTraveled);
        }

        void StartToMoveMouse() {
            if (paths.Count > 0) {
                targetGridPos = ((Vector3Int)paths[paths.Count - 1].coord.pos);
                paths.RemoveAt(paths.Count - 1);
            } else {
                Debug.Log("Unreachable");
            }


            SetTargetWorldPosFromGridPos();

            moveWorldPos = Vector3.Lerp(currentWorldPos, targetWorldPos, 0.5f);
            journeyLength = Vector3.Distance(currentWorldPos, targetWorldPos) / 2;

            FacingDirection previousFrameDirection = facingDirection;

            Vector3Int gridDirection = targetGridPos - currentGridPos;
            if (gridDirection.x > 0)
                facingDirection = FacingDirection.PosX;
            else if (gridDirection.y > 0)
                facingDirection = FacingDirection.PosY;
            else if (gridDirection.x < 0)
                facingDirection = FacingDirection.NegX;
            else if (gridDirection.y < 0)
                facingDirection = FacingDirection.NegY;

            if (previousFrameDirection != facingDirection)
                SetSprite();

            startTime = Time.time;
        }

        private void SetPath(Vector3Int targetPath) {
            paths.Clear();
            paths = Pathfinding.FindPath(gridManager.GetGridPosAt(currentGridPos), gridManager.GetGridPosAt(targetPath));
        }

        public void GoHere(Vector3Int destination) {
            bool exist = gridManager.nodes.TryGetValue((Vector2Int)destination, out NodeBase node);
            Debug.Log(node.coord);
            if (!exist) {
                Debug.Log("Outside Area");
            } else if (!node.Walkable) {  // Check if clicked node is walkable
                Debug.Log("Unwalkable");
            } else {     
                SetPath(destination);
                onClickMove = true;
            }
        }

        public void ClickGoHere(Vector3 wpmp) {
            GoHere(tileGrid.WorldToCell(wpmp));
        }


    }
}