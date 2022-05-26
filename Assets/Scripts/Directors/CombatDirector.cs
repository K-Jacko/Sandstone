using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatDirector : Director
{
    public enum CombatDirectorType{Instant,Fast,Slow}
    public CombatDirectorType combatDirectorType;
    public float spawnRadius = 5f;
    
    private MobCard[] _validMobs;
    private Entity _player;
    private Vector2 viewerPositionOld;

    public override void Init()
    {
        _validMobs = ValidateMobCards(StageDirector.Instance.monsters);
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
    }

    void Tick()
    {
        //if (_player.playerState == Player.PlayerState.Combat)
        //{
            //This fires off event that all the spawners are subbed too. 
            //This will spawn from the lowest priced and expend its wallet. Implement weights so harder enemies are prioritised with bigger wallets
            var oldWallet = wallet;
            for (var i = 0; i < _validMobs.Length; i++)
            {
                var mobCard = _validMobs[i];
                if (wallet >= mobCard.creditCost)
                {
                    SpawnMob(mobCard);
                }
                else if (wallet > 0f && wallet < 5)
                {
                    i = -1;
                }
                else if (wallet == 0 || wallet == 5)
                {
                    wallet += (oldWallet * creditMultiplier) + StageDirector.Instance.coEef;
                    break;
                }
            
            }

            StageDirector.Instance.GenerateNavMesh();
        //}
        
    }

    void SpawnMob(MobCard mobCard)
    {
        wallet -= mobCard.creditCost;

        var go = Instantiate(mobCard.entity,
            _player.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)),
            Quaternion.identity);
        var monsterScript = go.GetComponent<Monster>();

        monsterScript.GemElement = mobCard.GemElement;
        monsterScript.gameObject.GetComponentInChildren<MeshRenderer>().material = mobCard.mobMaterial;

        monsterScript.ability = mobCard.ability;
        
        monsterScript.Stamina = mobCard.Stamina;
        monsterScript.Agility = mobCard.Agility;
        monsterScript.Focus = mobCard.Focus;
        monsterScript.Power = mobCard.Power;
        monsterScript.Init();
        go.transform.parent = transform;
        
    }

    
    
    MobCard[] ValidateMobCards(MobCard[] mobCards)
    {
        foreach (var mob in mobCards)
        {
            //Validate VS Table from stage Director. Use BNF Grammar in future. GemElements get decided here too
            Debug.Log($"{mobCards[0]}" + "is Valid");
        }

        return mobCards;
    }
    
    private void OnDisable()
    {
        TimeManager.OnMinChanged -= Tick;
        TimeManager.OnMinDoubleChanged -= Tick;
    }
}
