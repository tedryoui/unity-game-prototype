﻿using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Item;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Jug : MonoBehaviour, IDamagable, IItemDroppable
{
    [Inject] private PlayerBase _playerBase;
    public GameObject destroyParticle;
    public List<ItemDrop> Drop;

    public AudioSource AudioSource;
    public AudioClip BreakClip;
    
    public bool GetDamage(float damage)
    {
        var part = Instantiate(destroyParticle);
        part.transform.position = transform.position;
        DropItems(_playerBase);

        _playerBase.StartCoroutine(AudioSource.PlayAndDestroy(BreakClip));
        Destroy(gameObject);

        return true;
    }

    public void DropItems<T1, T2>(EntityBase<T1, T2> toWho) where T1 : EntityController where T2 : IEntityState
    {
        foreach (ItemDrop itemDrop in Drop)
            if(Random.Range(0, 1) <= itemDrop.Chance)
                (toWho as PlayerBase)?.Inventory.AddItem(itemDrop.Item, 
                    (int)(itemDrop.Amount /** Random.Range(0f, 0.5f)*/));
    }
}