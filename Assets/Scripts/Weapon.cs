using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Weapon : Item
{
    [HideInInspector]
    public string[] weaponAttribs = {
                                     "sword", "Melee", "5", "2", "6", "50",
                                     "shield", "Shield", "4", "4", "8", "70",
                                     "cannon", "Distance", "2", "1", "1", "30"
                                    };
    public enum weaponType {Melee, Shield, Distance};
    private weaponType type;
    private float impact;
    private float endurance;
    private float cadence;
    private float weight;

    public Weapon(string name) : base(name)
    {
        var index = System.Array.IndexOf(weaponAttribs, name);
        type = (weaponType)System.Enum.Parse(typeof(weaponType), weaponAttribs[index + 1]);
        impact = float.Parse(weaponAttribs[index + 2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        endurance = float.Parse(weaponAttribs[index + 3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        cadence = float.Parse(weaponAttribs[index + 4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        weight = float.Parse(weaponAttribs[index + 5], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
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
        return "[" + getName() + ", " + getType() + ", " + getImpact() + ", " + getEndurance() + ", " + getCadence() + ", " + getWeight() + "]";
    }
    #endregion

}
