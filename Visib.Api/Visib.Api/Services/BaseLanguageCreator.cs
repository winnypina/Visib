using System;
using System.Threading.Tasks;
using Visib.Api.Data;
using Visib.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Visib.Api.Services
{
    public interface ICreateBaseLanguage
    {
        Task Create();
    }

    public class BaseLanguageCreator : ICreateBaseLanguage
    {
        private readonly ApplicationDbContext _context;

        public BaseLanguageCreator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create()
        {
            var languageTest = await _context.TextTranslations.FirstOrDefaultAsync();
            if (languageTest == null)
            {
                await CreateWelcomeScreen();
                await CreateSiginScreen();
                await CreateDeleteAccount();
                await CreateEditProfile();
                await CreateInviteFriends();
                await CreateLogoff();
                await CreateMenu();
                await CreateMyProfile();
                await CreatePrivacyPolicy();
                await CreateRecoverPassword();
                await CreateSignup();

                await CreateSignupProfileType();
                await CreateSignupLoginType();
                await CreateVerificationCode();
                await CreateUseTerms();

                await _context.SaveChangesAsync();
            }
        }

        private async Task CreateWelcomeScreen()
        {
            var screen = "Inicial";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Entrar",
                "Entrar", "Signin", "Entrar"));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Cadastrar",
                "Cadastrar", "Signup", "Cadastrar"));
        }

        private async Task CreateSiginScreen()
        {
            var screen = "Login";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Entrar", "Signin", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Texto Principal",
                "Aqui consumidores e fornecedores se encontram de forma geolocalizada",
                "Here consumers and providers find each other on a localized way",
                "Aqui consumidores e fornecedores se encontram de forma geolocalizada"));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Login",
                "Telefone ou email", "Phone or email", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Senha",
                "Senha", "Password", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Entrar",
                "Entrar", "Login", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Recuperar",
                "Recuperar Senha", "Recover Password", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Facebook",
                "Entrar com Facebook", "Login with Facebook", ""));
        }

        private async Task CreateSignupProfileType()
        {
            var screen = "Cadastro Perfil";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Cadastrar", "Signup", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Texto Principal",
                "Escolha seu tipo de perfil", "Choose your profile type", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Consumidor",
                "Consumidor", "Consumer", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Fornecedor",
                "Fornecedor", "Provider", ""));
        }

        private async Task CreateSignupLoginType()
        {
            var screen = "Cadastro Tipo Login";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Cadastrar", "Signup", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Texto Principal",
                "Escolha seu tipo de cadastro", "Choose your signup method", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Login",
                "Email ou telefone", "Phone or email", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Facebook",
                "Facebook", "Facebook", ""));
        }

        private async Task CreateSignup()
        {
            var screen = "Campos Cadastro";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo telefone",
                "Cadastro Telefone", "Signup with phone", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo email",
                "Cadastro Email", "Signup with email", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Texto Principal",
                "Aqui consumidores e fornecedores se encontram de forma geolocalizada",
                "Here consumers and providers find each other on a localized way", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Nome",
                "Nome", "Name", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Telefone",
                "Telefone", "Phone", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Email",
                "Email", "Email", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Senha",
                "Senha", "Password", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Confirmar Senha",
                "Confirmar senha", "Confirm password", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Confirmação Email",
                "Um código de verificação será enviado para o email", "A verification code will be sent to the email",
                ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Confirmação Telefone",
                "Um código de verificação será enviado para o número", "A verification has been sent to the number",
                ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Confirmar",
                "Confirmar", "Confirm", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Cancelar",
                "Cancelar", "Cancel", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Cadastrar",
                "Cadastrar", "Signup", ""));
        }

        private async Task CreateVerificationCode()
        {
            var screen = "Codigo Verificacao";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo telefone",
                "Código Telefone", "Phone code", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo email",
                "Código Email", "Email code", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Texto Principal Telefone",
                "Digite o código que recebeu por SMS", "Type in the code that was texted to your phone", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Texto Principal Email",
                "Digite o código que recebeu por email", "Type in the code that was mailed to you", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Não recebi",
                "Não recebi um código", "I didn't receive a code", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Reenvio",
                "Código reenviado com sucesso", "Code sent again sucessfully", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Confirmar",
                "Confirmar", "Confirm", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Sucesso Titulo",
                "Parabéns! Cadastro realizado com sucesso", "Congratulations! You signed up sucesfully", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Sucesso Descrição",
                "Para uma maior interação com seus parceiros potenciais, sugerimos completar seu perfil",
                "For a better interaction with your potential partner, we suggest that you finish filling your profile.",
                ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Sucesso Cancelar",
                "Cancelar", "Cancel", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Sucesso Confirmação",
                "Confirmar", "Confirm", ""));
        }

        private async Task CreateRecoverPassword()
        {
            var screen = "Recuperar Senha";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Texto Principal",
                "Digite seu email aqui, que enviaremos um link para redefinir sua senha",
                "Type your email here that we will send you a link to redefine your password", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Login",
                "Telefone ou email", "Phone or email", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Enviar",
                "Enviar", "Send", ""));
        }

        private async Task CreateEditProfile()
        {
            var screen = "Editar Perfil";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Editar Perfil", "Edit Profile", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Nome",
                "Nome", "Name", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Data Nascimento",
                "Data de Nascimento", "Birth Date", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Sobre",
                "Sobre", "About", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Site",
                "Site", "Website", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Email",
                "Email", "Email", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Telefone",
                "Telefone", "Mobile", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Telefone Negócio",
                "Telefone do negócio", "Business Phone", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Endereço",
                "Endereço", "Address", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Numero",
                "Número", "Number", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Complemento",
                "Complemento", "Line 2", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Cep",
                "Cep", "Zip code", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Cidade",
                "Cidade", "City", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Estado",
                "Estado", "State", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "País",
                "País", "Country", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Língua Portugues",
                "Português(pt-BR)", "Portuguese(pt-BR)", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Língua Inglês",
                "Inglês(en-US)", "English(en-US)", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Língua Espanhol",
                "Espanhol(es-ES)", "Spanish(es-ES)", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Salvar",
                "Salvar", "Save", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Alterar senha",
                "Alterar senha", "Change password", ""));
        }

        private async Task CreateMenu()
        {
            var screen = "Menu";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Escanear",
                "Escanear", "Scan", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Convidar Amigos",
                "Convidar Amigos", "Invite friends", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Perfil",
                "Perfil", "Profile", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Editar Perfil",
                "Editar Perfil", "Edit Profile", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Termos de uso",
                "Termos de uso", "Termos of use", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Política de privacidade",
                "Política de privacidade", "Privacy Policy", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Deletar conta",
                "Deletar conta", "Delete account", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Sair da sua conta",
                "Sair da sua conta", "Logoff", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Versão",
                "Versão", "Version", ""));
        }

        private async Task CreateInviteFriends()
        {
            var screen = "Convidar amigos";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Convidar amigos", "Invite Friends", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Convidar",
                "Convidar", "Invite", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Sucesso",
                "Email(s) enviado(s) com sucesso", "Email sent sucessfully", ""));
        }

        private async Task CreateMyProfile()
        {
            var screen = "Meu Perfil";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Meu perfil", "My profile", ""));
        }

        private async Task CreateUseTerms()
        {
            var screen = "Termos de uso";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Termos de uso", "Termos of use", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Aceitar",
                "Aceito os termos de uso", "Accept terms of use", ""));
        }

        private async Task CreatePrivacyPolicy()
        {
            var screen = "Privacidade";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Titulo",
                "Política de privacidade", "Privacy policy", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Aceitar",
                "Aceito a política de privacidade", "Accept privacy policy", ""));
        }

        private async Task CreateDeleteAccount()
        {
            var screen = "Deletar conta";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Mensagem",
                "Deseja realmente remover esta conta. Este procedimento irá apagar todos os seus dados definitivamente",
                "Do you really want to delete your account? This will erase all of your data.", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Confirmar",
                "Confirmar", "Confirm", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Cancelar",
                "Cancelar", "Cancel", ""));
        }

        private async Task CreateLogoff()
        {
            var screen = "Sair conta";
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Mensagem",
                "Deseja realmente sair da sua conta?", "Do you really want to logoff?", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Confirmar",
                "Confirmar", "Confirm", ""));
            await _context.TextTranslations.AddAsync(CreateTranslation(screen, "Cancelar",
                "Cancelar", "Cancel", ""));
        }


        private TextTranslation CreateTranslation(string screen, string key, string value, string valueEnUs,
            string valueEs)
        {
            var translation = new TextTranslation
            {
                Key = key,
                Screen = screen,
                Value = value,
                ValueEnUs = valueEnUs,
                ValueEs = valueEs,
                Id = Guid.NewGuid()
            };
            return translation;
        }
    }
}