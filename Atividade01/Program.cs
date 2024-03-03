
using System;
using System.Linq;
using System.Collections.Generic;
using Atividade01.Objetos;

namespace Atividade01
{
    class Program
    {
        private static Usuario usuarioLogado;

        static void Main(string[] args)
        {
            // inicia com menu de opcoes
            //   - listagem de eventos
            //   - cadastro
            //   - login
            // - listar eventos
            // - cadastrar usuario
            // - login
            // - cadastrar eventos
            // - participar do evento
            // - cancelar participacao
            // - listar os eventos que o usuario vai participar

            string desejaSair = "";

            while (desejaSair != "S")
            {
                ExibirMenuPrincipal();

                Console.Write("Deseja sair (s/n)? ");

                desejaSair = Console.ReadLine().ToUpper();
            }
        }

        static void ExibirMenuPrincipal()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("=    Menu Principal     =");
            Console.WriteLine("=========================");
            Console.WriteLine("");
            Console.WriteLine("Escolha sua opção:");
            Console.WriteLine("1) Listagem de Eventos");
            Console.WriteLine("2) Cadastro de Eventos");
            Console.WriteLine("3) Cadastro de Usuário");
            Console.WriteLine("4) Login");

            if (usuarioLogado != null)
            {
                Console.WriteLine("5) Participar de evento");
                Console.WriteLine("6) Cancelar participação de evento");
            }

            var opcao = Console.ReadLine().Trim();

            if (opcao == "1")
            {
                ListarEventos();
            }
            else if (opcao == "2")
            {
                CadastrarEvento();
            }
            else if (opcao == "3")
            {
                CadastrarUsuario();
            }
            else if (opcao == "4")
            {
                RealizarLogin();
            }
            else if (opcao == "5" && usuarioLogado != null)
            {
                ParticiparDeEvento();
            }
            else if (opcao == "6" && usuarioLogado != null)
            {
                CancelarParticipacao();
            }
            else
            {
                Console.WriteLine("Opção inválida.");
                Console.WriteLine("");

                ExibirMenuPrincipal();
            }
        }

        static void ListarEventos()
        {
            Console.WriteLine("Listagem de Eventos:");

            BancoDeDados.CarregarDados();

            List<Evento> eventosPassados = new List<Evento>();
            List<Evento> eventosPresentes = new List<Evento>();
            List<Evento> eventosFuturos = new List<Evento>();

            BancoDeDados.FiltrarEventos(ref eventosPassados, ref eventosPresentes, ref eventosFuturos);

            if (eventosPassados.Count > 0)
            {
                Console.WriteLine("Eventos Passados:");

                foreach (var evento in eventosPassados)
                {
                    Console.WriteLine($"{ evento.DataInicial.ToString("dd/MM/yy HH:mm") } { evento.Nome }");
                    Console.WriteLine("               " + evento.Descricao);
                    Console.WriteLine("");
                }
            }
            
            if (eventosPresentes.Count > 0)
            {
                Console.WriteLine("Eventos ocorrendo AGORA:");

                foreach (var evento in eventosPresentes)
                {
                    Console.WriteLine($"{ evento.DataInicial.ToString("dd/MM/yy HH:mm") } { evento.Nome }");
                    Console.WriteLine("               " + evento.Descricao);
                    Console.WriteLine("");
                }
            }
            
            if (eventosFuturos.Count > 0)
            {
                Console.WriteLine("Eventos no futuro:");

                foreach (var evento in eventosFuturos)
                {
                    Console.WriteLine($"{ evento.DataInicial.ToString("dd/MM/yy HH:mm") } { evento.Nome }");
                    Console.WriteLine("               " + evento.Descricao);
                    Console.WriteLine("");
                }
            }

            if (eventosPassados.Count == 0
            &&  eventosPresentes.Count == 0
            &&  eventosFuturos.Count == 0)
            {
                Console.WriteLine("Nenhum evento cadastrado.");
            }
        }

        static void CadastrarEvento()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("=  Cadastro de Eventos  =");
            Console.WriteLine("=========================");
            Console.WriteLine("");

            var novoEvento = new Evento();

            Console.Write("Nome do Evento: ");

            novoEvento.Nome = Console.ReadLine();
            
            Console.Write("Descrição do Evento: ");

            novoEvento.Descricao = Console.ReadLine();

            DateTime dataDigitada = DateTime.MinValue;

