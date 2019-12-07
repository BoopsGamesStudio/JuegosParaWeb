using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Weapon : Item
{
    [HideInInspector]
    public string[] weaponAttribs = {
                                     "Dagger", "Melee", "1", "1", "2", "4",
                                     "Sword", "Melee", "4", "2", "3", "3",
                                     "Axe", "Melee", "5", "4", "5", "2",
                                     "Spear", "Melee", "3", "4", "2", "3",
                                     "Buckler", "Shield", "2", "2", "3", "3",
                                     "Tear Shield", "Shield", "4", "5", "4", "1",
                                     "Medium Shield", "Shield", "3", "3", "3", "3",
                                     "Sharp Shield", "Shield", "5", "5", "4", "2",
                                     "Plasma Handgun", "Distance", "2", "1", "2", "3",
                                     "Plasma Submachine", "Distance", "1", "2", "1", "4",
                                     "Plasma Shotgun", "Distance", "5", "3", "5", "3",
                                     "Plasma Cannon", "Distance", "5", "4", "3", "2"
                                    };
    public enum weaponType {Melee, Shield, Distance};
    private weaponType type;
    private float impact;
    private float endurance;
    private float cadence;
    private float speed;

    public Weapon(string name) : base(name)
    {
        var index = System.Array.IndexOf(weaponAttribs, name);
        type = (weaponType)System.Enum.Parse(typeof(weaponType), weaponAttribs[index + 1]);
        impact = float.Parse(weaponAttribs[index + 2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        endurance = float.Parse(weaponAttribs[index + 3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        cadence = float.Parse(weaponAttribs[index + 4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        speed = float.Parse(weaponAttribs[index + 5], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
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

    public float getSpeed()
    {
        return speed;
    }

    public override string getAttribs()
    { 
        return "[" + getName() + ", " + getType() + ", " + getImpact() + ", " + getEndurance() + ", " + getCadence() + ", " + getSpeed() + "]";
    }
    #endregion

}
