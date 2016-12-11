using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Archer : Base_PlayerUnit
{
    public GameObject arrow;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    protected override void DoAttack()
    {
        this.transform.LookAt(this.AttackTarget.transform);

        //Is this units attack cooldown up?
        if (this.SinceLastAttack >= this.AttackCooldown)
        {
            AudioManager.manger.PlayShoot(this.transform.position);

            this.SinceLastAttack = 0;
            this.attackAnimFrame = 0;

            //Spawn an arrow.
            GameObject arrow = Instantiate(this.arrow);
            arrow.transform.position = this.transform.position;
            ArrowController controller = arrow.GetComponent<ArrowController>();

            //Tell the arrow who to hit.
            if (controller != null)
            {
                controller.Target = this.AttackTarget;
                controller.Damage = this.Attack;
                controller.Archer = this;
            }
        }
    }
}
