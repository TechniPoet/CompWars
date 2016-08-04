using UnityEngine;
using System.Collections.Generic;
using Team = ConstFile.Team;
using PuppetType = ConstFile.PuppetType;


public class UnitManager : UnitySingleton<UnitManager>
{
    public List<Puppet> ROCK_GRAVEYARD = new List<Puppet>();
    public List<Puppet> LIVING_UNITS = new List<Puppet>();

    GameObject rockUnitPrefab;
    static int id = 0;

    public delegate void managementDelegate(Team unitTeam, Transform unitTransform, PuppetType unitType);
    public event managementDelegate AddUnit;
    public event managementDelegate RemoveUnit;

    public void Awake()
    {
        rockUnitPrefab = Resources.Load<GameObject>("RockUnit");
        rockUnitPrefab.SetActive(false);
    }

    
    public void CreateNewUnit(PuppetType unitType, Team unitTeam, Vector2 gridLocation)
    {
        Puppet unit;
        switch (unitType)
        {
            case PuppetType.ROCK:
                if (ROCK_GRAVEYARD.Count > 0)
                {
                    unit = ROCK_GRAVEYARD[0];
                    ROCK_GRAVEYARD.RemoveAt(0);
                    unit.gameObject.SetActive(true);
                    unit.Setup(unitTeam, unitType, gridLocation);
                    unit.transform.position = ArenaManager.Instance.GridToWorld(gridLocation);
                    LIVING_UNITS.Add(unit);
                }
                else
                {
                    GameObject gUnit = Instantiate(rockUnitPrefab);
                    gUnit.transform.position = ArenaManager.Instance.GridToWorld(gridLocation);
                    gUnit.SetActive(true);
                    unit = gUnit.GetComponent<Puppet>();
                    unit.Setup(unitTeam, unitType, gridLocation);
                    unit.id = id;
                    id++;
                    LIVING_UNITS.Add(unit);
                    
                }
                break;
            default:
                throw new System.Exception("Unknown unitType being attempted to create.");
        }
        if (AddUnit != null)
        {
            AddUnit(unitTeam, unit.transform, unitType);
        }
        
    }


    public void UnitDied(Puppet unit)
    {
        switch (unit.currType)
        {
            case PuppetType.ROCK:
                LIVING_UNITS.Remove(unit);
                unit.gameObject.SetActive(false);
                ROCK_GRAVEYARD.Add(unit);
                break;
            default:
                throw new System.Exception("Unknown unitType being attempted to remove.");
        }
        if (RemoveUnit != null)
        {
            RemoveUnit(unit.team, unit.transform, unit.currType);
        }
        
    }
}
