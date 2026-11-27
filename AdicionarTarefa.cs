private static PrioridadeTarefa LerPrioridade()
{
    while (true)
    {
        Console.Write("Prioridade (1 - Baixa, 2 - Média, 3 - Alta) [padrão 2]: ");
        string? entrada = Console.ReadLine()?.Trim();

        PrioridadeTarefa? prioridade = entrada switch
        {
            "" or "2" => PrioridadeTarefa.Media,
            "1" => PrioridadeTarefa.Baixa,
            "3" => PrioridadeTarefa.Alta,
            _ => null
        };

        if (prioridade is not null)
        {
            return prioridade.Value;
        }

        Console.WriteLine("Opção inválida. Digite 1, 2, 3 ou pressione Enter.");
    }
}