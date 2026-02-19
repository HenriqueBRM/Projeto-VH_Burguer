using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VH_Burguer.Domains;
using VH_Burguer.Exceptions;

namespace VH_Burguer.Applications.Autenticacao
{
    public class GeradorTokenJwt
    {
        private readonly IConfiguration _config;
        //recebe as configuracoes do appsettings.json
        public GeradorTokenJwt(IConfiguration config)
        {
            _config = config;
        }

        public string GerarToken(Usuario usuario)
        {
            // Key = chave secreta usada para assinar o token
            var chave = _config["Jwt: Key"]!;

            // Issuer = quem gerou o token(nome da Api/sistema que gerou)
            var issuer = _config["Jwt:Issuer"]!;

            // Audience = para quem o token foi criado, define qual sistema pode usar o token
            var audience = _config["Jwt:Audience"]!;

            //Tempo de expiracao = define por quanto tempo o token sera valido, apos esse tempo o usuario deve logar novamente
            var expiraEmMinutos = int.Parse(_config["Jwt:ExpiraEmMinutos"]!);

            //Converte a chave para bytes(necessaria para criar a assinatura)
            var keyBytes = Encoding.UTF8.GetBytes(chave);
            

            //Seguranca: exige uma chave com pelo menos 32 caracteres
            if(keyBytes.Length < 32)
            {
                throw new DomainException("Jwt: Key precisa ter pelo menos 32 caracteres (256 bits)");
            }

            //Cria a chave de seguranca usada para assinar o token
            var securityKey = new SymmetricSecurityKey(keyBytes);

            //Define o algoritmo de assinatura do token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Claims -> informacoes do usuario que vao dentro do token, essas informacoes podem ser recuperadas na API para identificar quem esta logando

            var claims = new List<Claim>
            {
                // ID do usuario
                new Claim(ClaimTypes.NameIdentifier,usuario.UsuarioID.ToString()),

                // Nome do usuario
                new Claim(ClaimTypes.Name, usuario.Nome),

                // Email do usuario
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Cria o token Jwt com todas as informacoes 
            var token = new JwtSecurityToken(
                issuer: issuer, // quem gerou o token
                audience: audience, // quem pode usar o token
                claims: claims, //dados do usuario
                expires: DateTime.Now.AddMinutes(expiraEmMinutos), // validade do token
                signingCredentials: credentials //assinatura de seguranca
            );

            // Converte o token para string e essa string e enviada para o cliente

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
