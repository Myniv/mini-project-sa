﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models.Requests
{
    public class ReturnBookRequest
    {
        public List<int>? BookId { get; set; }
        public int UserId { get; set; }

    }
}
