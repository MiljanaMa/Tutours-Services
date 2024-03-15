using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class Wallet : Entity
    {

        public Wallet() { }

        public Wallet(int userId, double adventureCoins)
        {
            UserId = userId;
            AdventureCoins = adventureCoins;
        }

        public int UserId {  get; set; }
        public double AdventureCoins { get; set; }
    }
}
