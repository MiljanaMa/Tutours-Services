using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class WalletDto
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public double AdventureCoins {  get; set; }
    }
}
