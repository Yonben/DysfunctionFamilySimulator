using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogDefecation : MonoBehaviour
{
    private DogPlayableCharacter dogScript;
    [SerializeField] private GameObject poop;

    [SerializeField] private float poopIntervalTime;

    [SerializeField] private Transform poopTransform;

    private PlayerController _playerController;

    private Transform _transform;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _transform = transform;
        dogScript = GetComponent<DogPlayableCharacter>();
    }

    private void Start()
    {
        StartCoroutine(nameof(defecateTimer));
    }

    IEnumerator defecateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(poopIntervalTime);
            dogScript.ApplyStressImpact(1);
        }
    }

    private void defecate()
    {
        Vector3 position = _transform.position;
        position += _playerController.isRight ? poopTransform.localPosition : -poopTransform.localPosition;
        Instantiate(poop, position, Quaternion.identity);
    }
}
