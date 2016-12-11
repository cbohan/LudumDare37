using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Base_EnemyUnit
{
    public GameObject arrow;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        //Find the closest enemy, attack it.
        if (this.isAggressive)
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
            }
        }
    }
}
