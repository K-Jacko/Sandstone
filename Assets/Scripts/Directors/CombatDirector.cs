using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatDirector : Director
{
    public float spawnRadius = 5f;
    public GameObject mobSpawner;
    public int gridWidth = 5;
    public int gridHeight = 5;
    public float cellSize = 100;
    private MobCard[] _validMobs;
    private GameObject _player;

    public override void Init()
    {
        _validMobs = ValidateMobCards(StageDirector.Instance.monsters);
        _player = StageDirector.Instance.Player;
        baseGrid = new Grid(gridWidth,gridHeight,cellSize,Color.red);
        GenerateMobSpawners(baseGrid);
    }

    protected override void Tick()
    {
        //This will spawn from the lowest priced and expend its wallet. Implement weights so harder enemies are prioritised with bigger wallets
        base.Tick();
        for (var i = 0; i < _validMobs.Length; i++)
        {
            var validMob = _validMobs[i];
            if (wallet >= validMob.creditCost)
            {
                wallet -= validMob.creditCost;
                var go = Instantiate(validMob.entity,
                    _player.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius),
                        Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)),
                    Quaternion.identity);
                var goM = go.GetComponent<Monster>();
                validMob.GemElement = new None();
                goM.GemElement = validMob.GemElement;
                goM.gameObject.GetComponent<MeshRenderer>().material = validMob.mobMaterial;
            }
            else if (wallet > 0f)
            {
                i = -1;
            }
            else if (wallet == 0)
            {
                wallet += coEef * creditMultiplier;
                break;
            }
        }
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

    void GenerateMobSpawners(Grid grid)
    {
        this.baseGrid = grid;
        for (int x = 0; x < baseGrid.GetWidth(); x++)
        {
            for (int y = 0; y < baseGrid.GetHeigth(); y++)
            {
                var startPositionX = (baseGrid.GetWidth() / -2);
                var startPositionY = -baseGrid.GetHeigth() / 2;
                Instantiate(mobSpawner,baseGrid.GetWorldPosition(startPositionX + x, startPositionY + y) + new Vector3(cellSize,0,cellSize) * .5f,Quaternion.identity);
            }
        }
    }
}
