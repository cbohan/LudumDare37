using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Unit : MonoBehaviour
{
    //Base stats for each unit type
    public static float ARCHER_BASE_HEALTH = 15;
    public static float ARCHER_BASE_ATTACK = 3;
    public static float ARCHER_BASE_BLOCK = 0;
    public static float ARCHER_BASE_RANGE = 5;
    public static float ARCHER_BASE_ATTACK_COOLDOWN = 2;

    public static float LONGSWORDSMAN_BASE_HEALTH = 25;
    public static float LONGSWORDSMAN_BASE_ATTACK = 4;
    public static float LONGSWORDSMAN_BASE_BLOCK = 2;
    public static float LONGSWORDSMAN_BASE_RANGE = .4f;
    public static float LONGSWORDSMAN_BASE_ATTACK_COOLDOWN = 2;

    public static float SHORTSWORDSMAN_BASE_HEALTH = 30;
    public static float SHORTSWORDSMAN_BASE_ATTACK = 2;
    public static float SHORTSWORDSMAN_BASE_BLOCK = 5;
    public static float SHORTSWORDSMAN_BASE_RANGE = .4f;
    public static float SHORTSWORDSMAN_BASE_ATTACK_COOLDOWN = 2;

    //These are the unit's stats.
    public float Health;
    public float MaxHealth;
    public float Attack;
    public float Block;
    public float Range;
    public float AttackCooldown;

    //Experience is just passed through from the unit model in to the unit model out
    public int Experience;

    //Just for enemy units but I want to alter it in the TakeDamage method so its going here.
    //This dictates whether enemies attack first or not.
    //Only false for the first level.
    public bool isAggressive;

    //Has this unit been marked for destruction?
    private bool markedToDestroy = false;

    //How long has it been since the unit last attacked?
    protected float SinceLastAttack;

    //Where are we in the attack animation? -1 indicates we're not currently animating.
    protected float attackAnimFrame = -1;

    //These inform the unit's AI
    public bool IsAttacking { get; protected set; } //Is this unit moving or attacking right now?
    public Vector2 MoveTarget { get; protected set; }
    public bool ReachedMoveTarget { get; protected set; } //We've reach the target no need to keep trying to get there.
    public Base_Unit AttackTarget { get; protected set; }

    //Health bar texture which is displayed above the units head.
    public Texture HealthBarTexture;
    private GameObject healthBarPosition;

    //Who controls this unit
    public bool IsPlayerControlled { get; protected set; }

    //Single white pixel
    private Texture2D whiteTexture;

    //A list of every unit
    public static List<Base_Unit> unitList = new List<Base_Unit>();

    protected void Start()
    {
        //Get the position of our health bar marker
        this.healthBarPosition = this.transform.Find("HealthBarPosition").gameObject;

        //Create the white texture for our selection box.
        this.whiteTexture = new Texture2D(1, 1);
        this.whiteTexture.SetPixel(0, 0, Color.white);
        this.whiteTexture.Apply();

        //Add this unit to the unit list.
        Base_Unit.unitList.Add(this);
    }

    protected void Update()
    {
        //Do a mock collision detection so units don't intersect with one another.
        DoUnitCollisionDetection();

        //Perform the active AI action.
        if (this.IsAttacking)
            DoAttackAI();
        else
            DoMove();

        //Track time since last attack.
        this.SinceLastAttack += Time.deltaTime;
    }

    //Draw the health bar texture aboce the units head.
    void OnGUI()
    {
        //Make sure we have a health bar position game object.
        if (healthBarPosition == null)
            return;

        //Turn 3d position into 2d position
        Vector3 drawPosition = Camera.main.WorldToScreenPoint(this.healthBarPosition.transform.position);
        drawPosition.y = Screen.height - drawPosition.y;

        //Draw health bar
        if (this.IsPlayerControlled)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        float width = 60 * (this.Health / this.MaxHealth);
        Rect fillRect = new Rect(drawPosition.x - 30, drawPosition.y - 15, width, 8);
        GUI.DrawTexture(fillRect, this.whiteTexture);
        GUI.color = Color.white;

        //Draw health bar container
        Rect containerRect = new Rect(drawPosition.x - 30, drawPosition.y - 15, 60, 8);
        GUI.DrawTexture(containerRect, this.HealthBarTexture);
    }

    //Get into range of the target then attack.
    protected void DoAttackAI()
    {
        //We need to have an attack target to attack.
        if (AttackTarget == null)
            return;

        //Is this unit too far from the enemy to attack it?
        if (Vector3.Distance(this.transform.position, AttackTarget.transform.position) > this.Range)
        {
            //Move towards it.
            this.ReachedMoveTarget = false;
            MoveTarget = ExtractXZ(AttackTarget.transform);
            DoMove();
        }
        else
        {
            DoAttack();
        }
    }

    //This has to be defined on a unit-by-unit basis.
    protected abstract void DoAttack();

    //Move to the specified location
    protected void DoMove()
    {
        //Have we already reached our target?
        if (this.ReachedMoveTarget)
            return;

        Vector2 position2D = new Vector2(this.transform.position.x, this.transform.position.z);
        if (Vector2.Distance(position2D, MoveTarget) < .01f)
        {
            this.ReachedMoveTarget = true;
            return;
        }

        //Look towards our target.
        this.transform.LookAt(new Vector3(MoveTarget.x, this.transform.position.y, MoveTarget.y));

        //Move forward.
        float movement = 1 * Time.deltaTime;
        //Ensure that we don't overshoot the target.
        if (movement > Vector2.Distance(position2D, MoveTarget))
            movement = Vector2.Distance(position2D, MoveTarget);

        this.transform.Translate(new Vector3(0, 0, movement), Space.Self);
    }

    private void DoUnitCollisionDetection()
    {
        //variables to tune collision detection.
        float tooClose = .3f;
        float pushStrength = .02f;

        //check this unit against every other unit
        foreach (Base_Unit unit in Base_Unit.unitList)
        {
            //Make sure not to test against ourself.
            if (unit != this)
            {
                //Are we too close?
                if (Vector3.Distance(this.transform.position, unit.transform.position) < tooClose)
                {
                    //Push us away from the other guy.
                    Vector3 awayVector = this.transform.position - unit.transform.position;
                    this.transform.Translate(awayVector * (pushStrength / (Vector3.Magnitude(awayVector) + 1)), Space.World);
                }
            }
        }
    }

    //Extract x/z position from transform
    private Vector2 ExtractXZ(Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.z);
    }

    //Remove some health from the unit. Do some stuff it it dies.
    public void TakeDamage(float damage, Base_Unit attacker)
    {
        this.isAggressive = true;

        //Calculate the actual damage.
        float modifiedDamage = damage;
        modifiedDamage *= 1 - (this.Block / (10 + this.Block));

        this.Health -= modifiedDamage;

        //If the unit isn't attacking anyone and he's in range he might as well attack the guy who is attacking him.
        if (this.AttackTarget == null && this.ReachedMoveTarget && attacker != null)
        {
            if (Vector3.Distance(attacker.transform.position, this.transform.position) <= this.Range)
                this.SetAttackTarget(attacker);
        }

        //Mistah kurtz he dead.
        if (this.Health <= 0 && this.markedToDestroy == false)
        {
            AudioManager.manger.PlayDie(this.transform.position);

            this.markedToDestroy = true; 
            this.Health = 0;

            //Destroy all the children of this game object.
            //Code from: https://forum.unity3d.com/threads/deleting-all-chidlren-of-an-object.92827/
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));

            //Destoy this game object.
            Destroy(this.gameObject);

            //Remove it from the list of all units.
            Base_Unit.unitList.Remove(this);

            if (this is Base_PlayerUnit)
            {
                PersistentData.UnitsLost++;
                if (Base_PlayerUnit.PlayerUnitList.Contains((Base_PlayerUnit)this))
                    Base_PlayerUnit.PlayerUnitList.Remove((Base_PlayerUnit)this);
            }

            if (this is Base_EnemyUnit)
            {
                PersistentData.EnemiesKilled++;
                if (Base_EnemyUnit.EnemyUnitList.Contains((Base_EnemyUnit)this))
                    Base_EnemyUnit.EnemyUnitList.Remove((Base_EnemyUnit)this);
            }
        }
    }

    //Tell the unit where to go.
    public void SetMovePoint(Vector2 dest)
    {
        this.IsAttacking = false;
        this.MoveTarget = dest;
        this.ReachedMoveTarget = false;
    }

    //Tell the unit who to attack.
    public void SetAttackTarget(Base_Unit enemy)
    {
        this.IsAttacking = true;
        this.AttackTarget = enemy;
    }
}
