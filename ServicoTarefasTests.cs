using TodoCli;
using Xunit;

namespace TodoCli.Tests;

public class ServicoTarefasTests
{
    private static ServicoTarefas CriarServico(out RepositorioMemoriaFake repo)
    {
        repo = new RepositorioMemoriaFake
        {
            Dados = new List<TarefaItem>
            {
                new(1, "Estudar LINQ e C#", PrioridadeTarefa.Media, "Estudo"),
                new(2, "Entregar relatório", PrioridadeTarefa.Alta, "Trabalho", DateOnly.FromDateTime(DateTime.Today.AddDays(-2)))
            }
        };

        return new ServicoTarefas(repo);
    }

    [Fact]
    public void Adicionar_DeveCriarTarefaComIdIncrementalESalvar()
    {
        ServicoTarefas servico = CriarServico(out RepositorioMemoriaFake repo);

        TarefaItem criada = servico.Adicionar("Revisar padrões .NET", PrioridadeTarefa.Alta, "Projeto");

        Assert.Equal(3, criada.Id);
        Assert.Equal("Revisar padrões .NET", criada.Titulo);
        Assert.Equal(PrioridadeTarefa.Alta, criada.Prioridade);
        Assert.Equal("Projeto", criada.Categoria);
        Assert.False(criada.Concluida);
        Assert.Equal(3, servico.ListarTodas().Count);
        Assert.True(repo.ContagemSalvamentos > 0);
    }

    [Fact]
    public void Adicionar_ComTituloVazio_DeveLancarArgumentException()
    {
        ServicoTarefas servico = CriarServico(out _);

        Assert.Throws<ArgumentException>(() => servico.Adicionar("   "));
    }

    [Fact]
    public void AlternarConclusao_DeveMarcarEDesmarcarTarefa()
    {
        ServicoTarefas servico = CriarServico(out _);

        TarefaItem? marcada = servico.AlternarConclusao(1);
        Assert.NotNull(marcada);
        Assert.True(marcada!.Concluida);

        TarefaItem? reaberta = servico.AlternarConclusao(1);
        Assert.NotNull(reaberta);
        Assert.False(reaberta!.Concluida);
    }

    [Fact]
    public void FiltrarAtrasadas_DeveRetornarApenasPendentesComPrazoVencido()
    {
        ServicoTarefas servico = CriarServico(out _);

        List<TarefaItem> atrasadas = servico.FiltrarAtrasadas();

        Assert.Single(atrasadas);
        Assert.Equal(2, atrasadas[0].Id);
        Assert.True(atrasadas[0].EstaAtrasada);
    }

    [Fact]
    public void FiltrarPorCategoria_DeveIgnorarMaiusculasEMinusculas()
    {
        ServicoTarefas servico = CriarServico(out _);

        List<TarefaItem> resultado = servico.FiltrarPorCategoria("estudo");

        Assert.Single(resultado);
        Assert.Equal(1, resultado[0].Id);
    }

    [Fact]
    public void OrdenarPorPrioridade_DeveColocarAltaAntesDeMedia()
    {
        ServicoTarefas servico = CriarServico(out _);

        List<TarefaItem> ordenadas = servico.OrdenarPorPrioridade();

        Assert.Equal(PrioridadeTarefa.Alta, ordenadas[0].Prioridade);
        Assert.Equal(PrioridadeTarefa.Media, ordenadas[1].Prioridade);
    }
}