﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class AccessesService
    {
        private readonly IAccessesRepository accesses;

        public AccessesService(IAccessesRepository accesses)
        {
            this.accesses = accesses;
        }






    }
}
