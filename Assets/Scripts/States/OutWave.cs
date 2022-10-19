using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class OutWave : IState
{
    private SpawnData spawnData;
    private Stopwatch timer;
    private CombatDirector combatDirector;
    public void Tick()
    {
    }
    
    public OutWave(SpawnData spawnData, Stopwatch timer, CombatDirector combatDirector)
    {
        this.spawnData = spawnData;
        this.timer = timer;
        this.combatDirector = combatDirector;
    }

    public void OnEnter()
    {
        Debug.Log("OUTWAVE");
        if (!combatDirector.spawnData.isSuccessful)
        {
            combatDirector.spawnData = combatDirector.SetupNewSpawnData();
        }
        timer.Restart();
        
    }

    public void OnExit()
    {
        
    }
}
