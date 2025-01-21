using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Corn_Challenge.Entities.dto.Response
{
    public class PurchaseCornResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
