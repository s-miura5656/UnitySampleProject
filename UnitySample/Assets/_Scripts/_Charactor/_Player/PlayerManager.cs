using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //[SerializeField] private PlayerAnimation animationManager = null;
    [SerializeField] private PlayerMove playerMove = null;

    private void Start()
    {
        playerMove.MyStart();
    }

    private void Update()
    {
        playerMove.MyUpdate();
        //animationManager.MyUpdate();
    }

    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        
    }
}
