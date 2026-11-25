using TodoCli;

string nomeApp = "TaskFlow C#";
string versao = "0.3";

Console.WriteLine($"=== {nomeApp} (v{versao}) ===");
Console.Write("Digite seu nome: ");
string? nomeUsuario = Console.ReadLine();

if (string.IsNullOrWhiteSpace(nomeUsuario))
{
    nomeUsuario = "Desenvolvedor";
}

Console.WriteLine($"Olá, {nomeUsuario}! Vamos organizar suas tarefas.");

List<TarefaItem> tarefas = new()
{
    new TarefaItem(1, "Estudar sintaxe básica de C#"),
    new TarefaItem(2, "Configurar o ambiente .NET")
};

int proximoId = 3;
bool executando = true;

while (executando)
{
    Console.WriteLine("\n--- Menu ---");
    Console.WriteLine("1 - Listar tarefas");
    Console.WriteLine("2 - Adicionar tarefa");
    Console.WriteLine("3 - Remover tarefa");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            Console.WriteLine("\nSuas tarefas:");
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa cadastrada.");
            }
            else
            {
                foreach (TarefaItem tarefa in tarefas)
                {
                    Console.WriteLine($"#{tarefa.Id} - {tarefa.Titulo}");
                }
            }
            break;

        case "2":
            Console.Write("Digite o título da nova tarefa: ");
            string? titulo = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(titulo))
            {
                tarefas.Add(new TarefaItem(proximoId, titulo.Trim()));
                proximoId++;
                Console.WriteLine("Tarefa adicionada com sucesso!");
            }
            else
            {
                Console.WriteLine("O título não pode ficar vazio.");
            }
            break;

        case "3":
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Não há tarefas para remover.");
                break;
            }

            Console.Write("Digite o ID da tarefa para remover: ");
            string? entradaId = Console.ReadLine();

            if (int.TryParse(entradaId, out int idRemover))
            {
                TarefaItem? alvo = tarefas.FirstOrDefault(t => t.Id == idRemover);
                if (alvo is not null)
                {
                    tarefas.Remove(alvo);
                    Console.WriteLine($"Tarefa #{alvo.Id} (\"{alvo.Titulo}\") removida.");
                }
                else
                {
                    Console.WriteLine("Tarefa não encontrada.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            break;

            

        case "0":
            executando = false;
            Console.WriteLine("Até logo!");
            break;

        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}