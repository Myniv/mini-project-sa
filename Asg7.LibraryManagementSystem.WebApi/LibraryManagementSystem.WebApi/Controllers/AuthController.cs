using LibraryManagementSystem.Domain.Models.Entities;
using LibraryManagementSystem.Domain.Models.Requests;
using LibraryManagementSystem.Domain.Service;
using LibraryManagementSystem.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;

namespace LibraryManagementSystem.WebApi.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _environment;

        public AuthController(IAuthService authService,
            ITokenService tokenService,
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IWebHostEnvironment environment)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userManager = userManager;
            _emailService = emailService;
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));

        }


        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.Register(model);
            /*            if(response.Status == false)
                        {
                            return BadRequest(response.Message);
                        }*/

            return Ok(response);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUserLogin model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.Login(model);
            if (response.Status == false)
            {
                return BadRequest(response.Message);
            }

            SetRefreshTokenCookie("AuthToken", response.Token, response.RefreshTokenExpiredOn);
            SetRefreshTokenCookie("RefreshToken", response.RefreshToken, response.RefreshTokenExpiration);


            return Ok(response);
        }

        private void SetRefreshTokenCookie(string tokenType, string? token, DateTime? expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  // Hanya dapat diakses oleh server
                Secure = true,    // Hanya dikirim melalui HTTPS
                SameSite = SameSiteMode.Strict, // Cegah serangan CSRF
                Expires = DateTime.Now.AddDays(3) // Waktu kadaluarsa token
            };
            Response.Cookies.Append(tokenType, token, cookieOptions);
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] AppUserLogout model)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            // var response = await _authService.Logout(model.RefreshToken);
            // if (response.Status == false)
            // {
            //     return BadRequest(response.Message);
            // }

            // return Ok(response);
            try
            {
                // Hapus cookie
                Response.Cookies.Delete("AuthToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
                return Ok("Logout successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during logout");
            }
        }
        // POST: api/Auth/role
        [HttpPost("role")]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleRequest role)
        {
            var result = await _authService.CreateRole(role.RoleName);
            return Ok(result);
        }

        // POST: api/Auth/RefreshToken
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate the refresh token request.
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("Refresh token is required.");  // Return bad request if no refresh token is provided.
            }
            try
            {
                // Retrieve the username associated with the provided refresh token.
                var username = await _tokenService.RetrieveUsernameByRefreshToken(request.RefreshToken);
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized("Invalid refresh token.");  // Return unauthorized if no username is found (invalid or expired token).
                }
                // Retrieve the user by username.
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return Unauthorized("Invalid user.");  // Return unauthorized if no user is found.
                }
                // Issue a new access token and refresh token for the user.
                var accessToken = await _tokenService.IssueAccessToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                // Save the new refresh token.
                var newRt = await _tokenService.SaveRefreshToken(user.UserName, newRefreshToken);
                // Return the new access and refresh tokens.
                return Ok(new { Token = accessToken.Token, TokenExpiredOn = accessToken.ExpiredOn, RefreshToken = newRefreshToken, RefreshTokenExpiredOn = newRt.ExpiryDate });
            }
            catch (Exception ex)
            {
                // Handle any exceptions during the refresh process.
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Return a 500 internal server error on exception.
            }
        }

        // POST: api/Auth/RevokeToken
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            // Validate the revocation request.
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("Refresh token is required.");  // Return bad request if no refresh token is provided.
            }
            try
            {
                // Attempt to revoke the refresh token.
                var result = await _tokenService.RevokeRefreshToken(request.RefreshToken);
                if (!result)
                {
                    return NotFound("Refresh token not found.");  // Return not found if the token does not exist.
                }
                return Ok("Token revoked.");  // Return success message if the token is successfully revoked.
            }
            catch (Exception ex)
            {
                // Handle any exceptions during the revocation process.
                return StatusCode(500, $"Internal server error: {ex.Message}");  // Return a 500 internal server error on exception.
            }
        }

        // POST: api/Auth/email
        [HttpPost("email")]
        public IActionResult SendEmail([FromBody] MailData request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var emailBody = System.IO.File.ReadAllText(@"./EmailTemplate.html");

            emailBody = string.Format(emailBody,
                    "Library Management System",
                    DateTime.UtcNow
                );
            request.EmailBody = emailBody;
            var response = _emailService.SendMail(request);
            return Ok(response);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                long MaxFileSize = 2 * 1024 * 1024; // 2MB
                string[] AllowedFileTypes = new[] {
      "application/pdf",
      "application/msword",
      "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");
                if (file.Length > MaxFileSize)
                    return BadRequest("File size exceeds 2MB limit");
                if (!AllowedFileTypes.Contains(file.ContentType))
                    return BadRequest("Only PDF and Word documents are allowed");
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Save file to directory
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Ok("File uploaded succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // [HttpPost("upload")]
        // public async Task<IActionResult> Upload(IFormFile file)
        // {
        //     try
        //     {
        //         // File constraints
        //         long MaxFileSize = 2 * 1024 * 1024; // 2MB
        //         string[] AllowedFileTypes = new[]
        //         {
        //     "application/pdf",
        //     "application/msword",
        //     "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        // };

        //         // Validate file
        //         if (file == null || file.Length == 0)
        //             return BadRequest("File is empty.");
        //         if (file.Length > MaxFileSize)
        //             return BadRequest("File size exceeds 2MB limit.");
        //         if (!AllowedFileTypes.Contains(file.ContentType))
        //             return BadRequest("Only PDF and Word documents are allowed.");

        //         // Ensure upload directory exists
        //         string uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads");
        //         if (!Directory.Exists(uploadsFolder))
        //             Directory.CreateDirectory(uploadsFolder);

        //         // Generate unique file name and save file
        //         string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
        //         string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //         using (var fileStream = new FileStream(filePath, FileMode.Create))
        //         {
        //             await file.CopyToAsync(fileStream);
        //         }

        //         return Ok(new { Message = "File uploaded successfully.", FilePath = filePath });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }



    }
}
