using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    partial class Program : MyGridProgram {

        // wContainerSorter, v1.1
        // Copyright (c) 2023 by W0LF aka 'dreamforce'
        //
        // This script sorts the content of the containers.

        // Type of sorting.
        // By default (empty string) - sort by item's Type (MyObjectBuilder_Component/Computer, MyObjectBuilder_Ore/Stone, etc).
        // It can be a "Name" string - sort by SubtypeId (Computer, Stone).
        static readonly string sortType = "";



        // =============== !!! NO CHANGES BELOW !!! =============== //
        static List<IMyTerminalBlock> containers = new List<IMyTerminalBlock>();
        static List<MyInventoryItem> containerItems = new List<MyInventoryItem>();

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Main(string argument, UpdateType updateSource) {
            containers.Clear();
            GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(containers);

            foreach (var container in containers) {
                containerItems.Clear();
                IMyInventory containerInventory = container.GetInventory();
                containerInventory.GetItems(containerItems);

                if (sortType.ToUpper() == "NAME") containerItems.Sort((MyInventoryItem x, MyInventoryItem y) => x.Type.SubtypeId.ToString().CompareTo(y.Type.SubtypeId.ToString()));
                else containerItems.Sort((MyInventoryItem x, MyInventoryItem y) => x.Type.ToString().CompareTo(y.Type.ToString()));

                foreach (MyInventoryItem item in containerItems) {
                    for (int i = 0; i < containerInventory.ItemCount; i++) {
                        MyInventoryItem item2 = (MyInventoryItem)containerInventory.GetItemAt(i);
                        if (item.Equals(item2)) containerInventory.TransferItemTo(containerInventory, i, i + containerInventory.ItemCount, true);
                    }
                }
            }
        }
    }
}
