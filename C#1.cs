string nomeApp = "TaskFlow C#";
string versao = "0.2";

Console.WriteLine($"=== {nomeApp} (v{versao}) ===");
Console.Write("Digite seu nome: ");
string? nomeUsuario = Console.ReadLine();

if (string.IsNullOrWhiteSpace(nomeUsuario))
{
    nomeUsuario = "Desenvolvedor";
}

Console.WriteLine($"Olá, {nomeUsuario}! Vamos organizar suas tarefas.");

List<string> tarefas = new()
{
    "Estudar sintaxe básica de C#",
    "Configurar o ambiente .NET"
};

bool executando = true;

while (executando)
{
    Console.WriteLine("\n--- Menu ---");
    Console.WriteLine("1 - Listar tarefas");
    Console.WriteLine("2 - Adicionar tarefa");
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
                for (int i = 0; i < tarefas.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tarefas[i]}");
                }
            }
            break;

        case "2":
            Console.Write("Digite a nova tarefa: ");
            string? novaTarefa = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(novaTarefa))
            {
                tarefas.Add(novaTarefa.Trim());
                Console.WriteLine("Tarefa adicionada com sucesso!");
            }
            else
            {
                Console.WriteLine("A tarefa não pode ficar vazia.");
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