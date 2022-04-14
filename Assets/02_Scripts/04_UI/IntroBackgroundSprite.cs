using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBackgroundSprite : MonoBehaviour
{
    [SerializeField] List<Sprite> _spritesPossible;

    [SerializeField] float _timeToGrow = 1.0f;
    [SerializeField] float _speedMult = 1.0f;

    [SerializeField] Rigidbody2D _rb = null;
    [SerializeField] SpriteRenderer _sp = null;

    // Start is called before the first frame update
    void Start()
    {
        ChooseSprite();

        RandomizePositionAndRotation();

        StopAllCoroutines();
        StartCoroutine(PopIn());
    }

    private void Update()
    {
        if(!_sp.isVisible)
        {
            Start();
        }
    }

    void RandomizePositionAndRotation()
    {
        transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }

    void ChooseSprite()
    {
        _sp.sprite = _spritesPossible[Random.Range(0, _spritesPossible.Count)];
    }

    //Make the object start small and grow to a normal size after a given time then start moving in a random direction
    IEnumerator PopIn()
    {
        //Set velocity to a random direction
        _rb.velocity = Random.insideUnitSphere * _speedMult;

        //Make ourselves small
        Vector3 goal = Vector3.one;
        transform.localScale = Vector3.zero;

        //Grow back to the normal size over time
        float timeElapsed = 0.0f;
        while (timeElapsed < _timeToGrow)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, goal, timeElapsed / _timeToGrow);

            yield return null;
            timeElapsed += Time.unscaledDeltaTime;
        }

        transform.localScale = goal;
    }
}
