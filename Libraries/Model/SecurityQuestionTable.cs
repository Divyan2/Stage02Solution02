﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Model
{
    public class SecurityQuestionTable
    {
        [Key]
        public int Code { get; set; }
        public string Question { get; set; }
    }
}
