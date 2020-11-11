using System;
using System.Collections.Generic;
using System.Text;

namespace CicekSepeti.Infra.Data.Entity
{
   public class Product:BaseEntityWithDate
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Piece { get; set; }
       
    }
}
