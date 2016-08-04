using UnityEngine;
using System.Collections;
using Team = ConstFile.Team;
public class Mortal : MonoBehaviour
{
    public float energy;
    public float gainRate;
    public Team team;

    public RectTransform energyBar;
    protected float minX;
    protected float maxX;
    protected float cachedY;
    public float maxEnergy;
    protected bool dead;

    public float MaxEnergy
    {
        get { return maxEnergy; }
    }

    // Use this for initialization
    void Awake()
    {
        dead = false;
        if (energyBar != null)
        {
            cachedY = energyBar.anchoredPosition.y;
            maxX = energyBar.anchoredPosition.x;
            minX = energyBar.anchoredPosition.x - energyBar.rect.width;
        }
        
    }


    protected virtual void Update()
    {
        if (energyBar != null)
        {
            energy = Mathf.Clamp(energy, -10, maxEnergy);
            energyBar.anchoredPosition = new Vector3(minX + ((energy / maxEnergy) * energyBar.rect.width), cachedY);
            dead = energy <= 0;
        }
    }

    public void TakeDamage(float amt)
    {
        energy -= amt;
    }
}
