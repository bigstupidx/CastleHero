﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Scene
    {
        Login = 0,
        Loading,
        Wait,
        Battle,
    }

    [SerializeField] NetworkManager networkManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] DataManager dataManager;
    [SerializeField] BattleManager battleManager;

    void Awake()
    {
        tag = "GameManager";
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        ManagerInitialize();
    }

    void ManagerInitialize()
    {
        networkManager = (Instantiate(Resources.Load("Prefabs/Manager/NetworkManager")) as GameObject).GetComponent<NetworkManager>();
        uiManager = (Instantiate(Resources.Load("Prefabs/Manager/UIManager") as GameObject)).GetComponent<UIManager>();
        loadingManager = (Instantiate(Resources.Load("Prefabs/Manager/LoadingManager") as GameObject)).GetComponent<LoadingManager>();
        dataManager = (Instantiate(Resources.Load("Prefabs/Manager/DataManager") as GameObject)).GetComponent<DataManager>();
        battleManager = (Instantiate(Resources.Load("Prefabs/Manager/BattleManager") as GameObject)).GetComponent<BattleManager>();
        
        networkManager.ManagerInitialize();
        uiManager.ManagerInitialize();
        loadingManager.ManagerInitialize();
        battleManager.ManagerInitialize();
    }

    public void ManagerDestory()
    {
        Destroy(networkManager.gameObject);
        Destroy(uiManager.gameObject);
        Destroy(loadingManager.gameObject);
        Destroy(dataManager.gameObject);
        Destroy(battleManager.gameObject);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        networkManager.DataHandle();
    }

    void LateUpdate()
    {
        networkManager.DataSend();
    }

    void OnApplicationQuit()
    {
        networkManager.GameExit();
    }
}