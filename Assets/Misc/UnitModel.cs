using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Model of a unit just data not attached to a game object.
public class UnitModel
{ 
    public enum UnitType { Longswordsman, Shortswordsman, Archer }
    public UnitType Type;
    public float Health;
    public float MaxHealth;
    public float Attack;
    public float Block;
    public float Range;
    public float AttackCooldown;
    public int Experience;
    public bool IsAggressive; //Only for enemies and only for tutorial level.

    public static UnitModel GenerateLongswordsman()
    {
        UnitModel unit = new UnitModel();
        unit.Type = UnitType.Longswordsman;
        unit.Health = Base_Unit.LONGSWORDSMAN_BASE_HEALTH;
        unit.MaxHealth = Base_Unit.LONGSWORDSMAN_BASE_HEALTH;
        unit.Attack = Base_Unit.LONGSWORDSMAN_BASE_ATTACK;
        unit.Block = Base_Unit.LONGSWORDSMAN_BASE_BLOCK;
        unit.Range = Base_Unit.LONGSWORDSMAN_BASE_RANGE;
        unit.AttackCooldown = Base_Unit.LONGSWORDSMAN_BASE_ATTACK_COOLDOWN;
        unit.Experience = 0;
        unit.IsAggressive = true;

        return unit;
    }

    public static UnitModel GenerateShortswordsman()
    {
        UnitModel unit = new UnitModel();
        unit.Type = UnitType.Shortswordsman;
        unit.Health = Base_Unit.SHORTSWORDSMAN_BASE_HEALTH;
        unit.MaxHealth = Base_Unit.SHORTSWORDSMAN_BASE_HEALTH;
        unit.Attack = Base_Unit.SHORTSWORDSMAN_BASE_ATTACK;
        unit.Block = Base_Unit.SHORTSWORDSMAN_BASE_BLOCK;
        unit.Range = Base_Unit.SHORTSWORDSMAN_BASE_RANGE;
        unit.AttackCooldown = Base_Unit.SHORTSWORDSMAN_BASE_ATTACK_COOLDOWN;
        unit.Experience = 0;
        unit.IsAggressive = true;

        return unit;
    }

    public static UnitModel GenerateArcher()
    {
        UnitModel unit = new UnitModel();
        unit.Type = UnitType.Archer;
        unit.Health = Base_Unit.ARCHER_BASE_HEALTH;
        unit.MaxHealth = Base_Unit.ARCHER_BASE_HEALTH;
        unit.Attack = Base_Unit.ARCHER_BASE_ATTACK;
        unit.Block = Base_Unit.ARCHER_BASE_BLOCK;
        unit.Range = Base_Unit.ARCHER_BASE_RANGE;
        unit.AttackCooldown = Base_Unit.ARCHER_BASE_ATTACK_COOLDOWN;
        unit.Experience = 0;
        unit.IsAggressive = true;

        return unit;
    }

    public void AddExperience(int xp)
    {
        for (int i = 0; i < xp; i++)
        {
            this.Experience++;
            this.Attack += Mathf.Min(this.Attack*.05f, .5f);
            this.MaxHealth += Mathf.Min(this.MaxHealth * .05f, 2f);
            this.Health = this.MaxHealth;
            this.Block += Mathf.Min(this.Block * .05f, .5f);
        }
    }
}
