using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobs_Corn_Challenge.Entities
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
