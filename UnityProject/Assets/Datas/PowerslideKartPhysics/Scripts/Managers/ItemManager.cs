
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace PowerslideKartPhysics
{
    // This class manages items to be used by karts, deriving items from child objects
    public class ItemManager : NetworkBehaviour
    {
        public static ItemManager instance;
        Item[] items = new Item[0];
        public NetKartController[] allKarts = new NetKartController[0];

        private void Awake() {
            Init();
            items = GetComponentsInChildren<Item>();
            allKarts = FindObjectsOfType<NetKartController>();
            Debug.Log(allKarts.Length);
        }

        public void GetAllKarts()
        {
            allKarts = FindObjectsOfType<NetKartController>();
            Debug.Log(allKarts.Length);
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
        public Item GetRandomItem() {
            if (items.Length == 0) { return null; }
            return items[Random.Range(0, items.Length)];
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

    }
}
