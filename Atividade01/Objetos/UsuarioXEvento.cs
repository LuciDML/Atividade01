using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atividade01.Objetos
{
    class UsuarioXEvento
    {
        public int IdUsuario { get; set; }

        public int IdEvento { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
