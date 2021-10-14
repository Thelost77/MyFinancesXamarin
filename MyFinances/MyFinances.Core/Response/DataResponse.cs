using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinances.Core.Response
{
    public class DataResponse<T> : Response
    {
        public T Data { get; set; }
    }
}
