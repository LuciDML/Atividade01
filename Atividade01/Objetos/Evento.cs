using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atividade01.Objetos
{
    class Evento
    {
        public int Id { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public string Nome { get; set; }

        public string Endereco { get; set; }

        public CategoriaDeEvento Categoria { get; set; }

        public string Descricao { get; set; }
    }
}
