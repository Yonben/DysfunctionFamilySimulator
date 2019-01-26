using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogDefecation : MonoBehaviour
{
    [SerializeField] private GameObject poop;

    [SerializeField] private float poopIntervalTime;

    [SerializeField] private Transform poopTransform;

    private PlayerController _playerController;

    private Transform _transform;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _transform = transform;
    }

    private void Start()
    {
        StartCoroutine(nameof(defecate));
    }

    IEnumerator defecate()
    {
        while (true)
        {
            yield return new WaitForSeconds(poopIntervalTime);

            Vector3 position = _transform.position;
            position += _playerController.isRight ? poopTransform.localPosition : -poopTransform.localPosition;
            Instantiate(poop, position, Quaternion.identity);
        }
    }
}
