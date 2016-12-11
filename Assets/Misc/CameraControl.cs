/*
 RTS mouse selection tutorial I followed: http://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //How fast does the camera move when using the WASD keys?
    public float cameraPanSpeed = 8;

    //Texture used to indicate the mouse selection area
    private Texture2D whiteTexture;

    //Rectangular selection variables.
    private bool isSelecting = false;
    private Vector3 selectStartPosition;
    private List<Base_PlayerUnit> selectedUnits;

    //What things can we right click on?
    public LayerMask rightClickMask;

    void Start()
    {
        //Initialize the selected unit list.
        this.selectedUnits = new List<Base_PlayerUnit>();

        //Create the white texture for our selection box.
        this.whiteTexture = new Texture2D(1, 1);
        this.whiteTexture.SetPixel(0, 0, Color.white);
        this.whiteTexture.Apply();
    }

	void Update ()
    {
        WASDMovement();
        MouseSelection();
        if (Input.GetMouseButtonDown(1))
            MouseRightClick();
    }

    //Draw selection box if there is one.
    void OnGUI()
    {
        //Make sure we're currently selecting before drawing a box.
        if (this.isSelecting)
        {
            Rect rect = GetScreenRect(this.selectStartPosition, Input.mousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //WASD panning
    private void WASDMovement()
    {
        Vector2 cameraMoveVector = new Vector2();

        if (Input.GetKey(KeyCode.A))
            cameraMoveVector.y--;
        if (Input.GetKey(KeyCode.D))
            cameraMoveVector.y++;

        if (Input.GetKey(KeyCode.W))
            cameraMoveVector.x--;
        if (Input.GetKey(KeyCode.S))
            cameraMoveVector.x++;

        cameraMoveVector.Normalize();
        cameraMoveVector = cameraMoveVector * cameraPanSpeed * Time.deltaTime;

        this.transform.Translate(cameraMoveVector.x, 0, cameraMoveVector.y, Space.World);

        //Clamp the camera within the bounds of the arena.
        float clampedX = Mathf.Clamp(this.transform.position.x, -2, 8);
        float clampedZ = Mathf.Clamp(this.transform.position.z, -6, 6);
        this.transform.position = new Vector3(clampedX, this.transform.position.y, clampedZ);
    }

    //Select units with the mouse.
    private void MouseSelection()
    {
        //Start selecting
        if (Input.GetMouseButtonDown(0))
        {
            this.isSelecting = true;
            this.selectStartPosition = Input.mousePosition;
        }

        //Stop selecting
        if (Input.GetMouseButtonUp(0))
        {
            this.isSelecting = false;

            //We're done selecting now. Should we box-select or click-select?
            Vector3 mouseMovement = Input.mousePosition - this.selectStartPosition;

            //Remove dead units from lists
            this.selectedUnits.RemoveAll(unit => unit == null);
            Base_PlayerUnit.PlayerUnitList.RemoveAll(unit => unit == null);

            //Do selection
            if (mouseMovement.magnitude > 10)
                BoxSelect();
            else
                ClickSelect();
        }
    }

    //Issue commands to selected units.
    private void MouseRightClick()
    {
        //Let's remove all the dead units first.
        selectedUnits.RemoveAll(unit => unit == null);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 99999, rightClickMask))
        {
            //Figure out what we hit.
            if (hit.transform.gameObject.name == "Arena1Floor")
            {
                //We hit the floor so this is a move command.
                MoveCommand(hit);
            }
            else
            {
                //We hit an enemy unit so this is an attack command.
                AttackCommand(hit);
            }  
        }
    }

    //Tell the selected units to move.
    private void MoveCommand(RaycastHit hit)
    {
        //Move command.
        //We want the units to form a line facing away from the direction the came from. 
        Vector2 newMoveTarget = new Vector2(hit.point.x, hit.point.z);

        //First we get the average move vector.
        Vector2 moveDirection = Vector2.zero;
        foreach (Base_PlayerUnit unit in this.selectedUnits)
        {
            Vector2 unitPosition = new Vector2(unit.transform.position.x, unit.transform.position.z);
            moveDirection += (newMoveTarget - unitPosition);
        }
        moveDirection.Normalize();
        //In the extremely unlikely case where we're moving the units to the middle of all
        //of them, we'll face them right.
        if (moveDirection.magnitude < .5f)
            moveDirection = new Vector3(0, 0, 1);

        //We want the units to line up at a right angle to their movement. 
        Vector2 lineUpDirection = new Vector2(-moveDirection.y, moveDirection.x);
        //1 unit spacing is too big. We'll make it smaller.
        lineUpDirection *= .4f;

        //Next we move them.
        float i = 0;
        float listMiddle = ((float)this.selectedUnits.Count - 1.0f) / 2.0f;
        foreach (Base_PlayerUnit unit in this.selectedUnits)
        {
            float offset = i - listMiddle;

            unit.SetMovePoint(newMoveTarget + (offset * lineUpDirection));
            i++;
        }
    }

    //Tell the selected units to attack the target that the user just clicked.
    private void AttackCommand(RaycastHit hit)
    {
        //make sure the hit wasn't null or not an enemy
        if (hit.transform.gameObject == null)
            return;
        if (hit.transform.gameObject.GetComponent<Base_Unit>() == null)
            return;

        foreach (Base_PlayerUnit unit in this.selectedUnits)
        {
            unit.SetAttackTarget(hit.transform.gameObject.GetComponent<Base_Unit>());
        }
    }

    //Convert screen space selection rectangle to pixels.
    private Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        //Set origin
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;

        //Get top-left and bottom-right corners
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        return new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
    } 

    //Draw a rectangle on the screen.
    //Helper function for mouse selection
    private void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, this.whiteTexture);
        GUI.color = Color.white;
    }

    //Draw a rectangular border on the screen.
    //Helper function for the mouse selection.
    private void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    //Figure out what units were in our selection box.
    private void BoxSelect()
    {
        Bounds bounds = GetSelectionBounds(this.selectStartPosition, Input.mousePosition);

        //If we're holding shift we want to add units to the current list. Otherwise, we want 
        //to make a completely new list.
        if (Input.GetKey(KeyCode.LeftShift) == false)
            this.selectedUnits.Clear();

        foreach (Base_PlayerUnit unit in Base_PlayerUnit.PlayerUnitList)
        {
            //Is this unit in the selection box?
            if (bounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position)))
            {
                //Make sure the unit is not already in the list before we add it.
                if (this.selectedUnits.Contains(unit) == false)
                    this.selectedUnits.Add(unit);
            }

            //Turn on/off the selection circle for this unit.
            unit.SetUnitSelected(this.selectedUnits.Contains(unit));
        }
    }

    //Figure out which unit is under the mouse
    private void ClickSelect()
    {
        //If we're holding shift we want to add units to the current list. Otherwise, we want 
        //to make a completely new list.
        if (Input.GetKey(KeyCode.LeftShift) == false)
            this.selectedUnits.Clear();

        //Perform a raycast. It should only be able to hit the ground and the player's units.
        //Because of this we shouldn't need layers. If the ray can hit a unit it will.
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //Did we hit one of the player's units?
            if (hit.transform.gameObject.GetComponent<Base_PlayerUnit>() != null)
                this.selectedUnits.Add(hit.transform.gameObject.GetComponent<Base_PlayerUnit>());
        }

        //Now we need to iterate through all the player's units and tell them if they're selected
        foreach (Base_PlayerUnit unit in Base_PlayerUnit.PlayerUnitList)
        {
            if (this.selectedUnits.Contains(unit))
                unit.SetUnitSelected(true);
            else
                unit.SetUnitSelected(false);
        }
    }

    //Get bounds of the selection box.
    private Bounds GetSelectionBounds(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        //Define the min and max points of the selection box in viewport space.
        Vector3 point1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        Vector3 point2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        Vector3 minPoint = Vector3.Min(point1, point2);
        Vector3 maxPoint = Vector3.Max(point1, point2);
        minPoint.z = Camera.main.nearClipPlane;
        maxPoint.z = Camera.main.farClipPlane;

        //Create the bound box.
        Bounds bounds = new Bounds();
        bounds.SetMinMax(minPoint, maxPoint);
        return bounds;
    }
}
