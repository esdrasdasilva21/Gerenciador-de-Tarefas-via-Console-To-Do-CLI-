record ResumoTarefas(int Total, int Concluidas, int Pendentes, int Atrasadas)
{
    public double PercentualConcluido => Total > 0 ? (Concluidas * 100.0) / Total : 0;

    public static ResumoTarefas Calcular(IEnumerable<TarefaItem>? tarefas)
    {
        if (tarefas is null)
            return new ResumoTarefas(0, 0, 0, 0);

        int total = 0, concluidas = 0, pendentes = 0, atrasadas = 0;

        foreach (var t in tarefas)
        {
            if (t is null) continue;
            total++;

            if (t.Concluida) concluidas++;
            else if (t.EstaAtrasada) atrasadas++;
            else pendentes++;
        }

        return new ResumoTarefas(total, concluidas, pendentes, atrasadas);
    }
}

static void ExibirResumo(IEnumerable<TarefaItem>? tarefas)
{
    var resumo = ResumoTarefas.Calcular(tarefas);

    Console.WriteLine(
        $"\nResumo: {resumo.Concluidas}/{resumo.Total} concluídas ({resumo.PercentualConcluido:0.#}%) " +
        $"| {resumo.Pendentes} pendente(s) | {resumo.Atrasadas} atrasada(s)"
    );
}