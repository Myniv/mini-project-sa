﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Helpers
{
    public class BookCategoryCount
    {
        public string? Category {  get; set; }
        public int Count { get; set; }
        public int TotalMoneyPaid { get; set; }
    }
}