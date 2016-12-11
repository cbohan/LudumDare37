using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shortswordsman : Base_PlayerUnit
{
    private GameObject shortSword;

    private Vector3[] swordPositions;
    private Vector3[] swordRotations;

    public float animSpeed = 2;

    new void Start()
    {
        base.Start();

        //grab a reference to the shortsword
        this.shortSword = this.transform.Find("ProtoUnit").Find("Shortsword").gameObject;

        //Define the animation for the sword.
        this.swordPositions = new Vector3[3];
        this.swordRotations = new Vector3[3];

        this.swordPositions[0] = new Vector3(0.2472f, 0f, -0.057f);
        this.swordRotations[0] = new Vector3(-90.00001f, 0f, 0f);

        this.swordPositions[1] = new Vector3(0.2472f, 0.058f, -0.2846552f);
        this.swordRotations[1] = new Vector3(-61.355f, 0f, 0f);

        this.swordPositions[2] = new Vector3(0.2857f, 0.09200001f, -0.1257f);
        this.swordRotations[2] = new Vector3(-61.355f, -12.011f, 0f);
    }

    new void Update()
    {
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
                this.shortSword.transform.localPosition = position;
                this.shortSword.transform.localEulerAngles = rotation;
                this.attackAnimFrame += Time.deltaTime * animSpeed;
            }
            else if (this.attackAnimFrame >= .25f && attackAnimFrame < .5f) //Between frames 2 and 3.
            {
                lerpAmount = (attackAnimFrame - .25f) * 4;
                position = Vector3.Lerp(this.swordPositions[1], this.swordPositions[2], lerpAmount);
                rotation = Vector3.Lerp(this.swordRotations[1], this.swordRotations[2], lerpAmount);
                this.shortSword.transform.localPosition = position;
                this.shortSword.transform.localEulerAngles = rotation;
                this.attackAnimFrame += Time.deltaTime * animSpeed;
            }
            else if (this.attackAnimFrame >= .5f && attackAnimFrame < .75f) //Between frames 3 and 1.
            {
                lerpAmount = (attackAnimFrame - .5f) * 4;
                position = Vector3.Lerp(this.swordPositions[2], this.swordPositions[0], lerpAmount);
                rotation = Vector3.Lerp(this.swordRotations[2], this.swordRotations[0], lerpAmount);
                this.shortSword.transform.localPosition = position;
                this.shortSword.transform.localEulerAngles = rotation;
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
            this.shortSword.transform.localPosition = this.swordPositions[0];
            this.shortSword.transform.localEulerAngles = this.swordRotations[0];
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
