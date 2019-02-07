using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Licitar
{
    public class Bdi :  IChaveValue
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public double Valor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}