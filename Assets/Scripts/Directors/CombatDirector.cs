using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.AI.Navigation;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public struct SpawnData
{
    public bool isSuccessful;
    public Vector2 waveTimer;
    public GameObject target;
    public MobCard mobCard;
    public int mobTier;
    public int TierModifier;
}
public class CombatDirector : MonoBehaviour
{
    public enum CombatDirectorType{Instant,Fast,Slow}
    public CombatDirectorType combatDirectorType;
    public int wallet;
    public SpawnData spawnData;

    private Transform mobCollection;
    private StateMachine stateMachine;
    public MobCard[] mobCards;
    private int nameInt;
    //public MobTier[] mobTiers;
    public float[] prob;
    private int numOfTiers = 3;

    private float[] TierWeights;
    private float startMinTier = 0;
    private float startMaxTier = 1;
    private float startPeakTier = 2;

    private float endMinTier = 1;
    private float endMaxTier = 3;
    private float endPeakTier = 2;
    private float maxWeight = 10;

    public float minT;
    public float maxT;
    public float peakT;
    public float Weight;
    
    public void Init()
    {
        //Get all mobs spawnable on this map //Use Dictionary and strings to make array of possible spawns on this map
        //Pick target //Use data on mob to decide how far to spawn
        //Start at 0 credits
        //Start counter  //Set interval for  in Wave and out Wave
        //Start wave(SpawnLoop) after interval hits 0
        //SpawnLoop = Check mobCap, if Last spawn was a success use its data(MobCard not Tier) else, PickCard, PickTier(Must be able to afford Cards Elite Tier (Compare Tier modifier ect *6)), CountSpawns, setup mob values, setup rewards, Spawn, On success store spawndata
        LoadSpawnableMobs();
        wallet = 0;
        gameObject.AddComponent<CreditGenerator>();
        prob = new float[numOfTiers];
        TierWeights = new float[numOfTiers];
        spawnData = new SpawnData();
        if (!spawnData.isSuccessful)
        {
            spawnData = SetupNewSpawnData();
        }
        StateMachine();
        
        //Credits per second = creditMultiplier * (1 + 0.4 * coeff) * (playerCount + 1) / 2
        //_validMobs = ValidateMobCards(StageDirector.Instance.monsters);
        
        // switch (combatDirectorType)
        // {
        //     case CombatDirectorType.Fast :
        //         TimeManager.OnMinChanged += Tick;
        //         break;
        //     case CombatDirectorType.Instant:
        //         Tick();
        //         break;
        //     case CombatDirectorType.Slow :
        //         TimeManager.OnMinDoubleChanged += Tick;
        //         break;
        // }
        //
        // wallet = StageDirector.Instance.wallet;
        // LoadResources();
    }

    

    void LoadSpawnableMobs()
    {
        //Most expensive at 0index
        var validMobs = SceneDirector.Instance.currentMapData.validMobs;
        var mcList = new List<MobCard>();
        for (int i = 0; i < validMobs.Length; i++)
        {
            var mobCard = Resources.Load<MobCard>("MobCards/" + $"{validMobs[i]}");
            mcList.Add(mobCard);
        }
        mobCards = mcList.ToArray();
        if (mobCards.Length > 1)
        {
            Array.Sort(mobCards,
                delegate(MobCard x, MobCard y) { return x.creditCost.CompareTo(y.creditCost); });
        }
        

        //End up with An array of MobCards so i can use their strings to load Prefabs later in the loop 
        //Some will not be able to spawn cus to expensive but thats okay, add them to the list anyway. 
        //5 spawnable enemies each level 
        //1-2 spawnable bosses
        
    }

    public SpawnData SetupNewSpawnData()
    {
        var spawnData = new SpawnData();
        spawnData.target = StageDirector.Instance.friendlyEntities[Random.Range(0, StageDirector.Instance.friendlyEntities.Count)]
            .gameObject;
        spawnData.waveTimer = new Vector2(Random.Range(4.5f,9), Random.Range(0.1f, 1));
        spawnData.mobCard = PickMobCard();
        spawnData.mobTier = PickMobTier();
        return spawnData;
    }

    public MobCard PickMobCard()
    {
        while (true)
        {
            //pick one at random 
            //check price to wallet
            //check if too cheap
            var mc = mobCards[Random.Range(0, mobCards.Length)];
            return mc;
            
        }
    }

    public void Probability()
    {
        var time = (float)SceneDirector.Instance.TimeInGame.Elapsed.TotalSeconds;
        minT += 10 * ((endMinTier - startMinTier) / (time * 2 ));
        maxT += 10 * ((endMaxTier - startMaxTier) / (time * 2));
        peakT += 10 * ((endPeakTier - startPeakTier) / (time * 2));


        for (int T = 0; T < numOfTiers; T++)
        {
            if (T < minT)
                Weight = 0;
            if (T > maxT)
                Weight = 0;
            if (minT <= T & T < peakT)
            {
                Weight = 1 + (maxWeight - 1) / (peakT - minT) * (T - minT);
            }

            if (peakT <= T & T < maxT)
            {
                Weight = 1 + (maxWeight - 1) / (peakT - maxT) * (T - maxT);
            }

            TierWeights[T] = Weight;
            prob[T] = Weight / TierWeights.Sum();
        }
    }

    public int PickMobTier()
    {
        var p = Random.Range(0f, 1f);
        float sumProb = 0;
        for (int i = 0; i < numOfTiers; i++)
        {
            sumProb += prob[i];
            if (sumProb >= p)
            {
                return i;
            }
        }

        return 0;
    }

    void StateMachine()
    {
        var timer = new Stopwatch();
        timer.Start();
        stateMachine = new StateMachine();
        var outWave = new OutWave(spawnData,timer,this);
        var inWave = new InWave(spawnData,this, stateMachine, outWave);
        stateMachine.SetState(outWave);

        stateMachine.AddAnyTransition(inWave,WaveCooldown());

        Func<bool> WaveCooldown() => () => (timer.Elapsed.Seconds >= spawnData.waveTimer.x);

    }

    void Update()
    {
        stateMachine.Tick();
    }

    public Transform CollectionTransform()
    {
        if (!mobCollection)
        {
            mobCollection = new GameObject().transform;
            mobCollection.name = "Mobs";
            return mobCollection.transform;
        }

        return mobCollection;
    }
    
}
