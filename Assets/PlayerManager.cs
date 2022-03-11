using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    List<GameObject> _corpses;
    [SerializeField] GameObject _playerObject;
    GameObject _player;

    [Header("Level related")]
    [SerializeField] GameObject _startingVat;
 
    [SerializeField] int _lives;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if ((_lives -= 1) <= 0)
        {
            Lose();
            return;
        }

        _player = Instantiate(_playerObject, _startingVat.transform.position, Quaternion.identity);
    }

    void Lose()
    {

    }
}
