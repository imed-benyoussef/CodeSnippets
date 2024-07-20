using Aiglusoft.IAM.Infrastructure.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;

namespace Aiglusoft.IAM.Server.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    [Route("[controller]")]
    public class EncryptionController : ControllerBase
    {
        private readonly EncryptionService _encryptionService;

        public EncryptionController()
        {
            _encryptionService = new EncryptionService();
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] string message)
        {
            try
            {
                var publicKeyParameters = _encryptionService.GetPublicKey();
                using (var publicKey = RSA.Create())
                {
                    publicKey.ImportParameters(publicKeyParameters);
                    string encryptedMessage = _encryptionService.Encrypt(message, publicKey);
                    return Ok(new { EncryptedMessage = encryptedMessage });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] JsonElement data)
        {
            try
            {
                string encryptedMessage = data.GetProperty("encryptedMessage").GetString();
                string decryptedMessage = _encryptionService.Decrypt(encryptedMessage);
                return Ok(new { DecryptedMessage = decryptedMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
