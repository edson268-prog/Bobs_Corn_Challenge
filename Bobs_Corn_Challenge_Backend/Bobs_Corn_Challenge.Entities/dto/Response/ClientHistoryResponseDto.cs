using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Corn_Challenge.Entities.dto.Response
{
    public class ClientHistoryResponseDto
    {
        public int TotalPurchases { get; set; }
        public int SuccessfulPurchases { get; set; }
        public DateTime? LastPurchaseTime { get; set; }
    }
}
