﻿using System;
using System.Threading.Tasks;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Models
{
    public class RestaurantDto
    {
        public string Name { get; set; }
        public Uri MenuLink { get; set; }
    }
}