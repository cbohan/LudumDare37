using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_PlayerUnit : Base_Unit
{
    public static List<Base_PlayerUnit> PlayerUnitList = new List<Base_PlayerUnit>();

    private GameObject selectionCircle;

    new protected void Start()
    { 
        base.Start();

        this.IsPlayerControlled = true;

        //Get a reference to the selection circle under the unit and turn it off.
        selectionCircle = this.transform.Find("SelectionCircle").gameObject;
        SetUnitSelected(false);

        //Add this unit to the list of all the player's units.
        Base_PlayerUnit.PlayerUnitList.Add(this);

        //Tell the unit that it wants to move to where it is.
        this.MoveTarget = new Vector2(this.transform.position.x, this.transform.position.z);
        this.IsAttacking = false;
    }

    //Turn on/off the selection circle for this unit
    public void SetUnitSelected(bool selected)
    {
        selectionCircle.SetActive(selected);
    }
}
