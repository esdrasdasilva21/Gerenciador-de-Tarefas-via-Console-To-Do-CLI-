Console.Write("Digite seu nome: ");
string? nomeUsuario = Console.ReadLine();

if (string.IsNullOrWhiteSpace(nomeUsuario))
{
    nomeUsuario = "Desenvolvedor";
}

Console.WriteLine($"Olá, {nomeUsuario}! Vamos organizar suas tarefas.\n");