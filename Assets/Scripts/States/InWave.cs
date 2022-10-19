using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class InWave : IState
{
    private SpawnData spawnData;
    private Stopwatch timer;
    private CombatDirector combatDirector;
    private OutWave outWave;
    private StateMachine stateMachine;

    private int nameIndex = 1;
    //SpawnLoop = Check mobCap, if Last spawn was a success use its data(MobCard not Tier) else, PickCard, PickTier(Must be able to afford Cards Elite Tier (Compare Tier modifier ect *6)), CountSpawns, setup mob values, setup rewards, Spawn, On success store spawndata

    public void Tick()
    {
        combatDirector.Probability();
        spawnData = combatDirector.spawnData;
        SetUpTier();
        var mod = spawnData.mobCard.creditCost * spawnData.TierModifier;
        if (combatDirector.wallet >= mod)
        {
            if (combatDirector.wallet >= mod * 6)
            {
                Debug.Log("Too Cheap" + $"{spawnData.mobCard.creditCost * spawnData.TierModifier}" + $"{spawnData.mobCard.name}" + $"{spawnData.mobTier}");
                spawnData.isSuccessful = false;
                stateMachine.SetState(outWave);
            }
            else
            {
                SpawnMob();
                Debug.Log( $"{spawnData.mobCard.name}"+ $"{spawnData.mobTier}"); 
            }
            
        }
        else
        {
            if(combatDirector.wallet > spawnData.mobCard.creditCost)
            {
                spawnData.TierModifier = 1;
                spawnData.mobTier = 0;
                SpawnMob();
            }
            else
            {
                Debug.Log("Could Not Afford" + $"{spawnData.mobCard.creditCost * spawnData.TierModifier}" + $"{spawnData.mobCard.name}" + $"{spawnData.mobTier}");
                spawnData.isSuccessful = false;
                stateMachine.SetState(outWave);
            }
            
        }
        

    }
    
    public InWave(SpawnData spawnData, CombatDirector combatDirector, StateMachine stateMachine, OutWave outWave)
    {
        this.spawnData = spawnData;
        this.combatDirector = combatDirector;
        this.stateMachine = stateMachine;
        this.outWave = outWave;
    }

    public void OnEnter()
    {
        Debug.Log("INWAVE");
        nameIndex++;
        SetUpTier();
        
    }
    public void OnExit()
    {
        
    }

    private void SetUpTier()
    {
        if (spawnData.mobTier == 0)
        {
            spawnData.TierModifier = 1;
        }

        if (spawnData.mobTier == 1)
        {
            spawnData.TierModifier = 3;
        }

        if (spawnData.mobTier == 2)
        {
            spawnData.TierModifier = 5;
        }
    }
    
    void SpawnMob()
    {
        var go = Object.Instantiate(Resources.Load<Object>("Mobs/" + $"{spawnData.mobCard.name}" + $"{spawnData.mobTier}"),SceneDirector.Instance.Player.gameObject.transform.position + new Vector3(Random.Range(-spawnData.mobCard.spawnDistance,spawnData.mobCard.spawnDistance),2,Random.Range(-spawnData.mobCard.spawnDistance,spawnData.mobCard.spawnDistance)), Quaternion.identity, combatDirector.CollectionTransform());
        go.name = $"{spawnData.mobCard.name}" + $"{spawnData.mobTier}" + "Group:" + $"{nameIndex}";
        var mobScript = go.GetComponent<Monster>();
        mobScript.Init(spawnData);
        spawnData.isSuccessful = true;
        combatDirector.wallet -= spawnData.mobCard.creditCost * spawnData.TierModifier;
        Tick();
        // var go = Instantiate(mobCard.entity,
        //     _player.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius),
        //         Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)),
        //     Quaternion.identity);
        // var monsterScript = go.GetComponent<Monster>();

        //monsterScript.GemElement = mobCard.GemElement;
        // monsterScript.gameObject.GetComponentInChildren<MeshRenderer>().material = mobCard.mobMaterial;
        //
        // monsterScript.ability = mobCard.ability;
        //
        // // monsterScript.Stamina = mobCard.Stamina;
        // // monsterScript.Agility = mobCard.Agility;
        // // monsterScript.Focus = mobCard.Focus;
        // // monsterScript.Power = mobCard.Power;
        //
        // monsterScript.Init();
        // go.transform.parent = transform;
        // go.name = mobCard.name + nameInt;
        //TODO The monster script needs to call new stats and use them values

    }
    
}
