using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoStorageFiles.Models
{
    internal class LegoBricks
    {
        string itemId;
        string itemName;
        string categoryName;
        string colorName;
        int quantity;

        public LegoBricks(string itemId, string itemName, string categoryName, string colorName, int quantity)
        {
            this.itemId = itemId;
            this.itemName = itemName;
            this.categoryName = categoryName;
            this.colorName = colorName;
            this.quantity = quantity;
        }


        public string ItemId { get => itemId; set => itemId = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public string CategoryName { get => categoryName; set => categoryName = value; }
        public string ColorName { get => colorName; set => colorName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
