﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    sealed class ColumnDB : Attribute
    {
        public string Name { get; set; }
        public bool IsMapped { get; set; }
        public bool IsKey { get; set; }

    }
}
