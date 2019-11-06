using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Weapon : Item
{ 
    public enum weaponType {Melee, Shield, Distance};
    private weaponType type;
    private float impact;
    private float endurance;
    private float cadence;
    private float weight;

    public Weapon(string name, weaponType type = weaponType.Melee, float impact = 3, float endurance = 2, float cadence = 6, float weight = 50) : base(name)
    {
        this.type = type;
        this.impact = impact;
        this.endurance = endurance;
        this.cadence = cadence;
        this.weight = weight;

    }

    #region Getters
    public weaponType getType()
    {
        return type;
    }

    public float getImpact()
    {
        return impact;
    }

    public float getEndurance()
    {
        return endurance;
    }

    public float getCadence()
    {
        return cadence;
    }

    public float getWeight()
    {
        return weight;
    }

    public override string getAttribs()
    { 
        return "[" + getType() + ", " + getImpact() + ", " + getEndurance() + ", " + getCadence() + ", " + getWeight() + "]";
    }
    #endregion

}
