namespace Aiglusoft.IAM.Server.Controllers
{
  using Aiglusoft.IAM.Application.UseCases.Emails.VerifyEmail;
  using Aiglusoft.IAM.Application.UseCases.Registers.CheckUserEmail;
  using Aiglusoft.IAM.Application.UseCases.Registers.SetUserPassword;
  using Aiglusoft.IAM.Server.Models.V1.Requests;
  using MediatR;
  using Microsoft.AspNetCore.Authentication.JwtBearer;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using System.Security.Claims;

  [ApiController]
    [Route("api/v{version:apiVersion}/register")]
    public class RegisterController : ControllerBase
    {
        private readonly ISender _sender;

        public RegisterController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Crée un nouveau compte utilisateur avec le prénom et le nom.
        /// </summary>
        /// <param name="userDto">Objet contenant le prénom et le nom de l'utilisateur</param>
        /// <returns>Un objet contenant l'ID utilisateur, un token JWT et un message de succès</returns>
        [HttpPost("check")]
        public async Task<IActionResult> CheckUserEmail([FromBody] CheckUserEmailRequest userDto)
        {
            // TODO: Implémenter la logique de création de l'utilisateur
            // 1. Valider les données de l'utilisateur (prénom et nom)
            // 2. Créer un nouvel utilisateur dans la base de données
            // 3. Générer un token JWT pour cet utilisateur
            // 4. Retourner l'ID utilisateur et le token JWT

            var command = new CheckUserEmailCommand { FirstName = userDto.FirstName, Lastname = userDto.LastName, Birthdate = DateOnly.Parse(userDto.Birthdate), Gender = userDto.Gender, Email = userDto.Email };
            var tokenResponse = await _sender.Send(command);
            var jsonResponse = new
            {
                access_token = tokenResponse.AccessToken,
                token_type = tokenResponse.TokenType,
                expires_in = tokenResponse.ExpiresIn,
                id_token = tokenResponse.IdToken,
                refresh_token = tokenResponse.RefreshToken,
                scope = tokenResponse.Scope
            };
            return Ok(jsonResponse);
        }

        /// <summary>
        /// Vérifie le code de vérification envoyé à l'adresse email.
        /// </summary>
        /// <param name="request">Objet contenant le code de vérification</param>
        /// <returns>Un message de succès</returns>
        [HttpPost("email/verify")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailRequest request)
        {
            // TODO: Implémenter la logique de vérification du code email
            // 1. Vérifier que l'userId correspond à celui dans le JWT
            if (request.Email != User.FindFirstValue(ClaimTypes.Email))
            {
                return Unauthorized(new { message = "Unauthorized access" });
            }

            // Convertir la requête en commande
            var command = new VerifyEmailCommand
            {
                Email = request.Email,
                Code = request.Code,
            };

            // 2. Valider le code de vérification
            // 3. Marquer l'adresse email de l'utilisateur comme vérifiée dans la base de données
            var tokenResponse = await _sender.Send(command);

            var jsonResponse = new
            {
                access_token = tokenResponse.AccessToken,
                token_type = tokenResponse.TokenType,
                expires_in = tokenResponse.ExpiresIn,
                id_token = tokenResponse.IdToken,
                refresh_token = tokenResponse.RefreshToken,
                scope = tokenResponse.Scope
            };
            return Ok(jsonResponse);
        }

        /// <summary>
        /// Définit ou met à jour le mot de passe pour le compte utilisateur.
        /// </summary>
        /// <param name="request">Objet contenant le mot de passe et sa confirmation</param>
        /// <returns>Un message de succès</returns>
        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> SetUserPasswordAsync([FromBody] SetUserPasswordRequest request)
        {
            // TODO: Implémenter la logique de définition du mot de passe
            // 1. Vérifier que l'userId correspond à celui dans le JWT
            // 2. Valider que le mot de passe et sa confirmation correspondent et respectent les critères de sécurité
            // 3. Hasher le mot de passe
            // 4. Mettre à jour le mot de passe de l'utilisateur dans la base de données

          

            var command = new SetUserPasswordCommand { Password = request.Password };
            var tokenResponse = await _sender.Send(command);

            var jsonResponse = new
            {
                access_token = tokenResponse.AccessToken,
                token_type = tokenResponse.TokenType,
                expires_in = tokenResponse.ExpiresIn,
                id_token = tokenResponse.IdToken,
                refresh_token = tokenResponse.RefreshToken,
                scope = tokenResponse.Scope
            };
            return Ok(jsonResponse);
        }

        /// <summary>
        /// Ajoute un numéro de téléphone au profil de l'utilisateur et envoie un code de vérification.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur</param>
        /// <param name="phoneDto">Objet contenant le numéro de téléphone</param>
        /// <returns>Un message de succès</returns>
        [HttpPost("phone")]
        [Authorize]
        public IActionResult AddPhone([FromBody] AddPhoneRequest phoneDto)
        {
            // TODO: Implémenter la logique d'ajout du numéro de téléphone et d'envoi du code de vérification
            // 1. Vérifier que l'userId correspond à celui dans le JWT
            // 2. Valider le numéro de téléphone
            // 3. Mettre à jour le numéro de téléphone de l'utilisateur dans la base de données
            // 4. Envoyer un SMS avec un code de vérification

            return Ok(new { message = "Verification SMS sent successfully" });
        }

        /// <summary>
        /// Vérifie le code de vérification envoyé au numéro de téléphone.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur</param>
        /// <param name="verifyPhoneDto">Objet contenant le code de vérification</param>
        /// <returns>Un message de succès</returns>
        [HttpPost("phone/verify")]
        [Authorize]
        public IActionResult VerifyPhone([FromBody] VerifyPhoneRequest verifyPhoneDto)
        {
            // TODO: Implémenter la logique de vérification du code de téléphone
            // 1. Vérifier que l'userId correspond à celui dans le JWT
            // 2. Valider le code de vérification
            // 3. Marquer le numéro de téléphone de l'utilisateur comme vérifié dans la base de données

            return Ok(new { message = "Phone number verified successfully" });
        }

        /// <summary>
        /// Enregistre l'acceptation des conditions générales d'utilisation par l'utilisateur.
        /// </summary>
        /// <returns>Un message de succès</returns>
        [HttpPost("{userId}/terms")]
        [Authorize]
        public IActionResult AcceptTerms()
        {
            // TODO: Implémenter la logique d'enregistrement de l'acceptation des termes
            // 1. Vérifier que l'userId correspond à celui dans le JWT
            // 2. Valider que les termes ont été acceptés
            // 3. Mettre à jour le statut de l'utilisateur dans la base de données pour indiquer que les termes ont été acceptés
            // 4. Activer automatiquement le compte utilisateur après l'acceptation des termes

            return Ok(new { message = "Terms and conditions accepted, account activated" });
        }
    }

}
