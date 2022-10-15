using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatDirector : MonoBehaviour
{
    public enum CombatDirectorType{Instant,Fast,Slow}
    public CombatDirectorType combatDirectorType;
    public float spawnRadius = 5f;
    [SerializeField]
    private MobCard[] mobCards;
    private GameObject _player;
    private Vector2 viewerPositionOld;
    private float wallet;
    private int nameInt;
    
    public void Init(CombatDirectorType combatDirectorType)
    {
        //Get all mobs spawnable on this map //Use Dictionary and strings to make array of possible spawns on this map
        //Pick target //Use data on mob to decide how far to spawn
        //Start at 0 credits
        //Start counter  //Set interval for  in Wave and out Wave
        //Start wave(SpawnLoop) after interval hits 0
        //SpawnLoop = Check mobCap, if Last spawn was a success use its data(MobCard not Tier) else, PickCard, PickTier(Must be able to afford Cards Elite Tier (Compare Tier modifier ect *6)), CountSpawns, setup mob values, setup rewards, Spawn, On success store spawndata
        
        
        
        //Credits per second = creditMultiplier * (1 + 0.4 * coeff) * (playerCount + 1) / 2
        //_validMobs = ValidateMobCards(StageDirector.Instance.monsters);
        _player = StageDirector.Instance.Player;
        switch (combatDirectorType)
        {
            case CombatDirectorType.Fast :
                TimeManager.OnMinChanged += Tick;
                break;
            case CombatDirectorType.Instant:
                Tick();
                break;
            case CombatDirectorType.Slow :
                TimeManager.OnMinDoubleChanged += Tick;
                break;
        }

        wallet = StageDirector.Instance.wallet;
        LoadResources();
    }
    
    void LoadResources()
    {
        //Most expensive at 0index
        //Use BNF Grammar in future. GemElements get decided here too
        mobCards = Resources.LoadAll<MobCard>("MobCards");
        Array.Sort(mobCards,
            delegate(MobCard x, MobCard y) { return x.creditCost.CompareTo(y.creditCost); });
    }

    void Tick()
    {
        //if (_player.playerState == Player.PlayerState.Combat)
        //{
            //This fires off event that all the spawners are subbed too. 
            //This will spawn from the lowest priced and expend its wallet. Implement weights so harder enemies are prioritised with bigger wallets
            nameInt = 0;
            var oldWallet = wallet;
            for (var i = 0; i < mobCards.Length; i++)
            {
                var mobCard = mobCards[i];
                if (wallet >= mobCard.creditCost)
                {
                    SpawnMob(mobCard);
                }
                else if (wallet > 0f && wallet < mobCards[0].creditCost)
                {
                    i = -1;
                }
                
                if (wallet == 0 || wallet == 5)
                {
                    wallet += (StageDirector.Instance.wallet * (int)StageDirector.Instance.coEef);//+ StageDirector.Instance.coEef;
                    break;
                }

                nameInt++;

            }

            //StageDirector.Instance.GenerateNavMesh();
        //}
        
    }

    void SpawnMob(MobCard mobCard)
    {
        wallet -= mobCard.creditCost;

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

    private void OnDisable()
    {
        TimeManager.OnMinChanged -= Tick;
        TimeManager.OnMinDoubleChanged -= Tick;
    }
}
