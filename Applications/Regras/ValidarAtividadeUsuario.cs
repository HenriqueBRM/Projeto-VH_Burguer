using VH_Burguer.Exceptions;

namespace VH_Burguer.Applications.Regras
{
    public class ValidarAtividadeUsuario
    {
        public static void ValidarUsuario(bool? StatusUsuario)
        {
            if(StatusUsuario == false)
            {
                throw new DomainException("Usuario Invalido");
            }
        }
    }
}
