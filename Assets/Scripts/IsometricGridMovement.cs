using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricGridMovement : MonoBehaviour
{
    enum MoveState
    {
        idle,
        startMoving,
        isMoving,
        continueMoving,
        endMoving
    }

    public enum FacingDirection
    {
        positiveX,
        positiveY,
        negativeX,
        negativeY
    }

    [SerializeField] float defaultSpeed;
    [SerializeField] float speed => defaultSpeed; //* ServiceLocator.Instance.dayManager.moveSpeedModifier; // Speed Modifier
    [SerializeField] Grid movementGrid;
    [SerializeField] Grid tileGrid;
    [SerializeField] float baseZ;
    [SerializeField] bool invertNorth;

    [SerializeField] GridManager gridManager;

    Vector3Int currentGridPos;
    Vector3Int targetGridPos;

    Vector3Int direction;

    Vector3 currentWorldPos;
    Vector3 targetWorldPos;

    Vector3 moveWorldPos; 

    float x;
    float y;
    bool onMoveInput;
    bool onClickMove;

    public bool openingMachineUI;


    float journeyLength;
    float startTime;
    float distanceCovered;
    float fractionTraveled;

    MoveState moveState;
    public FacingDirection facingDirection;

    List<NodeBase> paths;

    //Temporary sprites
    public SpriteRenderer sprite;
    public Sprite posX;
    public Sprite posY;
    public Sprite negX;
    public Sprite negY;

    void SetSprite()
    {
        switch(facingDirection)
        {
            case FacingDirection.positiveX:
                sprite.sprite = posX;
                break;
            case FacingDirection.positiveY:
                sprite.sprite = posY;
                break;
            case FacingDirection.negativeX:
                sprite.sprite = negX;
                break;
            case FacingDirection.negativeY:
                sprite.sprite = negY;
                break;
        }
    }

    float EaseInQuad(float x)
    {
        if (x >= 1) return 1;
        return 1 - Mathf.Cos((x * Mathf.PI) / 2);
    }

    float EaseOutQuad(float x)
    {
        if (x >= 1) return 1;
        return Mathf.Sin((x * Mathf.PI) / 2);
    }

    void ReadInput()
    {       
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (invertNorth)
        {
            float temp = x;
            x = y;
            y = -temp;
        }

        if (x != 0 && y != 0)
        {
            if(!invertNorth)
            {
                if (x > 0 && y > 0) y = 0;
                else if (x > 0 && y < 0) x = 0;
                else if (x < 0 && y < 0) y = 0;
                else if (x < 0 && y > 0) x = 0;
            }
            else
            {
                if (x > 0 && y > 0) x = 0;
                else if (x > 0 && y < 0) y = 0;
                else if (x < 0 && y < 0) x = 0;
                else if (x < 0 && y > 0) y = 0;
            }
        }

        direction = new Vector3Int((int)x, (int)y, 0);

        if(x != 0 || y != 0)
        {
            onMoveInput = true;
            onClickMove = false;
            paths.Clear();
        }
        else
        {
            onMoveInput = false;
        }
    }

    void SetTargetWorldPosFromGridPos()
    {
        targetWorldPos = movementGrid.CellToWorld(targetGridPos);
        targetWorldPos.z = baseZ;
    }

    void UpdateCurretPosition()
    {
        transform.position = currentWorldPos = moveWorldPos;
        moveWorldPos = targetWorldPos;
        currentGridPos = targetGridPos;
        startTime = Time.time;
    }

    void MoveCharacter(MoveState state)
    {
        distanceCovered = (Time.time - startTime) * speed;

        switch (state)
        {            
            case MoveState.startMoving:
                fractionTraveled = EaseInQuad(distanceCovered / journeyLength);
                break;

            case MoveState.isMoving:
            case MoveState.continueMoving:
                distanceCovered *= 2;
                fractionTraveled = distanceCovered / journeyLength;
                break;

            case MoveState.endMoving:
                fractionTraveled = EaseOutQuad(distanceCovered / journeyLength);
                break;
        }

        transform.position = Vector3.Lerp(currentWorldPos, moveWorldPos, fractionTraveled);
    }

    void StartToMoveKeyboard()
    {
        targetGridPos = currentGridPos + direction;

        FacingDirection previousFrameDirection = facingDirection;

        Vector3Int gridDirection = targetGridPos - currentGridPos;
        if (gridDirection.x > 0) facingDirection = FacingDirection.positiveX;
        else if (gridDirection.y > 0) facingDirection = FacingDirection.positiveY;
        else if (gridDirection.x < 0) facingDirection = FacingDirection.negativeX;
        else if (gridDirection.y < 0) facingDirection = FacingDirection.negativeY;

        if (previousFrameDirection != facingDirection) SetSprite();

        if (!gridManager.nodes.ContainsKey(((Vector2Int)targetGridPos)) || !gridManager.nodes[((Vector2Int)targetGridPos)].Walkable)
        {
            targetGridPos = currentGridPos;
        }

        SetTargetWorldPosFromGridPos();

        moveWorldPos = Vector3.Lerp(currentWorldPos, targetWorldPos, 0.5f);
        journeyLength = Vector3.Distance(currentWorldPos, targetWorldPos) / 2;

        startTime = Time.time;
    }

    void StartToMoveMouse()
    {
        if(paths.Count > 0)
        {
            targetGridPos = ((Vector3Int)paths.Last().coord.pos);
            paths.RemoveAt(paths.Count - 1);           
        }
        else
        {
            Debug.Log("Unreachable");
        }
        

        SetTargetWorldPosFromGridPos();

        moveWorldPos = Vector3.Lerp(currentWorldPos, targetWorldPos, 0.5f);
        journeyLength = Vector3.Distance(currentWorldPos, targetWorldPos) / 2;

        FacingDirection previousFrameDirection = facingDirection;

        Vector3Int gridDirection = targetGridPos - currentGridPos;
        if (gridDirection.x > 0) facingDirection = FacingDirection.positiveX;
        else if (gridDirection.y > 0) facingDirection = FacingDirection.positiveY;
        else if (gridDirection.x < 0) facingDirection = FacingDirection.negativeX;
        else if (gridDirection.y < 0) facingDirection = FacingDirection.negativeY;

        if (previousFrameDirection != facingDirection) SetSprite();

        startTime = Time.time;
    }

    private Vector3Int GetMouseGridPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = tileGrid.WorldToCell(mousePos);

        return gridPos;
    }

    private void SetPath(Vector3Int targetPath)
    {
        paths.Clear();
        paths = Pathfinding.FindPath(gridManager.GetGridPosAt(currentGridPos), gridManager.GetGridPosAt(targetPath));
    }

    public void GoHere(Vector3Int destination)
    {
        // Check if clicked node exist / inside determined area in Grid Manager
        if (!gridManager.nodes.ContainsKey((Vector2Int)destination)) Debug.Log("Outside Area");

        // Check if clicked node is walkable
        else if (!gridManager.nodes[(Vector2Int)destination].Walkable) Debug.Log("Unwalkable");

        // else player can click the node (but not necessarily able to go there)          
        else
        {
            SetPath(destination);
            onClickMove = true;
        }
    }

    public void GoHere(Vector3Int destination, bool isClickingmachine)
    {
        //if(isClickingmachine)
        //{
        //    openingMachineUI = true;
        //}
        GoHere(destination);
    }

    // This handles the "click on the ground on the screen and the character will move there automatically" behaviour
    public void ClickGoHere()
    {
        //openingMachineUI = false;
        GoHere(GetMouseGridPos());
    }

    private void Awake()
    {
        gridManager = ServiceLocator.Instance.gridManager;
    }

    private void Start()
    {
        currentGridPos = new Vector3Int(0, 0, 0);

        Vector3 gridZero = movementGrid.CellToWorld(currentGridPos);
        gridZero.z = baseZ;

        transform.position = gridZero;
        currentWorldPos = gridZero;

        paths = new List<NodeBase>();

        moveState = MoveState.idle;
    }

    private void Update()
    {
        if(!onClickMove)
        {
            // This part handles the movement with keyboard input

            //if (!ServiceLocator.Instance.uiManager.isOpeningMenu) // BRUH pain, this is horrible, too bad!
            //{
            //    ReadInput();

            //    openingMachineUI = false;
            //}

            ReadInput();


            switch (moveState)
            {
                case MoveState.idle:
                    if (onMoveInput)
                    {
                        StartToMoveKeyboard();
                        moveState = MoveState.startMoving;
                    }
                    break;


                case MoveState.startMoving:
                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        UpdateCurretPosition();
                        moveState = onMoveInput ? MoveState.isMoving : MoveState.endMoving;
                    }

                    break;


                case MoveState.isMoving:
                    if (!onMoveInput) moveState = MoveState.endMoving;

                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        transform.position = currentWorldPos = moveWorldPos;
                        StartToMoveKeyboard();
                        moveState = MoveState.continueMoving;
                    }
                    break;


                case MoveState.continueMoving:
                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        UpdateCurretPosition();
                        moveState = onMoveInput ? MoveState.isMoving : MoveState.endMoving;
                    }
                    break;


                case MoveState.endMoving:
                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        transform.position = currentWorldPos = moveWorldPos;
                        moveState = MoveState.idle;
                    }
                    break;
            }
        }

        else
        {
            // This part handles the auto movement by the pathfinding algorithm

            ReadInput(); // Read Keyboard Input for cancelling auto movement

            switch (moveState)
            {
                case MoveState.idle:
                    if (onClickMove) // Mouse Input
                    {
                        StartToMoveMouse(); // Mouse Input
                        moveState = MoveState.startMoving;
                    }
                    break;


                case MoveState.startMoving:
                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        UpdateCurretPosition();
                        moveState = paths.Count > 0 ? MoveState.isMoving : MoveState.endMoving; // Mouse Input
                    }

                    break;


                case MoveState.isMoving:

                    // Unused in Mouse Input
                    // if (!onMoveInput) moveState = MoveState.endMoving; 

                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        transform.position = currentWorldPos = moveWorldPos;
                        StartToMoveMouse(); // Mouse Input
                        moveState = MoveState.continueMoving;
                    }
                    break;


                case MoveState.continueMoving:
                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        UpdateCurretPosition();
                        moveState = paths.Count > 0 ? MoveState.isMoving : MoveState.endMoving; // Mouse Input
                    }
                    break;


                case MoveState.endMoving:
                    MoveCharacter(moveState);

                    if (fractionTraveled >= 1)
                    {
                        transform.position = currentWorldPos = moveWorldPos;
                        moveState = MoveState.idle;

                        if(openingMachineUI)
                        {
                            //ServiceLocator.Instance.uiManager.OpenUI();
                            openingMachineUI = false;
                        }

                        onClickMove = false; // Mouse Input
                    }
                    break;

            }
        }   
    }
}
