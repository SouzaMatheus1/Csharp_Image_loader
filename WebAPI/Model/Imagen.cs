using System;
using System.Collections.Generic;

namespace WebAPI.Model
{
    public partial class Imagen
    {
        public int Id { get; set; }
        public string Uri { get; set; } = null!;
        public string Title { get; set; } = null!;
        public byte[] Bytes { get; set; } = null!;
    }
}