            while (dataDigitada == DateTime.MinValue)
            {
                dataDigitada = new DateTime(
                                            DateTime.Now.Year,
                                            DateTime.Now.Month,
                                            DateTime.Now.Day,
                                            DateTime.Now.Hour, 0, 0);

                Console.Write("Data Inicial: " + dataDigitada.ToString("dd/MM/yyyy HH:mm "));

                string valorDigitado = Console.ReadLine();

                if (valorDigitado != "")
                {
                    try
                    {
                        dataDigitada = DateTime.Parse(valorDigitado);

                        novoEvento.DataInicial = dataDigitada;
                    }
                    catch
                    {
                        Console.WriteLine("Data inválida.");
                    }
                }
                else
                {
                    novoEvento.DataInicial = dataDigitada;
                }
            }
            
            dataDigitada = DateTime.MinValue;

            while (dataDigitada == DateTime.MinValue)
            {
                dataDigitada = new DateTime(
                                            DateTime.Now.Year,
                                            DateTime.Now.Month,
                                            DateTime.Now.Day,
                                            DateTime.Now.Hour + 3, 0, 0);

                Console.Write("Data Final: " + dataDigitada.ToString("dd/MM/yyyy HH:mm "));
                
                string valorDigitado = Console.ReadLine();

                if (valorDigitado != "")
                {
                    try
                    {
                        dataDigitada = DateTime.Parse(valorDigitado);

                        novoEvento.DataFinal = dataDigitada;
                    }
                    catch
                    {
                        Console.WriteLine("Data inválida.");
                    }
                }
                else
                {
                    novoEvento.DataFinal = dataDigitada;
                }
            }
            
            Console.Write("Endereço do Evento: ");

            novoEvento.Endereco = Console.ReadLine();

            while (novoEvento.Endereco.Length < 5)
            {
                Console.Write("Endereço muito curto, digite o endereço completo: ");

                novoEvento.Endereco = Console.ReadLine();
            }
            
            Console.WriteLine("Selecione uma categoria: ");

            List<CategoriaDeEvento> categorias = new List<CategoriaDeEvento>();
                
            var valores = Enum.GetValues(typeof(CategoriaDeEvento)).Cast<CategoriaDeEvento>();

            categorias.AddRange(valores);

            for (int i = 0; i < categorias.Count; i++)
            {
                Console.WriteLine((i + 1) + ") " + categorias[i]);
            }

            string opcaoDigitada = Console.ReadLine();

            int opcao = -1;

            while (opcao == -1)
            {
                try
                {
                    opcao = Convert.ToInt32(opcaoDigitada);

                    if (opcao > 0 && opcao <= categorias.Count)
                    {
                        // ok
                        novoEvento.Categoria = categorias[opcao - 1];
                    }
                    else
                    {
                        Console.WriteLine($"Opção inválida, digite de 1 a { categorias.Count }");

                        opcaoDigitada = Console.ReadLine();

                        opcao = -1;
                    }
                }
                catch 
                {
                    Console.WriteLine($"Opção inválida, digite de 1 a { categorias.Count }");

                    opcaoDigitada = Console.ReadLine();
                }
            }

            BancoDeDados.SalvarEvento(novoEvento);

            Console.WriteLine("Evento salvado com sucesso.");

