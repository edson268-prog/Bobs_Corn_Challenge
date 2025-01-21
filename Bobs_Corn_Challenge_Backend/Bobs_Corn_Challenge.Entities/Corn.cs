using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Corn_Challenge.Entities
{
    public class Corn
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public DateTime PurchaseTime { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
