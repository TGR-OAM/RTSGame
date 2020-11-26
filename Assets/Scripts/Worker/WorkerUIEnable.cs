using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUIEnable : MonoBehaviour
{
    [SerializeField] private LayerMask _botLayer;
    [SerializeField] private Canvas _canvas;
    private GameObject _panel;
    [SerializeField] private Game _game;
    void Start()
    {
        
        if (_canvas.GetComponentInChildren<WorkerUI>() == null)
        {
            _game.ErrorInGame("Doesn't created menu");
        }
        else
        {
            _panel = _canvas.GetComponentInChildren<WorkerUI>().transform.gameObject;
        }
    }
    
    void Update()
    {
        
    }
}
