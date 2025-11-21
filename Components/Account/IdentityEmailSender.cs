using DiarioMagna.Components.Account.Pages.Manage;
using DiarioMagna.Data;
using DiarioMagna.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DiarioMagna.Components.Account;
    internal sealed class IdentityEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly IIdentityEmailService _emailSender;

        public IdentityEmailSender(IIdentityEmailService emailSender)
        {
            _emailSender = emailSender;
        }

      public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        _emailSender.SendEmailAsync(
        email,
            "Confirmación de registro en Diario Magna",
            $@"
                <p>Hola {user.UserName},</p>
                <p>Tu cuenta en <strong>Diario Magna</strong> ha sido creada exitosamente.</p>
                <p>Confirma tu correo haciendo <a href='{confirmationLink}'>clic aquí</a>.</p>
                <p>Gracias,<br>El equipo de Diario Magna</p>
            "
        );
      public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        _emailSender.SendEmailAsync(
            email,
            "Restablecimiento de contraseña",
            $@"
                <p>Hola {user.UserName},</p>
                <p>Se ha solicitado restablecer tu contraseña en <strong>Diario Magna</strong>.</p>
                <p>Haz clic en el siguiente enlace para restablecer tu contraseña: <a href='{resetLink}'>Restablecer contraseña</a>.</p>
                <p>Si no solicitaste este cambio, ignora este correo.</p>
            "
        );

      public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        _emailSender.SendEmailAsync(
            email,
            "Código de restablecimiento de contraseña",
            $@"
                <p>Hola {user.UserName},</p>
                <p>Usa el siguiente código para restablecer tu contraseña en <strong>Diario Magna</strong>:</p>
                <h3>{resetCode}</h3>
                <p>Si no solicitaste este código, ignora este correo.</p>
            "
        );
    }
