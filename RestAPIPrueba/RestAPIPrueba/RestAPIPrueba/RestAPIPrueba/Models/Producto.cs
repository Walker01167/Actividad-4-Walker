using System;
using System.Collections.Generic;

namespace RestAPIPrueba.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Category { get; set; } = null!;

    public string Warehouse { get; set; } = null!;

    public int Stock { get; set; }
}
