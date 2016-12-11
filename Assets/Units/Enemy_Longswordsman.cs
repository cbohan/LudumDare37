using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Longswordsman : Base_EnemyUnit
{
    private GameObject longSword;

    private Vector3[] swordPositions;
    private Vector3[] swordRotations;

    public float animSpeed = 2;

    new void Start()
    {
        base.Start();

        //Grab a reference to our longsword.
        this.longSword = this.transform.Find("ProtoUnit").Find("Longsword").gameObject;

        //Define the animation for the sword.
        this.swordPositions = new Vector3[3];
        this.swordRotations = new Vector3[3];

        this.swordPositions[0] = new Vector3(0.154f, 0.267f, 0.045f);
        this.swordRotations[0] = new Vector3(-146.911f, -180f, 0f);

        this.swordPositions[1] = new Vector3(0.1542492f, 0.323f, 0.07785511f);
        this.swordRotations[1] = new Vector3(-127.123f, -180f, -29.37097f);

        this.swordPositions[2] = new Vector3(0.138f, 0.2609999f, 0.1601088f);
        this.swordRotations[2] = new Vector3(-166.003f, -207.285f, -29.37701f);
    }

    new void Update()
    {
        //Find the closest enemy, attack it. Unless we're already attacking someone.
        if ((this.AttackTarget == null ||
            Vector3.Distance(this.transform.position, this.AttackTarget.transform.position) > this.Range) &&
            this.isAggressive)
        { 
            Base_Unit closestUnit = null;
            float closestDistance = float.MaxValue;
            foreach (Base_Unit unit in Base_PlayerUnit.PlayerUnitList)
            {
                if (Vector3.Distance(this.transform.position, unit.transform.position) < closestDistance)
                {
                    closestDistance = Vector3.Distance(this.transform.position, unit.transform.position);
                    closestUnit = unit;
                }
            }

            if (this.AttackTarget != closestUnit)
            {
                this.SetAttackTarget(closestUnit);
            }
        }

        base.Update();

        //Check to see if we're doing an attack animation. These are just hard coded on a unit-by-unit basis.
        if (this.attackAnimFrame >= 0)
        {
            Vector3 position;
            Vector3 rotation;
            float lerpAmount;

            //Whelp, this was the wrong way to do this.
            if (this.attackAnimFrame >= 0 && attackAnimFrame < .25f) //Between frames 1 and 2.
            {
                lerpAmount = attackAnimFrame * 4;
                position = Vector3.Lerp(this.swordPositions[0], this.swordPositions[1], lerpAmount);
                rotation = Vector3.Lerp(this.swordRotations[0], this.swordRotations[1], lerpAmount);
                this.longSword.transform.localPosition = position;
                this.longSword.transform.localEulerAngles = rotation;
                this.attackAnimFrame += Time.deltaTime * animSpeed;
            }
            else if (this.attackAnimFrame >= .25f && attackAnimFrame < .5f) //Between frames 2 and 3.
            {
                lerpAmount = (attackAnimFrame - .25f) * 4;
                position = Vector3.Lerp(this.swordPositions[1], this.swordPositions[2], lerpAmount);
                rotation = Vector3.Lerp(this.swordRotations[1], this.swordRotations[2], lerpAmount);
                this.longSword.transform.localPosition = position;
                this.longSword.transform.localEulerAngles = rotation;
                this.attackAnimFrame += Time.deltaTime * animSpeed;
            }
            else if (this.attackAnimFrame >= .5f && attackAnimFrame < .75f) //Between frames 3 and 1.
            {
                lerpAmount = (attackAnimFrame - .5f) * 4;
                position = Vector3.Lerp(this.swordPositions[2], this.swordPositions[0], lerpAmount);
                rotation = Vector3.Lerp(this.swordRotations[2], this.swordRotations[0], lerpAmount);
                this.longSword.transform.localPosition = position;
                this.longSword.transform.localEulerAngles = rotation;
                this.attackAnimFrame += Time.deltaTime * animSpeed;
            }
            else //The animation is over. Reset everything and do damage to the target. 
            {
                this.attackAnimFrame = -1;
                this.AttackTarget.TakeDamage(this.Attack, this);
            }
        }
        else
        {
            //Make sure our weapon is in its neutral position.
            this.longSword.transform.localPosition = this.swordPositions[0];
            this.longSword.transform.localEulerAngles = this.swordRotations[0];
        }
    }

    protected override void DoAttack()
    {
        this.transform.LookAt(this.AttackTarget.transform);

        //Is this units attack cooldown up?
        if (this.SinceLastAttack >= this.AttackCooldown)
        {
            AudioManager.manger.PlaySword(this.transform.position);
            this.SinceLastAttack = 0;
            this.attackAnimFrame = 0;
        }
    }
}