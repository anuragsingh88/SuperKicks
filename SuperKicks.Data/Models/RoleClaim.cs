﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuperKicks.Data.Models;

public partial class RoleClaim
{
    public int Id { get; set; }

    public Guid RoleId { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public virtual Role Role { get; set; }
}