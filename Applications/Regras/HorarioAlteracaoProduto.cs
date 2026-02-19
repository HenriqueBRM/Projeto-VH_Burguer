using VH_Burguer.Exceptions;

namespace VH_Burguer.Applications.Regras
{
    public class HorarioAlteracaoProduto
    {
        public static void ValidarHorario()
        {
            var agora = DateTime.Now.TimeOfDay;
            var abertura = new TimeSpan(10,0,0);
            var fechamento = new TimeSpan(24,0,0);

            // retorna um true ou false
            var estaAberto = agora >= abertura && agora <= fechamento;

            if (estaAberto)
            {
                throw new DomainException("Produto so pode ser alterado fora do horario de funcionamento");
            }
        }
    }
}
