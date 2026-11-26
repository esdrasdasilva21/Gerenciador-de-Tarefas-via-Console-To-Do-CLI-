public sealed record ResumoTarefas(
    int Total,
    int Concluidas,
    int Pendentes,
    int Atrasadas)
{
    public double PercentualConcluido =>
        Total == 0 ? 0d : Math.Round((Concluidas * 100.0) / Total, 1);

    public static ResumoTarefas APartirDe(IEnumerable<TarefaItem>? tarefas)
    {
        if (tarefas is null)
            return Vazio;

        int total = 0;
        int concluidas = 0;
        int pendentes = 0;
        int atrasadas = 0;

        foreach (var tarefa in tarefas)
        {
            if (tarefa is null)
                continue;

            total++;

            if (tarefa.Concluida)
                concluidas++;
            else if (tarefa.EstaAtrasada)
                atrasadas++;
            else
                pendentes++;
        }

        return new ResumoTarefas(total, concluidas, pendentes, atrasadas);
    }

    public static ResumoTarefas Vazio => new(0, 0, 0, 0);
}

public static class TarefasResumoPresenter
{
    public static void ExibirResumo(
        IEnumerable<TarefaItem>? tarefas,
        TextWriter? saida = null)
    {
        var writer = saida ?? Console.Out;
        var resumo = ResumoTarefas.APartirDe(tarefas);

        if (resumo.Total == 0)
        {
            writer.WriteLine("\nResumo: nenhuma tarefa cadastrada.");
            return;
        }

        writer.WriteLine(
            $"\nResumo: {resumo.Concluidas}/{resumo.Total} concluídas ({resumo.PercentualConcluido:0.#}%) " +
            $"| Pendentes: {resumo.Pendentes} | Atrasadas: {resumo.Atrasadas}"
        );
    }
}