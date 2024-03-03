using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;

namespace Atividade01.Objetos
{
    static class BancoDeDados
    {
        public static List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        
        public static List<Evento> Eventos { get; set; } = new List<Evento>();

        public static List<UsuarioXEvento> UsuariosXEventos { get; set; } = new List<UsuarioXEvento>();

        private static string DiretorioDeDados
        {
            get
            {
                string diretorio = Directory.GetCurrentDirectory();

                diretorio = Path.Combine(diretorio, "data");

                if (!Directory.Exists(diretorio))
                    Directory.CreateDirectory(diretorio);

                return diretorio;
            }
        }
        
        private static string CaminhoDoArquivoDeEventos
        {
            get
            {
                return Path.Combine(DiretorioDeDados, "events.data");
            }
        }
        
        private static string CaminhoDoArquivoDeUsuarios
        {
            get
            {
                return Path.Combine(DiretorioDeDados, "users.data");
            }
        }
        
        private static string CaminhoDoArquivoDeUsuariosXEventos
        {
            get
            {
                return Path.Combine(DiretorioDeDados, "usersXevents.data");
            }
        }

        public static void CarregarDados()
        {
            string conteudo = "";

            try
            {
                conteudo = File.ReadAllText(CaminhoDoArquivoDeEventos);

                Eventos = JsonSerializer.Deserialize<List<Evento>>(conteudo);
            }
            catch { }

            try
            {
                conteudo = File.ReadAllText(CaminhoDoArquivoDeUsuarios);

                Usuarios = JsonSerializer.Deserialize<List<Usuario>>(conteudo);
            }
            catch { }
            
            try
            {
                conteudo = File.ReadAllText(CaminhoDoArquivoDeUsuariosXEventos);

                UsuariosXEventos = JsonSerializer.Deserialize<List<UsuarioXEvento>>(conteudo);
            }
            catch { }
        }

        public static void SalvarEvento(Evento evento)
        {
            if (Eventos.Count == 0)
                CarregarDados();

            if (evento.Id == 0)
            {
                // item novo no banco -> CREATE
                evento.Id = Eventos.Count + 1;

                Eventos.Add(evento);

                string conteudo = JsonSerializer.Serialize(
                    Eventos,
                    new JsonSerializerOptions() { WriteIndented = true }
                );

                File.WriteAllText(CaminhoDoArquivoDeEventos, conteudo);
            }
            else
            {
                // item que já existe -> UPDATE
            }
        }

        public static void SalvarUsuario(Usuario usuario)
        {
            if (Usuarios.Count == 0)
                CarregarDados();

            if (usuario.Id == 0)
            {
                //novo usuario
                usuario.Id = Usuarios.Count + 1;

                Usuarios.Add(usuario);

                string conteudo = JsonSerializer.Serialize(
                    Usuarios,
                    new JsonSerializerOptions() { WriteIndented = true }
                );

                File.WriteAllText(CaminhoDoArquivoDeUsuarios, conteudo);

            }
            else
            {
                //atualizar usuario
            }
        }

        public static Usuario RealizarLogin(string nomeDeUsuario, string senha)
        {
            if (Usuarios.Count == 0)
                CarregarDados();

            foreach (var usuario in Usuarios)
            {
                if (usuario.NomeDeUsuario == nomeDeUsuario && usuario.Senha == senha)
                    return usuario;
            }

            return null;
        }

        public static void SalvarUsuarioXEvento(Usuario usuario, Evento evento)
        {
            UsuarioXEvento novaParticipacao = new UsuarioXEvento();
   
            novaParticipacao.IdUsuario = usuario.Id;
            novaParticipacao.IdEvento = evento.Id;

            foreach (var participacao in UsuariosXEventos)
            {
                if (participacao.IdUsuario == novaParticipacao.IdUsuario
                    && participacao.IdEvento == novaParticipacao.IdEvento)
                {
                    throw new Exception("P_EX");
                }
            }

            UsuariosXEventos.Add(novaParticipacao);

            string conteudo = JsonSerializer.Serialize(
                UsuariosXEventos,
                new JsonSerializerOptions() { WriteIndented = true }
            );

            File.WriteAllText(CaminhoDoArquivoDeUsuariosXEventos, conteudo);
        }

        public static List<Evento> EventosQueUsuarioVai(Usuario usuario)
        {
            List<Evento> lista = new List<Evento>();

            foreach (var participacao in UsuariosXEventos)
            {
                if (participacao.IdUsuario == usuario.Id)
                {
                    foreach (var evento in Eventos)
                    {
                        if (evento.Id == participacao.IdEvento)
                            lista.Add(evento);
                    }
                }
            }

            return lista;
        }

        public static void ExcluirUsuarioXEvento(Usuario usuario, Evento evento)
        {
            int indiceParaExcluir = -1;

            for (int i = 0; i < UsuariosXEventos.Count; i++)
            {
                if (UsuariosXEventos[i].IdUsuario == usuario.Id
                    && UsuariosXEventos[i].IdEvento == evento.Id)
                    indiceParaExcluir = i;
            }

            if (indiceParaExcluir >= 0)
            {
                UsuariosXEventos.RemoveAt(indiceParaExcluir);
                
                string conteudo = JsonSerializer.Serialize(
                    UsuariosXEventos,
                    new JsonSerializerOptions() { WriteIndented = true }
                );

                File.WriteAllText(CaminhoDoArquivoDeUsuariosXEventos, conteudo);
            }
        }

        public static void FiltrarEventos(ref List<Evento> eventosPassados, ref List<Evento> eventosPresentes, ref List<Evento> eventosFuturos)
        {
            eventosPassados = new List<Evento>();
            eventosPresentes = new List<Evento>();
            eventosFuturos = new List<Evento>();

            foreach (var evento in Eventos)
            {
                if (evento.DataFinal < DateTime.Now)
                {
                    eventosPassados.Add(evento);
                }

                if (evento.DataInicial <= DateTime.Now && evento.DataFinal > DateTime.Now)
                {
                    eventosPresentes.Add(evento);
                }

                if (evento.DataInicial > DateTime.Now)
                {
                    eventosFuturos.Add(evento);
                }
            }

            if (eventosFuturos.Count > 1)
            {
                eventosFuturos = eventosFuturos.OrderBy(a => a.DataInicial).ToList();
            }
        }
    }
}