            ExibirMenuPrincipal();
        }

        static void CadastrarUsuario()
        {
            
            Console.WriteLine("=========================");
            Console.WriteLine("=  Cadastro de Usuário  =");
            Console.WriteLine("=========================");
            Console.WriteLine("");

            Usuario novoUsuario = new Usuario();

            Console.WriteLine("Nome Completo:");

            novoUsuario.NomeCompleto = Console.ReadLine();

            while(novoUsuario.NomeCompleto == "")
            {
                Console.WriteLine("Nome não pode ser vazio. Digite o nome:");
                
                novoUsuario.NomeCompleto = Console.ReadLine();
            }

            Console.WriteLine("Email:");

            novoUsuario.Email = Console.ReadLine();

            while(novoUsuario.Email == "" || !novoUsuario.Email.Contains("@"))
            {
                Console.WriteLine("Email inválido. Digite o email:");

                novoUsuario.Email = Console.ReadLine();
            }
            
            Console.WriteLine("Nome de Usuário:");

            novoUsuario.NomeDeUsuario = Console.ReadLine();

            while(novoUsuario.NomeDeUsuario == "")
            {
                Console.WriteLine("Nome de Usuário não pode ser vazio. Digite o nome de usuário:");

                novoUsuario.NomeDeUsuario = Console.ReadLine();
            }
            
            Console.WriteLine("Senha:");

            novoUsuario.Senha = Console.ReadLine();

            BancoDeDados.SalvarUsuario(novoUsuario);
            
            Console.WriteLine("Usuário salvado com sucesso!");
            Console.WriteLine("");

            ExibirMenuPrincipal();
        }

        static void RealizarLogin()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("=         Login         =");
            Console.WriteLine("=========================");
            Console.WriteLine("");

            string nome, senha;

            Console.Write("Nome de usuário: ");

            nome = Console.ReadLine();

            while(nome == "")
            {
                Console.Write("Nome de usuário: ");
                
                nome = Console.ReadLine();
            }
            
            Console.Write("Senha: ");

            senha = Console.ReadLine();

            while(senha == "")
            {
                Console.Write("Senha: ");
                
                senha = Console.ReadLine();
            }

            // banco faz login
            usuarioLogado = BancoDeDados.RealizarLogin(nome, senha);

            if (usuarioLogado == null)
            {
                Console.WriteLine("Login inválido.");
            }
            else
            {
                Console.WriteLine("Login com sucesso! Olá " + usuarioLogado.NomeCompleto);
            }
            
            Console.WriteLine("");

            ExibirMenuPrincipal();
        }

        static void ParticiparDeEvento()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("= Participar de Evento  =");
            Console.WriteLine("=========================");
            Console.WriteLine("");

            Console.Write("Qual evento deseja ir (digite n para voltar)? ");

            for (int i = 0; i < BancoDeDados.Eventos.Count; i++)
            {
                var evento = BancoDeDados.Eventos[i];

                Console.Write($"\r\n{ (i + 1) }) { evento.Nome } - de {evento.DataInicial:dd/MM/yy HH:mm} até {evento.DataFinal:dd/MM/yy HH:mm}");
            }

            Console.WriteLine("");

            int opcao = -1;

            string entrada = Console.ReadLine();

            if (entrada.ToUpper() == "N")
                ExibirMenuPrincipal();

            while (opcao < 0 || opcao > BancoDeDados.Eventos.Count)
            {
                try
                {
                    opcao = Convert.ToInt32(entrada);

                    if (opcao < 0 || opcao > BancoDeDados.Eventos.Count)
                    {
                        Console.WriteLine($"Opção inválida, escolha de 1 a { BancoDeDados.Eventos.Count }");

                        entrada = Console.ReadLine();

                        opcao = -1;
                    }
                }
                catch
                {
                    Console.WriteLine($"Opção inválida, escolha de 1 a { BancoDeDados.Eventos.Count }");

                    entrada = Console.ReadLine();
                }
            }

            var eventoEscolhido = BancoDeDados.Eventos[opcao - 1];

            try
            {
                BancoDeDados.SalvarUsuarioXEvento(usuarioLogado, eventoEscolhido);

                Console.WriteLine("Participação salvada com sucesso!");
            }
            catch (Exception x)
            {
                if (x.Message == "P_EX")
                {
                    Console.WriteLine("Você já está cadastrado para esse evento.");
                }
                else
                {
                    throw;
                }
            }

            ExibirMenuPrincipal();
        }
        
        static void CancelarParticipacao()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("= Cancelar Participação =");
            Console.WriteLine("=========================");
            Console.WriteLine("");

            var eventos = BancoDeDados.EventosQueUsuarioVai(usuarioLogado);

            if (eventos.Count > 0)
            {
                Console.WriteLine("Eventos que você está participando:");

                for (int i = 0; i < eventos.Count; i++)
                {
                    var ev = eventos[i];

                    Console.WriteLine($"{ (i + 1) } ) { ev.Nome } - de {ev.DataInicial:dd/MM/yy HH:mm} até {ev.DataFinal:dd/MM/yy HH:mm}");
                }

                Console.Write("Qual deseja cancelar (digite n para voltar)? ");

                int opcao = -1;

                string entrada = Console.ReadLine();

                if (entrada.ToUpper() == "N")
                    ExibirMenuPrincipal();

                while (opcao < 0 || opcao > eventos.Count)
                {
                    try
                    {
                        opcao = Convert.ToInt32(entrada);

                        if (opcao < 0 || opcao > eventos.Count)
                        {
                            Console.WriteLine($"Opção inválida, escolha de 1 a { eventos.Count }");

                            entrada = Console.ReadLine();

                            opcao = -1;
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"Opção inválida, escolha de 1 a { eventos.Count }");

                        entrada = Console.ReadLine();
                    }
                }

                var eventoEscolhido = BancoDeDados.Eventos[opcao - 1];

                BancoDeDados.ExcluirUsuarioXEvento(usuarioLogado, eventoEscolhido);

                Console.WriteLine("Participação cancelada... =(");

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Você não marcou participação em nenhum evento.");
            }

            ExibirMenuPrincipal();
        }
    }
}
