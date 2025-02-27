﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class State
{
    public int Id { get; set; }

    public int CountryId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
