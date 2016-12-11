using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_EnemyUnit : Base_Unit
{
    public static List<Base_EnemyUnit> EnemyUnitList = new List<Base_EnemyUnit>();

    new protected void Start()
    {
        base.Start();

        //Enemies can't be controlled by the player.
        this.IsPlayerControlled = false;

        //Add this unit to the list of all the enemy's units.
        Base_EnemyUnit.EnemyUnitList.Add(this);

        //Tell the unit that it wants to move to where it is.
        this.MoveTarget = new Vector2(this.transform.position.x, this.transform.position.z);
        this.IsAttacking = false;
    }

    //Enemy AI (archers are special so they will override this)
    new protected void Update()
    {
        base.Update();
    }
}