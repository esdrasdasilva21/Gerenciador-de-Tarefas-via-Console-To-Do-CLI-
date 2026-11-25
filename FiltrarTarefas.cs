static void FiltrarTarefas(List<TarefaItem> tarefas)
{
    Console.WriteLine("1 - Apenas pendentes");
    Console.WriteLine("2 - Apenas concluídas");
    Console.WriteLine("3 - Apenas atrasadas");
    Console.WriteLine("4 - Por categoria");
    Console.Write("Escolha o filtro: ");
    string? filtro = Console.ReadLine();

    List<TarefaItem> resultado;

    switch (filtro)
    {
        case "1":
            resultado = tarefas.Where(t => !t.Concluida).ToList();
            break;

        case "2":
            resultado = tarefas.Where(t => t.Concluida).ToList();
            break;

        case "3":
            resultado = tarefas.Where(t => t.EstaAtrasada).ToList();
            break;

        case "4":
            Console.Write("Digite o nome da categoria: ");
            string? categoriaBusca = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(categoriaBusca))
            {
                Console.WriteLine("Categoria inválida.");
                return;
            }

            resultado = tarefas
                .Where(t => t.Categoria.Equals(categoriaBusca, StringComparison.OrdinalIgnoreCase))
                .ToList();
            break;

        default:
            Console.WriteLine("Filtro inválido.");
            return;
    }

    if (resultado.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa encontrada para esse filtro.");
        return;
    }

    Console.WriteLine("\nResultado:");
    foreach (TarefaItem item in resultado)
    {
        Console.WriteLine(item);
    }
}