List<string> tarefas = new()
{
    "Estudar sintaxe básica de C#",
    "Configurar o ambiente .NET"
};

Console.WriteLine("Tarefas iniciais:");
foreach (string tarefa in tarefas)
{
    Console.WriteLine($"- {tarefa}");
}