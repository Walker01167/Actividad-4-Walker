using System;
using System.Collections.Generic;

namespace RestAPIPrueba.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
