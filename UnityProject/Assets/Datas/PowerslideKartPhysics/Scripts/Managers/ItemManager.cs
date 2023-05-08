
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace PowerslideKartPhysics
{
    // This class manages items to be used by karts, deriving items from child objects
    public class ItemManager : NetworkBehaviour
    {
        public static ItemManager instance;
        Item[] items = new Item[0];
        public NetKartController[] allKarts = new NetKartController[0];
        public List<playerData> PlayerDatas = new List<playerData>();
        public NetworkObject No1Player;


        public override void OnNetworkSpawn()
        {
            Init();
            items = GetComponentsInChildren<Item>();
            allKarts = FindObjectsOfType<NetKartController>();
        }

        private void Update()
        {
            if (IsServer)
            {
                No1Player =
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetPlayManager.instance.rank[0]);
                
            }
             
        }

        public void GetAllKarts()
        {
            if (!IsServer) return;
            allKarts = FindObjectsOfType<NetKartController>();
            
            PlayerDatas = new List<playerData>();
            for (int i = 0; i < allKarts.Length; i++)
            {
                playerData tmp;
                tmp.clientId = allKarts[i].GetComponent<NetworkObject>().OwnerClientId;
                tmp.networkobjectId = allKarts[i].GetComponent<NetworkObject>().NetworkObjectId;
                tmp.teamNumber = allKarts[i].GetComponent<NetPlayerInfo>().teamNumber.Value;
                PlayerDatas.Add(tmp);
            }
            Debug.Log("playerdata.count :  " + PlayerDatas.Count);
            
        }

        void Init()
        {
            if (instance == null)
            {
                GameObject im = GameObject.Find("NetItemsManager");
                if (im == null)
                {
                    im = new GameObject { name = "NetItemsManager" };
                }

                if (im.GetComponent<ItemManager>() == null)
                {
                    im.AddComponent<ItemManager>();
                }
                DontDestroyOnLoad(im);
                instance = im.GetComponent<ItemManager>();
            }
        }

        // Return a random item from the list of items
        public Item GetRandomItem(int myRank, bool test) {
            if (test)
            {
                if (items.Length == 0) { return null; }
                return items[Random.Range(0, items.Length)];
            }

            else
            {
                itemName[] myItems = RandomItem(myRank);
                itemName itemName = myItems[Random.Range(0, myItems.Length)];
                Item item = GetItem(itemName.ToString());
                return item;
            }
            
        }

        // Return an item of a specific type from the list if it exists
        public Item GetItem<T>() where T : Item {
            for (int i = 0; i < items.Length; i++) {
                if (items[i] is T) {
                    return items[i];
                }
            }
            return null;
        }

        // Return an item by its object name if it exists in the list
        public Item GetItem(string itemName) {
            
            for (int i = 0; i < items.Length; i++) {
                if (string.Compare(itemName, items[i].itemName, true) == 0 || string.Compare(itemName, items[i].transform.name, true) == 0) {
                    return items[i];
                }
            }
            return null;
        }


        public itemName[] RandomItem(int myRank)
        {
            itemName[] myItems= null;
            itemName[] item1 = new[]
            {
                itemName.position, itemName.FishItem, itemName.FishItem, itemName.FishItem, itemName.FishItem,
                itemName.ShildItem, itemName.ShildItem, itemName.SquidItem, itemName.LimitSkillItem, itemName.ShildItem
            };
            itemName[] item2 = new[]
            {
                itemName.position,itemName.FishItem,itemName.FishItem,itemName.FishItem,itemName.FishItem,
                itemName.ShildItem,itemName.BirdStrikeItem,itemName.HomingItem,itemName.BirdStrikeItem
            };
            itemName[] item3 = new[]
            {
                itemName.position, itemName.BirdStrikeItem, itemName.BirdStrikeItem, itemName.BirdStrikeItem,
                itemName.BirdStrikeItem,
                itemName.HomingItem, itemName.HomingItem, itemName.SquidItem, itemName.ShildItem,
                itemName.LimitSkillItem
            };
            itemName[] item4 = new[]
            {
                itemName.position, itemName.BirdStrikeItem, itemName.BirdStrikeItem, itemName.BirdStrikeItem,
                itemName.BirdStrikeItem,
                itemName.HomingItem, itemName.HomingItem, itemName.GuardOneItem, itemName.ShildItem,
                itemName.LimitSkillItem
            };
            itemName[] item5 = new[]
            {
                itemName.position, itemName.HomingItem, itemName.HomingItem, itemName.HomingItem, itemName.HomingItem,
                itemName.BirdStrikeItem, itemName.BirdStrikeItem, itemName.SlowItem, itemName.GuardOneItem,
                itemName.BoostItem
            };
            itemName[] item6 = new[]
            {
                itemName.position, itemName.HomingItem, itemName.HomingItem, itemName.HomingItem, itemName.HomingItem,
                itemName.BirdStrikeItem, itemName.BirdStrikeItem, itemName.SlowItem, itemName.GuardOneItem,
                itemName.BoostItem
            };
            itemName[] item7 = new[]
            {
                itemName.position, itemName.BoostItem, itemName.BoostItem, itemName.BoostItem, itemName.BoostItem,
                itemName.HomingItem, itemName.HomingItem, itemName.SquidItem, itemName.ThunderItem,
                itemName.LimitSkillItem
            };
            itemName[] item8 = new[]
            {
                itemName.position, itemName.BoostItem, itemName.BoostItem, itemName.BoostItem, itemName.BoostItem,
                itemName.HomingItem, itemName.HomingItem, itemName.BoostItem, itemName.ThunderItem,
                itemName.LimitSkillItem
            };

            switch (myRank)
            {
                case 0 :
                    myItems = item1;
                    break;
                case 1 :
                    myItems = item2;
                    break;
                case 2 :
                    myItems = item3;
                    break;
                case 3 :
                    myItems = item4;
                    break;
                case 4 :
                    myItems = item5;
                    break;
                case 5 :
                    myItems = item6;
                    break;
                case 6 :
                    myItems = item7;
                    break;
                case 7 :
                    myItems = item8;
                    break;
            }

            return myItems;
        }
        
    }

    public enum itemName
    {
        position, HomingItem, GuardOneItem, LimitSkillItem, SquidItem, BirdStrikeItem, FishItem, BoostItem, ShildItem, SlowItem,ThunderItem,BombItem,BufferItem_ReverseTeam,RushItem
    }
    public struct playerData : INetworkSerializable, System.IEquatable<playerData>
    {
        public ulong clientId;
        public ulong networkobjectId;
        public int teamNumber;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out clientId);
                reader.ReadValueSafe(out networkobjectId);
                reader.ReadValueSafe(out teamNumber);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(clientId);
                writer.WriteValueSafe(networkobjectId);
                writer.WriteValueSafe(teamNumber);
            }
        }

        public bool Equals(playerData other)
        {
            return clientId == other.clientId && networkobjectId == other.networkobjectId &&
                   teamNumber == other.teamNumber;
        }
    }
}
