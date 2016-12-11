using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float Damage;
    public Base_Unit Archer;
    public Base_Unit Target;

    void Update()
    {
        //Is our target dead?
        if (this.Target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        //Heat-seaking arrows!
        this.transform.LookAt(this.Target.transform);
        float maxMove = 10 * Time.deltaTime;
        float moveAmount = Mathf.Min(maxMove, Vector3.Distance(this.transform.position, this.Target.transform.position));
        this.transform.Translate(new Vector3(0, 0, moveAmount), Space.Self);

        //Have we hit the target yet?
        if (Vector3.Distance(this.transform.position, Target.transform.position) < .1f)
        {
            this.Target.TakeDamage(this.Damage, this.Archer);
            Destroy(this.gameObject);
        }
    }
}
